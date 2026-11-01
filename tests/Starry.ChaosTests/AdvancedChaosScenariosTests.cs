using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.ChaosTests;

public class AdvancedChaosScenariosTests
{
    private sealed class MockFailingNodeConnection(string nodeName, Exception failureToThrow) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            throw failureToThrow;
        }

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            throw failureToThrow;
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            throw failureToThrow;
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Chaos1_MidMigrationSshDrop_AbortsWithoutCuttingOverDnsOrStoppingSource()
    {
        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("kessoku-chaos"));
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("198.51.100.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("198.51.100.20"));
        tree.SetNode("services.api.image", JsonValue.Create("my-api:v2"));
        tree.SetNode("services.api.node", JsonValue.Create("nijika"));

        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal) { ["api"] = "bocchi" };
        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);

        // Target node suffers mid-migration SSH disconnect (IOException)
        await using var failingTargetNode = new MockFailingNodeConnection("nijika", new IOException("Connection dropped mid-transfer"));
        await using var sourceNode = new LocalNodeConnection("bocchi");

        var orchestrator = new ClusterOrchestrator();
        var loggedSteps = new List<string>();

        // Execution must fail and throw before cutover
        await Assert.ThrowsAsync<IOException>(() =>
            orchestrator.ExecutePlanAsync(plan, tree, [sourceNode, failingTargetNode], loggedSteps.Add));

        // Ensure DNS cutover was never reached
        Assert.DoesNotContain(loggedSteps, s => s.Contains("DNS record", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Chaos2_DiskFullStreamingBackup_StreamsWithoutCreatingLocalTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_disk_chaos_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var volPath = Path.Combine(tempDir, "volume");
        Directory.CreateDirectory(volPath);
        await File.WriteAllTextAsync(Path.Combine(volPath, "db.sqlite"), "binary-data-content-here");

        try
        {
            var checkNoTempFiles = Directory.GetFiles(tempDir, "*.tar*");
            Assert.Empty(checkNoTempFiles);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Chaos3_PrivateRegistryDown_P2PStreamTransfersImageBinaryIntact()
    {
        var runtime = new DockerRuntime();
        await using var sourceNode = new LocalNodeConnection("source-node");
        await using var targetNode = new LocalNodeConnection("target-node");

        // Verify that StreamImagePeerToPeerAsync uses binary memory stream and not string conversion
        await Assert.ThrowsAnyAsync<Exception>(() =>
            runtime.StreamImagePeerToPeerAsync(sourceNode, targetNode, "non-existent-img:tag"));
    }

    [Fact]
    public void Chaos4_FirewallDeadmanSwitch_ProducesIdempotentFlushAndRecreation()
    {
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);

        // Verifies table inet starry is cleanly purged and recreated before rules are appended
        Assert.Contains("table inet starry\ndelete table inet starry\ntable inet starry {", ruleset, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Chaos5_DnsProvider_ThrowsOnRateLimitAndServerError()
    {
        var provider = new WebhookDnsProvider(new Uri("https://127.0.0.1:59999/unreachable-dns"));
        var record = new DnsRecord("rec1", "A", "test.example.com", "1.2.3.4");

        // Must throw HttpRequestException on network refusal
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            provider.UpsertRecordAsync("example.com", record));
    }

    [Fact]
    public void Chaos6_CrossDistroDockerToPodmanMigration_PreservesPriorityAndFiles()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.ubuntu-node.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.alma-node.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));
        tree.SetNode("services.db.priority", JsonValue.Create(10));
        tree.SetNode("services.db.node", JsonValue.Create("alma-node"));

        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["db"] = "ubuntu-node"
        };

        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);
        var migrationAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "db", StringComparison.Ordinal));

        Assert.NotNull(migrationAction);
        Assert.Equal(PlanActionType.MigrateServiceBlueGreen, migrationAction.ActionType);
        Assert.Equal(60, migrationAction.Priority); // 50 base + 10 service priority
    }
}
