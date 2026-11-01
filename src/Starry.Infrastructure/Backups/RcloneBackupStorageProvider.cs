using Starry.Core.Backups;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Backups;

/// <summary>
/// Streaming backup provider using rclone over Unix pipes without creating temp files on the VPS disk
/// (tar -C volume -cf - . | zstd -3 | rclone rcat remote:bucket/backupId.tar.zst).
/// </summary>
public sealed class RcloneBackupStorageProvider : IBackupStorageProvider
{
    private readonly string _remoteDestination;

    public string TypeName => "rclone";

    public RcloneBackupStorageProvider(string remoteDestination)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteDestination);
        _remoteDestination = remoteDestination.TrimEnd('/');
    }

    public async Task StreamBackupAsync(
        INodeConnection node,
        string volumePath,
        string backupId,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(volumePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(backupId);

        var remoteTarget = $"{_remoteDestination}/{backupId}.tar.zst";

        // Pure in-memory pipe: tar -> zstd -> rclone rcat
        var pipeCmd = $"tar -C '{volumePath}' -cf - . | zstd -3 | rclone rcat '{remoteTarget}'";

        var backupSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(backupSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming backup for volume '{volumePath}' failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task StreamRestoreAsync(
        INodeConnection node,
        string backupId,
        string targetVolumePath,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(backupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetVolumePath);

        var remoteTarget = $"{_remoteDestination}/{backupId}.tar.zst";

        // Pure in-memory restore pipe: rclone cat -> zstd -d -> tar -C target -xf -
        var pipeCmd = $"mkdir -p '{targetVolumePath}' && rclone cat '{remoteTarget}' | zstd -d | tar -C '{targetVolumePath}' -xf -";

        var restoreSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(restoreSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming restore for backup '{backupId}' failed on '{node.NodeName}': {result.StandardError}");
        }
    }
}
