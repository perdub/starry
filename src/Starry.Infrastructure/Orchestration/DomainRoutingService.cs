using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Dsl;
using Starry.Infrastructure.Dns;

namespace Starry.Infrastructure.Orchestration;

public sealed record ResolvedDomainRoute(
    string FullHostname,
    string ZoneDomain,
    string? HttpTarget,
    string? SniTarget,
    bool ProxyProtocol,
    string? TargetNode,
    string? DnsProviderKey,
    int EntryPort);

/// <summary>
/// Traverses hierarchical domains graph (TLD -> domain -> subdomains), resolving inherited properties,
/// Cloudflare Origin Rules, and Nginx L4 SNI / L7 reverse-proxy routes.
/// </summary>
public static class DomainRoutingService
{
    public static List<ResolvedDomainRoute> TraverseDomains(ConfigTree config)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (!config.Root.TryGetPropertyValue("domains", out var domNode) || domNode is not JsonObject domObj)
        {
            return [];
        }

        var results = new List<ResolvedDomainRoute>();
        foreach (var (tld, tldVal) in domObj)
        {
            if (tldVal is JsonObject tldObj)
            {
                TraverseBranch(tldObj, [tld], inheritedNode: null, inheritedProvider: null, inheritedPort: 443, config, results);
            }
        }

