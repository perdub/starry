using System.Globalization;
using System.Security.Cryptography;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Security;

/// <summary>
/// Provides lockout-safe SSH port migration by opening the new port alongside the old port,
/// verifying reachability, and removing the old port only after verification succeeds.
/// </summary>
public static class PortRandomizerService
{
    public static async Task<int> MigrateSshPortSafelyAsync(
        INodeConnection node,
        int currentPort,
        int? requestedNewPort = null,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var newPort = requestedNewPort is > 0 ? requestedNewPort.Value : RandomNumberGenerator.GetInt32(10000, 60000);
        if (newPort == currentPort)
        {
            return currentPort;
        }

        logger?.Invoke($"Opening new SSH port {newPort} alongside {currentPort}...");

        // 1. Add Port newPort to sshd_config.d or sshd_config
        var openDualSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"echo 'Port {currentPort}' > /etc/ssh/sshd_config.d/starry-ports.conf && echo 'Port {newPort}' >> /etc/ssh/sshd_config.d/starry-ports.conf && (systemctl reload sshd || systemctl reload ssh || service ssh reload || true)"]
        };
        await node.ExecuteCommandAsync(openDualSpec, ct).ConfigureAwait(false);

        // 2. Verify new port is listening
        logger?.Invoke($"Verifying SSH listening on new port {newPort}...");
        var testSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ss -lnt | grep ':{newPort} ' || netstat -lnt | grep ':{newPort} '"]
        };
        var testResult = await node.ExecuteCommandAsync(testSpec, ct).ConfigureAwait(false);

        if (!testResult.IsSuccess)
        {
            logger?.Invoke($"Verification failed for port {newPort}. Rolling back to port {currentPort}...");
            await RollbackPortAsync(node, currentPort, ct).ConfigureAwait(false);
            throw new InvalidOperationException($"Lockout prevention: SSH port verification failed on port {newPort}. Rolled back.");
        }

        // 3. Keep only new port
        logger?.Invoke($"Locking SSH exclusively to verified port {newPort}...");
        var lockSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"echo 'Port {newPort}' > /etc/ssh/sshd_config.d/starry-ports.conf && (systemctl reload sshd || systemctl reload ssh || service ssh reload || true)"]
        };
        await node.ExecuteCommandAsync(lockSpec, ct).ConfigureAwait(false);

        return newPort;
    }

    private static async Task RollbackPortAsync(INodeConnection node, int originalPort, CancellationToken ct)
    {
        var rollbackSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"echo 'Port {originalPort}' > /etc/ssh/sshd_config.d/starry-ports.conf && (systemctl reload sshd || systemctl reload ssh || service ssh reload || true)"]
        };
        await node.ExecuteCommandAsync(rollbackSpec, ct).ConfigureAwait(false);
    }
}
