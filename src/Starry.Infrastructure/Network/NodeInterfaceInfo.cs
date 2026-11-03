namespace Starry.Infrastructure.Network;

public sealed record NodeInterfaceInfo(
    string InterfaceName,
    IReadOnlyList<string> Ipv4Addresses,
    IReadOnlyList<string> Ipv6Addresses,
    string? ExternalSeenIpv4,
    string? ExternalSeenIpv6,
    InterfaceClassification Classification,
    bool IsReachableFromPeers);