        return results;
    }

    private static void TraverseBranch(
        JsonObject currentObj,
        List<string> pathSegments,
        string? inheritedNode,
        string? inheritedProvider,
        int inheritedPort,
        ConfigTree config,
        List<ResolvedDomainRoute> results)
    {
        var node = currentObj["node"]?.ToString() ?? inheritedNode;
        var provider = currentObj["dns_provider"]?.ToString() ?? inheritedProvider;
        var port = ResolvePort(currentObj["port"], inheritedPort, config);

        var isLeaf = currentObj.ContainsKey("http") || currentObj.ContainsKey("sni");

        if (isLeaf)
        {
            var hostname = BuildHostname(pathSegments);
            var zone = ResolveZoneDomain(hostname);
            var http = currentObj["http"]?.ToString();
            var sni = currentObj["sni"]?.ToString();
            var proxyProtocol = bool.TryParse(currentObj["proxy_protocol"]?.ToString(), out var pp) && pp;

            results.Add(new ResolvedDomainRoute(
                FullHostname: hostname,
                ZoneDomain: zone,
                HttpTarget: http,
                SniTarget: sni,
                ProxyProtocol: proxyProtocol,
                TargetNode: node,
                DnsProviderKey: provider,
                EntryPort: port));
        }

        foreach (var (k, v) in currentObj)
        {
            if (k is "node" or "dns_provider" or "cert" or "port" or "http" or "sni" or "proxy_protocol" or "proxied")
            {
                continue;
            }

            if (v is JsonObject childObj)
            {
                var nextSegments = new List<string>(pathSegments) { k };
                TraverseBranch(childObj, nextSegments, node, provider, port, config, results);
            }
        }
    }

    private static string BuildHostname(List<string> segments)
    {
        var filtered = segments.Where(s => s != "@").Reverse().ToList();
        return string.Join(".", filtered);
    }

    private static string ResolveZoneDomain(string hostname)
    {
        var parts = hostname.Split('.');
        if (parts.Length <= 2)
        {
            return hostname;
        }

        return $"{parts[^2]}.{parts[^1]}";
    }

    private static int ResolvePort(JsonNode? portNode, int defaultPort, ConfigTree config)
    {
        if (portNode == null)
        {
            return defaultPort;
        }

        if (portNode is JsonObject portObj)
        {
            var from = portObj["from"]?.ToString();
            if (!string.IsNullOrEmpty(from))
            {
                var colon = from.LastIndexOf(':');
                if (colon > 0 && int.TryParse(from[(colon + 1)..], CultureInfo.InvariantCulture, out var parsed))
                {
                    return parsed;
                }
            }
        }

        var portStr = portNode.ToString();
        if (int.TryParse(portStr, CultureInfo.InvariantCulture, out var p))
        {
            return p;
        }

        if (portStr.StartsWith("$ports.", StringComparison.OrdinalIgnoreCase))
        {
            var ruleKey = portStr.Substring("$ports.".Length);
            var fromStr = config.Root["ports"]?[ruleKey]?["from"]?.ToString();
            if (!string.IsNullOrEmpty(fromStr))
            {
                var colon = fromStr.LastIndexOf(':');
                if (colon > 0 && int.TryParse(fromStr[(colon + 1)..], CultureInfo.InvariantCulture, out var parsed))
                {
                    return parsed;
                }
            }
        }

        return defaultPort;
    }

    public static async Task SynchronizeDomainsAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        IDnsProvider? dnsProvider,
        Action<string>? logger,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(nodes);

        var routes = TraverseDomains(config);
        if (routes.Count == 0)
        {
            return;
        }

        var nginxRoutes = new List<Generators.NginxRoute>();

        foreach (var r in routes)
        {
            if (!string.IsNullOrEmpty(r.SniTarget))
            {
                nginxRoutes.Add(Generators.NginxRoute.NewSniPassthrough(r.FullHostname, r.SniTarget, r.ProxyProtocol));
            }
            else if (!string.IsNullOrEmpty(r.HttpTarget))
            {
                var parts = r.HttpTarget.Split(':');
                var host = parts[0];
                var port = parts.Length > 1 && int.TryParse(parts[1], CultureInfo.InvariantCulture, out var p) ? p : 80;

                nginxRoutes.Add(Generators.NginxRoute.NewHttpReverseProxy(
                    r.FullHostname,
                    host,
                    port,
                    Microsoft.FSharp.Core.FSharpOption<string>.None,
                    Microsoft.FSharp.Core.FSharpOption<string>.None));
            }

            if (dnsProvider != null)
            {
                var targetIp = "127.0.0.1";
                if (!string.IsNullOrEmpty(r.TargetNode))
                {
                    var cleanNode = r.TargetNode.StartsWith("$nodes.", StringComparison.OrdinalIgnoreCase)
                        ? r.TargetNode.Substring("$nodes.".Length)
                        : r.TargetNode;

                    var nodeConn = nodes.FirstOrDefault(n => string.Equals(n.NodeName, cleanNode, StringComparison.OrdinalIgnoreCase));
                    if (nodeConn != null)
                    {
                        targetIp = nodeConn.Host;
                    }
                }

                logger?.Invoke($"Registering DNS record for '{r.FullHostname}' in zone '{r.ZoneDomain}' -> {targetIp}...");
                var record = new DnsRecord(Guid.NewGuid().ToString("N"), "A", r.FullHostname, targetIp, 120, Proxied: true);
                await dnsProvider.UpsertRecordAsync(r.ZoneDomain, record, ct).ConfigureAwait(false);

                if (dnsProvider is CloudflareDnsProvider cf && r.EntryPort != 443)
                {
                    logger?.Invoke($"Configuring Cloudflare Origin Rule for '{r.FullHostname}' rewriting origin port to {r.EntryPort}...");
                    await cf.ConfigureOriginRuleAsync(r.ZoneDomain, r.FullHostname, r.EntryPort, ct).ConfigureAwait(false);
                }
            }
        }

        // If ingress routes were compiled, generate Nginx config
        if (nginxRoutes.Count > 0)
        {
            var nginxConf = Generators.generateNginxConfig(1, Microsoft.FSharp.Collections.ListModule.OfSeq(nginxRoutes));
            logger?.Invoke($"Compiled Nginx Ingress proxy config for {nginxRoutes.Count} route(s).");
            if (nodes.Count > 0)
            {
                var targetNode = nodes[0];
                await targetNode.ExecuteCommandAsync(new ProcessSpec
                {
                    Executable = "sh",
                    Arguments = ["-c", "mkdir -p /etc/nginx && cat > /etc/nginx/nginx.conf && (nginx -s reload 2>/dev/null || true)"],
                    StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(nginxConf)
                }, ct).ConfigureAwait(false);
            }
        }
    }
}
