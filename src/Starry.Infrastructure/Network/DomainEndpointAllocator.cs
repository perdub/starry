using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Allocates external ports and domain endpoints supporting random port generation,
/// explicit or random IPv4/IPv6 allocation, and protocol routing (http://, sni://, tcp://, udp://).
/// </summary>
public static class DomainEndpointAllocator
{
    public static int AllocateRandomPort(HashSet<int>? existingPorts = null)
    {
        var used = existingPorts ?? [];
        for (var attempt = 0; attempt < 500; attempt++)
        {
            var candidate = RandomNumberGenerator.GetInt32(10000, 60000);
            if (!used.Contains(candidate))
            {
                return candidate;
            }
        }
        return RandomNumberGenerator.GetInt32(10000, 60000);
    }

    public static IReadOnlyList<AllocatedPortInfo> ListPorts(ConfigTree tree)
    {
        ArgumentNullException.ThrowIfNull(tree);
        var list = new List<AllocatedPortInfo>();

        if (tree.Root.TryGetPropertyValue("ports", out var portsNode) && portsNode is JsonObject portsObj)
        {
            foreach (var (ruleName, ruleVal) in portsObj)
            {
                if (ruleVal is JsonObject rObj)
                {
                    var from = rObj["from"]?.ToString() ?? string.Empty;
                    var to = rObj["to"]?.ToString() ?? string.Empty;
                    var proto = rObj["proto"]?.ToString() ?? "tcp";
                    var family = rObj["family"]?.ToString() ?? (from.Contains("ipv6", StringComparison.OrdinalIgnoreCase) ? "ipv6" : "ipv4");

                    list.Add(new AllocatedPortInfo(ruleName, from, to, proto, family));
                }
            }
        }

        return list;
    }

    public static AllocatedPortInfo ExposeService(
        ConfigTree tree,
        string ruleName,
        string targetService,
        int targetPort,
        string family = "both",
        int? requestedPort = null,
        string proto = "tcp")
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleName);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetService);
        ArgumentNullException.ThrowIfNull(family);
        ArgumentNullException.ThrowIfNull(proto);

        if (tree.Root["ports"] is not JsonObject portsObj)
        {
            portsObj = new JsonObject();
            tree.Root["ports"] = portsObj;
        }

        var usedPorts = ExtractUsedPorts(portsObj);
        var hostPort = requestedPort is > 0 ? requestedPort.Value : AllocateRandomPort(usedPorts);

        string fromExpr;
        if (string.Equals(family, "ipv6", StringComparison.OrdinalIgnoreCase))
        {
            fromExpr = $"[::]:{hostPort}";
        }
        else if (string.Equals(family, "ipv4", StringComparison.OrdinalIgnoreCase))
        {
            fromExpr = $"0.0.0.0:{hostPort}";
        }
        else
        {
            fromExpr = $":{hostPort}";
        }

        var ruleObj = new JsonObject
        {
            ["from"] = fromExpr,
            ["to"] = $"{targetService}:{targetPort}",
            ["proto"] = proto.ToUpperInvariant(),
            ["family"] = family.ToUpperInvariant()
        };

        portsObj[ruleName] = ruleObj;
        return new AllocatedPortInfo(ruleName, fromExpr, $"{targetService}:{targetPort}", proto, family);
    }

    private static HashSet<int> ExtractUsedPorts(JsonObject portsObj)
    {
        var used = new HashSet<int>();
        foreach (var (_, val) in portsObj)
        {
            if (val is JsonObject obj && obj.TryGetPropertyValue("from", out var fromVal) && fromVal != null)
            {
                var fromStr = fromVal.ToString();
                var colon = fromStr.LastIndexOf(':');
                if (colon >= 0 && int.TryParse(fromStr[(colon + 1)..], CultureInfo.InvariantCulture, out var p))
                {
                    used.Add(p);
                }
            }
        }
        return used;
    }
}
