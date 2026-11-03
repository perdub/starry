using Starry.Core.Backups;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Backups;

/// <summary>
/// Streaming S3 backup provider executing piped tar -> zstd -> S3 commands
/// without writing temporary tarballs to the VPS disk.
/// </summary>
public sealed class S3BackupStorageProvider : IBackupStorageProvider
{
    private readonly S3StorageOptions _options;

    public string TypeName => "s3";

    public S3BackupStorageProvider(S3StorageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.Bucket))
        {
            throw new ArgumentException("Bucket must not be null or whitespace.", nameof(options));
        }
        _options = options;
    }

    public async Task<bool> HasBackupAsync(INodeConnection node, string backupId, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(backupId);

        var targetPath = BuildS3TargetPath(backupId);
        var env = BuildEnvironmentVariables();

        var checkSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"rclone size '{targetPath}' 2>/dev/null || aws s3 ls '{targetPath}' 2>/dev/null"],
            EnvironmentVariables = env,
            Timeout = TimeSpan.FromSeconds(15)
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        return result.IsSuccess && !result.StandardOutput.Contains("Total objects: 0", StringComparison.OrdinalIgnoreCase);
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

        var targetPath = BuildS3TargetPath(backupId);
        var env = BuildEnvironmentVariables();

        var pipeCmd = $"tar -C '{volumePath}' -cf - . | zstd -3 | (rclone rcat '{targetPath}' 2>/dev/null || aws s3 cp - '{targetPath}')";

        var backupSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            EnvironmentVariables = env,
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(backupSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming S3 backup for volume '{volumePath}' failed on '{node.NodeName}': {result.StandardError}");
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

        var targetPath = BuildS3TargetPath(backupId);
        var env = BuildEnvironmentVariables();

        var pipeCmd = $"mkdir -p '{targetVolumePath}' && (rclone cat '{targetPath}' 2>/dev/null || aws s3 cp '{targetPath}' -) | zstd -d | tar -C '{targetVolumePath}' -xf -";

        var restoreSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            EnvironmentVariables = env,
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(restoreSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming S3 restore for backup '{backupId}' failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    private string BuildS3TargetPath(string backupId)
    {
        var cleanPrefix = string.IsNullOrWhiteSpace(_options.Prefix) ? string.Empty : $"{_options.Prefix.Trim('/')}/";
        return $":s3:{_options.Bucket}/{cleanPrefix}{backupId}.tar.zst";
    }

    private Dictionary<string, string> BuildEnvironmentVariables()
    {
        var env = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["RCLONE_CONFIG_S3_TYPE"] = "s3",
            ["RCLONE_CONFIG_S3_PROVIDER"] = "AWS",
            ["RCLONE_CONFIG_S3_REGION"] = _options.Region ?? "us-east-1"
        };

        if (!string.IsNullOrEmpty(_options.Endpoint))
        {
            env["RCLONE_CONFIG_S3_ENDPOINT"] = _options.Endpoint;
            env["AWS_ENDPOINT_URL"] = _options.Endpoint;
        }

        if (!string.IsNullOrEmpty(_options.AccessKey))
        {
            env["RCLONE_CONFIG_S3_ACCESS_KEY_ID"] = _options.AccessKey;
            env["AWS_ACCESS_KEY_ID"] = _options.AccessKey;
        }

        if (!string.IsNullOrEmpty(_options.SecretKey))
        {
            env["RCLONE_CONFIG_S3_SECRET_ACCESS_KEY"] = _options.SecretKey;
            env["AWS_SECRET_ACCESS_KEY"] = _options.SecretKey;
        }

        return env;
    }
}
