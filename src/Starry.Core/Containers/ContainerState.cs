namespace Starry.Core.Containers;

public sealed record ContainerState(
    string Id,
    string Name,
    string Image,
    string Status,
    bool IsRunning,
    int ExitCode,
    DateTime CreatedAt,
    IReadOnlyDictionary<string, string> Labels);
