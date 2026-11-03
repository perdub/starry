namespace Starry.Infrastructure.Network;

public sealed record NodeNetworkReport(
    string NodeName,
    string Host,
    int HostId,
    string MeshIpv6,
    IReadOnlyList<NodeInterfaceInfo> Interfaces);
