namespace Starry.Infrastructure.Network;

public sealed record IdLedgerState
{
    public int NextHostId { get; set; } = 1;
    public Dictionary<string, int> AllocatedHosts { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public IList<int> TombstonedHostIds { get; init; } = [];

    public int NextContainerId { get; set; } = 1;
    public Dictionary<string, int> AllocatedContainers { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public IList<int> TombstonedContainerIds { get; init; } = [];
}
