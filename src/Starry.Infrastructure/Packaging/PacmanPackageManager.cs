using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// Pacman package manager implementation for Arch Linux and derivative distributions.
/// </summary>
public sealed class PacmanPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["arch", "manjaro", "endeavouros"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var installArgs = new List<string> { "-Sy", "--noconfirm", "--needed" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "pacman",
            Arguments = installArgs
        };

        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Pacman package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeSpec = new ProcessSpec
        {
            Executable = "pacman",
            Arguments = ["-Syu", "--noconfirm"]
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Pacman system upgrade failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "test",
            Arguments = ["-f", "/var/run/reboot-required"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        return result.ExitCode == 0;
    }

    public static string MapLogicalPackage(string logicalPackage)
    {
        return logicalPackage switch
        {
            "wireguard" => "wireguard-tools",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            "docker" => "docker",
            "podman" => "podman",
            _ => logicalPackage
        };
    }
}
