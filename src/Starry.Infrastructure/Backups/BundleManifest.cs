namespace Starry.Infrastructure.Backups;

public sealed record BundleManifest(
    string ClusterName,
    DateTime CreatedAt,
    string StarryVersion,
    IReadOnlyList<string> Nodes);
