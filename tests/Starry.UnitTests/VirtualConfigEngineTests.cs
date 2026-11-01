using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Xunit;

namespace Starry.UnitTests;

public class VirtualConfigEngineTests
{
    private readonly VirtualConfigEngine _engine = new();

    [Fact]
    public void YamlFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new YamlConfigFormatProvider();
        const string yaml = """
            cluster_name: kessoku-cluster
            nodes:
              bocchi:
                host: 192.168.1.10
                port: 22
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(yaml), "test.yaml");
        Assert.NotNull(node);
        Assert.Equal("kessoku-cluster", node["cluster_name"]?.ToString());
        Assert.Equal("192.168.1.10", node["nodes"]?["bocchi"]?["host"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("kessoku-cluster", serialized, StringComparison.Ordinal);
        Assert.Contains("192.168.1.10", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void TomlFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new TomlConfigFormatProvider();
        const string toml = """
            title = "Starry Config"

            [nodes.nijika]
            host = "10.0.0.1"
            port = 2222
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(toml), "test.toml");
        Assert.NotNull(node);
        Assert.Equal("Starry Config", node["title"]?.ToString());
        Assert.Equal("10.0.0.1", node["nodes"]?["nijika"]?["host"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("Starry Config", serialized, StringComparison.Ordinal);
        Assert.Contains("10.0.0.1", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void JsonFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new JsonConfigFormatProvider();
        const string json = """
            {
              "version": "1.0",
              "services": {
                "api": {
                  "image": "nginx:alpine",
                  "priority": 10
                }
              }
            }
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(json), "test.json");
        Assert.NotNull(node);
        Assert.Equal("1.0", node["version"]?.ToString());
        Assert.Equal("nginx:alpine", node["services"]?["api"]?["image"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("nginx:alpine", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void VirtualConfigEngine_LoadsDirectoryAndBuildsHierarchyWithSourceMap()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        Directory.CreateDirectory(Path.Combine(tempDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(tempDir, "services"));

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), "cluster_name: starry-mesh\nversion: '1.0'\n");
            File.WriteAllText(Path.Combine(tempDir, "nodes", "bocchi.yaml"), "host: 192.168.1.100\nport: 22\nvpn_ip: 10.99.0.1\n");
            File.WriteAllText(Path.Combine(tempDir, "services", "web.toml"), "image = 'caddy:latest'\npriority = 50\nnode = 'bocchi'\n");

            var tree = _engine.Load(tempDir);

            Assert.Equal("starry-mesh", tree.Root["cluster_name"]?.ToString());
            Assert.Equal("192.168.1.100", tree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("caddy:latest", tree.Root["services"]?["web"]?["image"]?.ToString());

            // Check SourceMap
            Assert.True(tree.SourceMap.ContainsKey("nodes.bocchi.host"));
            Assert.Contains("bocchi.yaml", tree.SourceMap["nodes.bocchi.host"].FilePath, StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void VirtualConfigEngine_PacksAndSplitsCorrectly()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_pack_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("test-fleet"));
            tree.SetNode("nodes.ryo.host", JsonValue.Create("10.0.0.5"));
            tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));

            var packedFile = Path.Combine(tempDir, "bundle.yaml");
            _engine.Pack(tree, packedFile, "yaml");
            Assert.True(File.Exists(packedFile));

            var splitDir = Path.Combine(tempDir, "split_out");
            _engine.Split(tree, splitDir, "yaml");

            Assert.True(File.Exists(Path.Combine(splitDir, "starry.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "nodes", "ryo.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "services", "db.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
