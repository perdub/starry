namespace Starry.Core.Containers;

public sealed record ServiceSpec
{
    public required string Name { get; init; }
    public required string Image { get; init; }
    public int Priority { get; init; } = 50;
    public string? VipAddress { get; init; }
    public IReadOnlyList<string> Ports { get; init; } = [];
    public IReadOnlyList<string> Volumes { get; init; } = [];
    public IReadOnlyDictionary<string, string> Environment { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Files { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public string? HealthcheckEndpoint { get; init; }
}
