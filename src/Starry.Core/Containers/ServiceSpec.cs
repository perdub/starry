namespace Starry.Core.Containers;

public sealed record ServiceSpec
{
    public required string Name { get; init; }
    public required string Image { get; init; }
    public int Priority { get; init; } = 50;
    public string? VipAddress { get; init; }
    public int Replicas { get; init; } = 1;
    public string? TargetNode { get; init; }
    public IReadOnlyList<string> TargetNodes { get; init; } = [];
    public IReadOnlyList<string> Networks { get; init; } = ["default"];
    public string? Egress { get; init; } = "true";

    public IReadOnlyList<string> Ports { get; init; } = [];
    public IReadOnlyList<string> Volumes { get; init; } = [];
    public IReadOnlyDictionary<string, string> Environment { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Files { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public string? HealthcheckEndpoint { get; init; }

    // Full Docker Compose service parameters
    public string? Command { get; init; }
    public string? Entrypoint { get; init; }
    public string? WorkingDir { get; init; }
    public string? User { get; init; }
    public string? Hostname { get; init; }
    public string? Domainname { get; init; }
    public string Restart { get; init; } = "unless-stopped";
    public bool Privileged { get; init; }
    public IReadOnlyList<string> CapAdd { get; init; } = [];
    public IReadOnlyList<string> CapDrop { get; init; } = [];
    public IReadOnlyList<string> Devices { get; init; } = [];
    public IReadOnlyDictionary<string, string> Sysctls { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyList<string> ExtraHosts { get; init; } = [];
    public string? NetworkMode { get; init; }
    public long? ShmSize { get; init; }
    public string? MemLimit { get; init; }
    public double? Cpus { get; init; }
    public string? StopSignal { get; init; }
    public int? StopGracePeriod { get; init; }
}
