using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;
using Starry.Infrastructure.Packaging;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Orchestrates cross-node operations such as rolling OS security upgrades and SSH key rotation.
/// </summary>
public sealed class NodeOperationsService
{
    private readonly PackageManagerFactory _pkgFactory = new();

    public async Task<bool> UpgradeNodeAsync(
        INodeConnection node,
        bool fullUpgrade = false,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        logger?.Invoke($"Connecting to node '{node.NodeName}' ({node.Host})...");
        var pkgManager = _pkgFactory.ResolveForNode(node);

        logger?.Invoke($"Applying security updates using {pkgManager.GetType().Name}...");
        await pkgManager.ApplySecurityUpdatesAsync(node, fullUpgrade, ct).ConfigureAwait(false);

        var rebootRequired = await pkgManager.IsRebootRequiredAsync(node, ct).ConfigureAwait(false);
        if (rebootRequired)
        {
            logger?.Invoke($"Node '{node.NodeName}' requires a reboot. Draining containers before rolling restart...");
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", "docker stop $(docker ps -q) 2>/dev/null || podman stop $(podman ps -q) 2>/dev/null || true"],
                Timeout = TimeSpan.FromMinutes(2)
            }, ct).ConfigureAwait(false);

            logger?.Invoke($"Initiating rolling reboot on '{node.NodeName}'...");
            await RebootAndWaitAsync(node, logger, ct).ConfigureAwait(false);
            logger?.Invoke($"Node '{node.NodeName}' successfully rebooted and online.");
        }
        else
        {
            logger?.Invoke($"Node '{node.NodeName}' update complete, no reboot required.");
        }

        return true;
    }

    public static async Task<string> RotateSshKeysAsync(
        INodeConnection node,
        string oldPublicKey,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(oldPublicKey);

        logger?.Invoke($"Generating new Ed25519 key pair for node '{node.NodeName}'...");

        var (privateKey, publicKey) = await GenerateKeyPairAsync(node.NodeName, ct).ConfigureAwait(false);

        logger?.Invoke("Adding new public key to remote authorized_keys...");
        await DeployNewKeyAsync(node, publicKey, ct).ConfigureAwait(false);

        logger?.Invoke("Verifying connection with the new key...");
        await VerifyNewKeyConnectionAsync(node, privateKey, ct).ConfigureAwait(false);

        logger?.Invoke("Removing obsolete public key from remote authorized_keys...");
        await RemoveOldKeyAsync(node, oldPublicKey, ct).ConfigureAwait(false);

        logger?.Invoke($"SSH key rotation for '{node.NodeName}' completed successfully!");
        return privateKey;
    }

    private static async Task<(string PrivateKey, string PublicKey)> GenerateKeyPairAsync(string nodeName, CancellationToken ct)
    {
        var tempKeyDir = Path.Combine(Path.GetTempPath(), $"starry-rotate-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempKeyDir);
        var keyPath = Path.Combine(tempKeyDir, "id_ed25519");

        try
        {
            var genResult = await new Processes.ProcessRunner().RunAsync(new ProcessSpec
            {
                Executable = "ssh-keygen",
                Arguments = ["-t", "ed25519", "-N", "", "-f", keyPath, "-C", $"starry@{nodeName}-{DateTime.UtcNow:yyyyMMdd}"]
            }, ct).ConfigureAwait(false);

            if (!genResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to generate new SSH key: {genResult.StandardError}");
            }

            var privateKey = await File.ReadAllTextAsync(keyPath, ct).ConfigureAwait(false);
            var publicKey = (await File.ReadAllTextAsync(keyPath + ".pub", ct).ConfigureAwait(false)).Trim();

            return (privateKey, publicKey);
        }
        finally
        {
            if (Directory.Exists(tempKeyDir))
            {
                Directory.Delete(tempKeyDir, recursive: true);
            }
        }
    }

    private static async Task DeployNewKeyAsync(INodeConnection node, string publicKey, CancellationToken ct)
    {
        var appendCommand = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"mkdir -p ~/.ssh && chmod 700 ~/.ssh && echo '{publicKey}' >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"]
        };
        await node.ExecuteCommandAsync(appendCommand, ct).ConfigureAwait(false);
    }

    private static async Task VerifyNewKeyConnectionAsync(INodeConnection node, string privateKey, CancellationToken ct)
    {
        var testOptions = new SshNodeOptions
        {
            NodeName = node.NodeName,
            Host = node.Host,
            Port = node.Port,
            PrivateKeyContent = privateKey
        };

        await using var testConn = new SshNodeConnection(testOptions);
        await testConn.InitializeAsync(ct).ConfigureAwait(false);
        var testRes = await testConn.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["starry-key-verified"]
        }, ct).ConfigureAwait(false);

        if (!testRes.IsSuccess || !testRes.StandardOutput.Contains("starry-key-verified", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Failed to verify remote access with new SSH key. Aborting rotation to prevent lockout.");
        }
    }

    private static async Task RemoveOldKeyAsync(INodeConnection node, string oldPublicKey, CancellationToken ct)
    {
        var cleanCommand = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"grep -v '{oldPublicKey.Trim()}' ~/.ssh/authorized_keys > ~/.ssh/authorized_keys.tmp && mv ~/.ssh/authorized_keys.tmp ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"]
        };
        await node.ExecuteCommandAsync(cleanCommand, ct).ConfigureAwait(false);
    }

    private static async Task RebootAndWaitAsync(INodeConnection node, Action<string>? logger, CancellationToken ct)
    {
        try
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "reboot",
                Timeout = TimeSpan.FromSeconds(5)
            }, ct).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Reboot causes SSH connection to drop immediately
        }
        catch (InvalidOperationException)
        {
            // Reboot causes SSH connection to drop immediately
        }

        await Task.Delay(5000, ct).ConfigureAwait(false);

        var retryCount = 0;
        const int maxRetries = 30;
        while (retryCount < maxRetries)
        {
            ct.ThrowIfCancellationRequested();
            retryCount++;
            await Task.Delay(5000, ct).ConfigureAwait(false);

            try
            {
                var checkRes = await node.ExecuteCommandAsync(new ProcessSpec
                {
                    Executable = "uptime",
                    Timeout = TimeSpan.FromSeconds(5)
                }, ct).ConfigureAwait(false);

                if (checkRes.IsSuccess)
                {
                    return;
                }
            }
            catch (TimeoutException)
            {
                logger?.Invoke($"Waiting for node '{node.NodeName}' to come back online (attempt {retryCount}/{maxRetries})...");
            }
            catch (InvalidOperationException)
            {
                logger?.Invoke($"Waiting for node '{node.NodeName}' to come back online (attempt {retryCount}/{maxRetries})...");
            }
        }

        throw new TimeoutException($"Node '{node.NodeName}' failed to come back online after reboot within 150 seconds.");
    }
}
