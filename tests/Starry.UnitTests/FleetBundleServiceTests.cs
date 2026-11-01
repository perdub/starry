using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Backups;
using Xunit;

namespace Starry.UnitTests;

public class FleetBundleServiceTests
{
    [Fact]
    public void ExportAndImportBundle_PreservesConfigAndAppliesIpOverrides()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_bundle_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("kessoku-dr"));
            tree.SetNode("nodes.bocchi.host", JsonValue.Create("198.51.100.10"));
            tree.SetNode("nodes.bocchi.port", JsonValue.Create(22));
            tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));

            var bundleFile = Path.Combine(tempDir, "fleet.tar.gz");
            FleetBundleService.ExportBundle(tree, bundleFile);

            Assert.True(File.Exists(bundleFile));

            var importDir = Path.Combine(tempDir, "restored");
            var ipOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["bocchi"] = "203.0.113.99"
            };

            var importedTree = FleetBundleService.ImportBundle(bundleFile, importDir, ipOverrides);

            Assert.Equal("kessoku-dr", importedTree.Root["cluster_name"]?.ToString());
            Assert.Equal("203.0.113.99", importedTree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("postgres:16", importedTree.Root["services"]?["db"]?["image"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
