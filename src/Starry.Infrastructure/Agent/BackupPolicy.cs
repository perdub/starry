namespace Starry.Infrastructure.Agent;

public sealed record BackupPolicy(
    string ServiceName,
    string VolumePath,
    TimeSpan Interval,
    string? RemoteDestination = null,
    string? BackupPrefix = null);
