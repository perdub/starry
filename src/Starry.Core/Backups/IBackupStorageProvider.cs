using Starry.Core.Nodes;

namespace Starry.Core.Backups;

/// <summary>
/// Streaming backup storage provider abstraction (Rclone, SFTP-Peer, S3, Local).
/// </summary>
public interface IBackupStorageProvider
{
    string TypeName { get; }
    Task<bool> HasBackupAsync(INodeConnection node, string backupId, CancellationToken ct = default);
    Task StreamBackupAsync(INodeConnection node, string volumePath, string backupId, CancellationToken ct = default);
    Task StreamRestoreAsync(INodeConnection node, string backupId, string targetVolumePath, CancellationToken ct = default);
}
