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
