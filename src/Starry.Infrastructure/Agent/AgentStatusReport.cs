using Starry.Core.Containers;

namespace Starry.Infrastructure.Agent;

public sealed record AgentStatusReport(
    bool IsStarry0Active,
    string? WireGuardDump,
    IReadOnlyList<WireGuardPeerDump> Peers,
    IReadOnlyList<ContainerState> Containers,
    IReadOnlyList<ContainerState> RunningContainers,
    IReadOnlyList<DriftEvent> RecentDriftEvents,
    DateTime TimestampUtc)
{
    public bool IsInterfaceActive => IsStarry0Active;
    public int TotalContainers => Containers.Count;
    public int TotalRunningContainers => RunningContainers.Count;
}
