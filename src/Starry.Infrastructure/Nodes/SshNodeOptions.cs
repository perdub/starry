namespace Starry.Infrastructure.Nodes;

public sealed record SshNodeOptions
{
    public required string NodeName { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; } = 22;
    public string User { get; init; } = "root";
    public string? PrivateKeyContent { get; init; }
    public string? JumpHost { get; init; }
    public string ControlPathDir { get; init; } = Path.Combine(Path.GetTempPath(), "starry-ssh");
}
