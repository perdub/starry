using Starry.Core.Nodes;
using Starry.Core.Packaging;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// Factory to resolve the appropriate package manager for any given Linux distribution.
/// </summary>
public sealed class PackageManagerFactory
{
    private readonly List<IOsPackageManager> _managers =
    [
        new AptPackageManager(),
        new DnfPackageManager(),
        new ApkPackageManager(),
        new PacmanPackageManager()
    ];

    public IOsPackageManager Resolve(string distroId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(distroId);

        var trimmed = distroId.Trim();
        var matched = _managers.FirstOrDefault(manager =>
            manager.SupportedDistroIds.Any(id => string.Equals(id, trimmed, StringComparison.OrdinalIgnoreCase)));

        if (matched != null)
        {
            return matched;
        }

        throw new NotSupportedException($"Unsupported Linux distribution '{distroId}'. Supported distributions: Debian/Ubuntu (apt), RHEL/CentOS/AlmaLinux/Rocky/Fedora (dnf), Alpine (apk), Arch (pacman).");
    }

    public IOsPackageManager ResolveForNode(INodeConnection node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var distro = node.DistroId ?? "ubuntu";
        return Resolve(distro);
    }
}
