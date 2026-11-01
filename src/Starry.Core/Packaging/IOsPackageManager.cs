using Starry.Core.Nodes;

namespace Starry.Core.Packaging;

/// <summary>
/// Cross-distribution package manager abstraction (apt, dnf, apk, pacman).
/// </summary>
public interface IOsPackageManager
{
    /// <summary>
    /// Linux distribution identifiers supported by this manager (e.g. ["ubuntu", "debian"]).
    /// </summary>
    IReadOnlyList<string> SupportedDistroIds { get; }

    /// <summary>
    /// Ensures that logical system packages (e.g. wireguard, nftables, curl) are installed.
    /// </summary>
    Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default);

    /// <summary>
    /// Applies security updates or full system upgrades on the node.
    /// </summary>
    Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default);

    /// <summary>
    /// Checks whether the node requires a reboot after package updates.
    /// </summary>
    Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default);
}
