using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class WireGuardMeshTests
{
    [Fact]
    public void GenerateMeshConfigurations_GeneratesP2PConfigsWithKeepaliveAndVips()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false),
            new("ryo", "10.99.0.3", "privKey3", "pubKey3", 51820, null, IsPublic: false)
        };

        var containerVips = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal)
        {
            ["nijika"] = ["10.99.10.1", "10.99.10.2"],
            ["ryo"] = ["10.99.20.5"]
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes, containerVips);

        Assert.Equal(3, configs.Count);
        Assert.True(configs.ContainsKey("bocchi"));
        Assert.True(configs.ContainsKey("nijika"));
        Assert.True(configs.ContainsKey("ryo"));

        // Check bocchi config:
        var bocchiConf = configs["bocchi"];
        Assert.Contains("Address = 10.99.0.1/16", bocchiConf, StringComparison.Ordinal);
        Assert.Contains("ListenPort = 51820", bocchiConf, StringComparison.Ordinal);
        Assert.Contains("PersistentKeepalive = 25", bocchiConf, StringComparison.Ordinal);

        // Bocchi should have nijika's VIPs in AllowedIPs:
        Assert.Contains("AllowedIPs = 10.99.0.2/32, 10.99.10.1/32, 10.99.10.2/32", bocchiConf, StringComparison.Ordinal);

        // Nijika config should have Endpoint for public bocchi node:
        var nijikaConf = configs["nijika"];
        Assert.Contains("Endpoint = 203.0.113.1:51820", nijikaConf, StringComparison.Ordinal);

        // Nijika config should NOT have Endpoint for non-public ryo node:
        Assert.DoesNotContain("Endpoint = null", nijikaConf, StringComparison.Ordinal);
    }

    [Fact]
    public void GenerateMeshConfigurations_DoesNotDuplicatePort_WhenEndpointAlreadyHasPort()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1:51820", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        var nijikaConf = configs["nijika"];

        Assert.Contains("Endpoint = 203.0.113.1:51820", nijikaConf, StringComparison.Ordinal);
        Assert.DoesNotContain(":51820:51820", nijikaConf, StringComparison.Ordinal);
    }
}
