using Starry.Infrastructure.Packaging;
using Xunit;

namespace Starry.UnitTests;

public class PackageManagerTests
{
    private readonly PackageManagerFactory _factory = new();

    [Theory]
    [InlineData("ubuntu", typeof(AptPackageManager))]
    [InlineData("debian", typeof(AptPackageManager))]
    [InlineData("almalinux", typeof(DnfPackageManager))]
    [InlineData("rocky", typeof(DnfPackageManager))]
    [InlineData("fedora", typeof(DnfPackageManager))]
    [InlineData("rhel", typeof(DnfPackageManager))]
    [InlineData("alpine", typeof(ApkPackageManager))]
    [InlineData("arch", typeof(PacmanPackageManager))]
    public void Factory_ResolvesCorrectManager(string distroId, Type expectedType)
    {
        var manager = _factory.Resolve(distroId);
        Assert.IsType(expectedType, manager);
    }

    [Fact]
    public void Factory_ThrowsOnUnsupportedDistro()
    {
        Assert.Throws<NotSupportedException>(() => _factory.Resolve("templeos"));
    }

    [Fact]
    public void AptPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard", AptPackageManager.MapLogicalPackage("wireguard"));
        Assert.Equal("nftables", AptPackageManager.MapLogicalPackage("nftables"));
        Assert.Equal("curl", AptPackageManager.MapLogicalPackage("curl"));
    }

    [Fact]
    public void DnfPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", DnfPackageManager.MapLogicalPackage("wireguard"));
        Assert.Equal("nftables", DnfPackageManager.MapLogicalPackage("nftables"));
    }

    [Fact]
    public void ApkPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", ApkPackageManager.MapLogicalPackage("wireguard"));
    }

    [Fact]
    public void PacmanPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", PacmanPackageManager.MapLogicalPackage("wireguard"));
    }
}
