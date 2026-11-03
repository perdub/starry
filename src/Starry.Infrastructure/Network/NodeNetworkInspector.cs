using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Probes and classifies network interfaces and external reachability on cluster nodes.
/// </summary>
public static class NodeNetworkInspector
{
    public static async Task<NodeNetworkReport> InspectNodeAsync(
        INodeConnection node,
        int hostId,
        bool verifyReachability = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var interfaces = new List<NodeInterfaceInfo>();

        var ipResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "ip",
            Arguments = ["-j", "addr", "show"],
            Timeout = TimeSpan.FromSeconds(10)
        }, ct).ConfigureAwait(false);

        var (extIpv4, extIpv6) = await QueryExternalIpsAsync(node, ct).ConfigureAwait(false);

        if (ipResult.IsSuccess && !string.IsNullOrWhiteSpace(ipResult.StandardOutput))
        {
            try
            {
                if (JsonNode.Parse(ipResult.StandardOutput) is JsonArray jsonNode)
                {
                    foreach (var item in jsonNode)
                    {
                        if (item is JsonObject ifaceObj)
                        {
                            var info = ParseInterfaceInfo(ifaceObj, extIpv4, extIpv6);
                            if (info != null)
                            {
                                interfaces.Add(info);
                            }
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Fallback to basic interface
            }
        }

        if (interfaces.Count == 0)
        {
            interfaces.Add(new NodeInterfaceInfo(
                InterfaceName: "eth0",
                Ipv4Addresses: [node.Host],
                Ipv6Addresses: [],
                ExternalSeenIpv4: extIpv4 ?? node.Host,
                ExternalSeenIpv6: extIpv6,
                Classification: InterfaceClassification.DedicatedPublic,
                IsReachableFromPeers: true));
        }

        var meshIp = IdLedgerService.FormatHostWireGuardIp(hostId);
        return new NodeNetworkReport(node.NodeName, node.Host, hostId, meshIp, interfaces);
    }

    private static async Task<(string? Ipv4, string? Ipv6)> QueryExternalIpsAsync(INodeConnection node, CancellationToken ct)
    {
        var extIpv4Result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-4s", "--max-time", "3", "https://api.ipify.org"],
            Timeout = TimeSpan.FromSeconds(5)
        }, ct).ConfigureAwait(false);

        var extIpv6Result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-6s", "--max-time", "3", "https://api6.ipify.org"],
            Timeout = TimeSpan.FromSeconds(5)
        }, ct).ConfigureAwait(false);

        var extIpv4 = extIpv4Result.IsSuccess ? extIpv4Result.StandardOutput.Trim() : null;
        var extIpv6 = extIpv6Result.IsSuccess ? extIpv6Result.StandardOutput.Trim() : null;

        return (extIpv4, extIpv6);
    }

    private static NodeInterfaceInfo? ParseInterfaceInfo(JsonObject ifaceObj, string? extIpv4, string? extIpv6)
    {
        var ifname = ifaceObj["ifname"]?.ToString() ?? "unknown";
        if (ifname.Equals("lo", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var v4List = new List<string>();
        var v6List = new List<string>();

        if (ifaceObj["addr_info"] is JsonArray addrs)
        {
            foreach (var a in addrs)
            {
                var family = a?["family"]?.ToString();
                var local = a?["local"]?.ToString();
                var prefixlen = a?["prefixlen"]?.ToString();

                if (string.IsNullOrEmpty(local)) continue;

                var withPrefix = !string.IsNullOrEmpty(prefixlen) ? $"{local}/{prefixlen}" : local;

                if (string.Equals(family, "inet", StringComparison.OrdinalIgnoreCase))
                {
                    v4List.Add(withPrefix);
                }
                else if (string.Equals(family, "inet6", StringComparison.OrdinalIgnoreCase) &&
                         !local.StartsWith("fe80", StringComparison.OrdinalIgnoreCase))
                {
                    v6List.Add(withPrefix);
                }
            }
        }

        var classification = ClassifyInterface(v4List, v6List, extIpv4, extIpv6);

        return new NodeInterfaceInfo(
            InterfaceName: ifname,
            Ipv4Addresses: v4List,
            Ipv6Addresses: v6List,
            ExternalSeenIpv4: extIpv4,
            ExternalSeenIpv6: extIpv6,
            Classification: classification,
            IsReachableFromPeers: classification == InterfaceClassification.DedicatedPublic);
    }

    private static InterfaceClassification ClassifyInterface(
        List<string> v4List,
        List<string> v6List,
        string? extIpv4,
        string? extIpv6)
    {
        var hasMatchingPublicV4 = !string.IsNullOrEmpty(extIpv4) &&
                                  v4List.Any(v4 => v4.StartsWith(extIpv4, StringComparison.OrdinalIgnoreCase));

        var hasMatchingPublicV6 = !string.IsNullOrEmpty(extIpv6) &&
                                  v6List.Any(v6 => v6.StartsWith(extIpv6, StringComparison.OrdinalIgnoreCase));

        if (hasMatchingPublicV4 || hasMatchingPublicV6)
        {
            return InterfaceClassification.DedicatedPublic;
        }

        if (v6List.Any(v6 => v6.Contains("/64", StringComparison.Ordinal) || v6.Contains("/56", StringComparison.Ordinal)))
        {
            return InterfaceClassification.RoutedIpv6Subnet;
        }

        var hasPrivateV4 = v4List.Any(v4 =>
            v4.StartsWith("10.", StringComparison.Ordinal) ||
            v4.StartsWith("192.168.", StringComparison.Ordinal) ||
            v4.StartsWith("172.16.", StringComparison.Ordinal) ||
            v4.StartsWith("172.17.", StringComparison.Ordinal) ||
            v4.StartsWith("172.18.", StringComparison.Ordinal) ||
            v4.StartsWith("172.19.", StringComparison.Ordinal) ||
            v4.StartsWith("172.2", StringComparison.Ordinal) ||
            v4.StartsWith("172.3", StringComparison.Ordinal) ||
            v4.StartsWith("100.", StringComparison.Ordinal)); // CGNAT

        if (hasPrivateV4)
        {
            return InterfaceClassification.BehindNatOrCgnat;
        }

        return InterfaceClassification.InternalOnly;
    }
}
