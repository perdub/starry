using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// APT package manager implementation for Debian and Ubuntu systems.
/// </summary>
public sealed class AptPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["ubuntu", "debian", "pop", "mint"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var updateSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = ["update", "-qq"],
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };
        await node.ExecuteCommandAsync(updateSpec, ct).ConfigureAwait(false);

        var installArgs = new List<string> { "install", "-y", "-qq" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = installArgs,
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };
        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APT package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeArgs = fullUpgrade
            ? new[] { "dist-upgrade", "-y", "-qq" }
            : new[] { "upgrade", "-y", "-qq", "--with-new-pkgs" };

        var upgradeSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = upgradeArgs,
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APT system update failed on '{node.NodeName}': {result.StandardError}");
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
            "wireguard" => "wireguard",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            _ => logicalPackage
        };
    }
}
