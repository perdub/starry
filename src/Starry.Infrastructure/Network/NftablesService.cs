using System.Globalization;
using System.Text;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Service managing host firewall and container VIP port forwarding via nftables (table inet starry).
/// </summary>
public static class NftablesService
{
    public const string TableName = "starry";

    public static string GenerateRuleset(
        IReadOnlyList<PortForwardRule> portForwards,
        IReadOnlyList<int> publicAllowedPorts,
        int sshPort = 22,
        int wireGuardPort = 51820)
    {
        ArgumentNullException.ThrowIfNull(portForwards);
        ArgumentNullException.ThrowIfNull(publicAllowedPorts);

        var sb = new StringBuilder();
        sb.AppendLine("#!/usr/sbin/nft -f");
        sb.AppendLine("# Managed by Starry GitOps Orchestrator - Table: inet starry");
        sb.AppendLine();
        // Idempotent table recreation preventing rule duplication
        sb.AppendLine(CultureInfo.InvariantCulture, $"table inet {TableName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"delete table inet {TableName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"table inet {TableName} {{");

        AppendInputChain(sb, publicAllowedPorts, sshPort, wireGuardPort);
        AppendForwardChain(sb);
        AppendPreroutingChain(sb, portForwards);
        AppendPostroutingChain(sb);

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void AppendInputChain(StringBuilder sb, IReadOnlyList<int> publicAllowedPorts, int sshPort, int wireGuardPort)
    {
        sb.AppendLine("    chain input {");
        sb.AppendLine("        type filter hook input priority filter; policy drop;");
        sb.AppendLine("        ct state established,related accept");
        sb.AppendLine("        iifname \"lo\" accept");
        sb.AppendLine("        iifname \"starry0\" accept");
        sb.AppendLine(CultureInfo.InvariantCulture, $"        tcp dport {sshPort} accept");
        sb.AppendLine(CultureInfo.InvariantCulture, $"        udp dport {wireGuardPort} accept");

        foreach (var port in publicAllowedPorts)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"        tcp dport {port} accept");
            sb.AppendLine(CultureInfo.InvariantCulture, $"        udp dport {port} accept");
        }

        sb.AppendLine("        icmp type echo-request accept");
        sb.AppendLine("        icmpv6 type { echo-request, nd-neighbor-solicit, nd-neighbor-advert, nd-router-advert } accept");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendForwardChain(StringBuilder sb)
    {
        sb.AppendLine("    chain forward {");
        sb.AppendLine("        type filter hook forward priority filter; policy drop;");
        sb.AppendLine("        ct state established,related accept");
        sb.AppendLine("        iifname \"starry0\" accept");
        sb.AppendLine("        oifname \"starry0\" accept");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendPreroutingChain(StringBuilder sb, IReadOnlyList<PortForwardRule> portForwards)
    {
        sb.AppendLine("    chain prerouting {");
        sb.AppendLine("        type nat hook prerouting priority dstnat; policy accept;");
        foreach (var rule in portForwards)
        {
            var proto = string.Equals(rule.Protocol, "udp", StringComparison.OrdinalIgnoreCase) ? "udp" : "tcp";

            if (string.Equals(rule.AddressFamily, "ipv4", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        meta nfproto ipv4 {proto} dport {rule.HostPort} dnat ip to {rule.TargetVip}:{rule.TargetPort}");
            }
            else if (string.Equals(rule.AddressFamily, "ipv6", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        meta nfproto ipv6 {proto} dport {rule.HostPort} dnat ip6 to {rule.TargetVip}:{rule.TargetPort}");
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        {proto} dport {rule.HostPort} dnat to {rule.TargetVip}:{rule.TargetPort}");
            }
        }
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendPostroutingChain(StringBuilder sb)
    {
        sb.AppendLine("    chain postrouting {");
        sb.AppendLine("        type nat hook postrouting priority srcnat; policy accept;");
        sb.AppendLine("        oifname \"starry0\" masquerade");
        sb.AppendLine("    }");
    }

    public static async Task ApplyRulesetAsync(
        INodeConnection node,
        string rulesetContent,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(rulesetContent);

        // Backup existing ruleset for Deadman rollback
        await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "[ -f /etc/nftables.starry.conf ] && cp /etc/nftables.starry.conf /etc/nftables.starry.conf.bak || true"]
        }, ct).ConfigureAwait(false);

        // Upload and apply new ruleset
        var applySpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "cat > /etc/nftables.starry.conf && nft -f /etc/nftables.starry.conf"],
            StandardInputBytes = Encoding.UTF8.GetBytes(rulesetContent)
        };

        var result = await node.ExecuteCommandAsync(applySpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            // Deadman Switch Rollback
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", "[ -f /etc/nftables.starry.conf.bak ] && nft -f /etc/nftables.starry.conf.bak || true"]
            }, ct).ConfigureAwait(false);

            throw new InvalidOperationException($"Failed to apply nftables ruleset on '{node.NodeName}': {result.StandardError}");
        }
    }
}
