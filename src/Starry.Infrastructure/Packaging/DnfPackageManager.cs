using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// DNF / YUM package manager implementation for RHEL, AlmaLinux, Rocky Linux, and Fedora.
/// </summary>
public sealed class DnfPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["rhel", "almalinux", "rocky", "fedora", "centos"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var installArgs = new List<string> { "install", "-y", "-q" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "dnf",
            Arguments = installArgs
        };

        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"DNF package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeArgs = fullUpgrade
            ? new[] { "upgrade", "-y", "-q" }
            : new[] { "update", "--security", "-y", "-q" };

        var upgradeSpec = new ProcessSpec
        {
            Executable = "dnf",
            Arguments = upgradeArgs
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"DNF security update failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "needs-restarting",
            Arguments = ["-r"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        // needs-restarting -r returns 1 when reboot is required
        return result.ExitCode == 1;
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
