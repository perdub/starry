using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class ClusterEndToEndTests
{
    [Fact]
    public void E2E_FullClusterLifecycle_FromConfigToPlanToBundle()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_e2e_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var nodesDir = Path.Combine(tempDir, "nodes");
        var servicesDir = Path.Combine(tempDir, "services");
        Directory.CreateDirectory(nodesDir);
        Directory.CreateDirectory(servicesDir);

        try
        {
            // 1. Multi-file modular configuration
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), "cluster_name: kessoku-e2e\nversion: '1.0'\n");
            File.WriteAllText(Path.Combine(nodesDir, "bocchi.yaml"), "host: 198.51.100.10\nport: 22\nvpn_ip: 10.99.0.1\nis_public: true\nssh_key: PLAIN[super-secret-key]\n");
            File.WriteAllText(Path.Combine(servicesDir, "api.toml"), "image = 'my-api:v1'\npriority = 50\nnode = 'bocchi'\nvip = '10.99.10.1'\n");

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);

            // 2. Validation
            var valResult = ConfigValidator.Validate(tree);
            Assert.True(valResult.IsValid);
            Assert.Contains(valResult.Issues, i => i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));

            // 3. Encrypt open secrets
            var masterKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
            var secretManager = new SecretManager();
            var encryptedCount = secretManager.EncryptOpenSecrets(tree, masterKey, tempDir, engine);
            Assert.Equal(1, encryptedCount);

            var encryptedKeyVal = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(encryptedKeyVal);
            Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedKeyVal, StringComparison.Ordinal);

            // 4. DAG Migration Plan
            var plan = MigrationPlanner.BuildPlan(tree);
            Assert.True(plan.HasChanges);
            Assert.Equal("kessoku-e2e", plan.ClusterName);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.CreateNode && string.Equals(a.Target, "bocchi", StringComparison.Ordinal));
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.ConfigureWireGuardMesh);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.DeployService && string.Equals(a.Target, "api", StringComparison.Ordinal));

            // 5. WireGuard & Nftables generation
            var wgNodes = new List<WireGuardNodeConfig>
            {
                new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "198.51.100.10", IsPublic: true)
            };
            var wgConfigs = WireGuardMeshService.GenerateMeshConfigurations(wgNodes);
            Assert.Single(wgConfigs);
            Assert.Contains("Address = 10.99.0.1/16", wgConfigs["bocchi"], StringComparison.Ordinal);

            var nftRules = NftablesService.GenerateRuleset([], [80, 443]);
            Assert.Contains("table inet starry", nftRules, StringComparison.Ordinal);

            // 6. Disaster Recovery Bundle Export & Import with IP Override
            var bundlePath = Path.Combine(tempDir, "fleet-bundle.tar.gz");
            FleetBundleService.ExportBundle(tree, bundlePath, engine);
            Assert.True(File.Exists(bundlePath));

            var restoreDir = Path.Combine(tempDir, "dr-restored");
            var ipOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["bocchi"] = "203.0.113.88"
            };
            var restoredTree = FleetBundleService.ImportBundle(bundlePath, restoreDir, ipOverrides, engine);

            Assert.Equal("kessoku-e2e", restoredTree.Root["cluster_name"]?.ToString());
            Assert.Equal("203.0.113.88", restoredTree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("my-api:v1", restoredTree.Root["services"]?["api"]?["image"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
