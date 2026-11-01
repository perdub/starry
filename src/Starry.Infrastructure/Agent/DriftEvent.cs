namespace Starry.Infrastructure.Agent;

public sealed record DriftEvent(
    string ContainerId,
    string ContainerName,
    string Reason,
    DateTime TimestampUtc,
    bool RestartTriggered = true,
    bool RestartSucceeded = false,
    string? Details = null);
