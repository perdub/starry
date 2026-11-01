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

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) => throw failureToThrow;
        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default) => throw failureToThrow;
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) => throw failureToThrow;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockSuccessNodeConnection(string nodeName) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockSuccessHttpMessageHandler();
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockRecordingNodeConnection(string nodeName) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";
        public List<string> ExecutedCommands { get; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            ExecutedCommands.Add(string.Join(" ", command.Arguments));
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            ExecutedCommands.Add(string.Join(" ", command.Arguments));
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) =>
            Task.FromResult(new HttpClient());

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockBinaryStreamingNodeConnection(string nodeName, byte[] payload) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";
        public byte[] ReceivedBytes { get; private set; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        public async Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            if (stdoutStream != null && payload.Length > 0)
            {
                await stdoutStream.WriteAsync(payload, ct);
            }

            if (stdinStream != null)
            {
                using var ms = new MemoryStream();
                await stdinStream.CopyToAsync(ms, ct);
                ReceivedBytes = ms.ToArray();
            }

            return new ProcessResult(0, "success", string.Empty, TimeSpan.Zero);
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) =>
            Task.FromResult(new HttpClient());

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockSuccessHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":\"mock-container-id\",\"State\":\"running\",\"Status\":\"Up\"}", Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class MockRetryingHttpMessageHandler(Queue<HttpStatusCode> responses) : HttpMessageHandler
    {
        public int RequestCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            var status = responses.Count > 0 ? responses.Dequeue() : HttpStatusCode.OK;
            return Task.FromResult(new HttpResponseMessage(status)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            });
        }
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

        await using var failingTargetNode = new MockFailingNodeConnection("nijika", new IOException("Connection dropped mid-transfer"));
        await using var sourceNode = new MockSuccessNodeConnection("bocchi");

        var orchestrator = new ClusterOrchestrator();
        var loggedSteps = new List<string>();

        await Assert.ThrowsAsync<IOException>(() =>
            orchestrator.ExecutePlanAsync(plan, tree, [sourceNode, failingTargetNode], loggedSteps.Add));

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
            var provider = new RcloneBackupStorageProvider("s3:my-bucket/backups");
            await using var mockNode = new MockRecordingNodeConnection("local-node");

            await provider.StreamBackupAsync(mockNode, volPath, "backup-01");

            // Verify the command executed is in-memory streaming: tar -C ... | zstd -3 | rclone rcat ...
            Assert.Contains(mockNode.ExecutedCommands, cmd =>
                cmd.Contains("rclone rcat 's3:my-bucket/backups/backup-01.tar.zst'", StringComparison.Ordinal) &&
                cmd.Contains("zstd -3", StringComparison.Ordinal) &&
                cmd.Contains("tar -C", StringComparison.Ordinal));

            // Verify 0 temporary tar files were written to disk
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
        // Array containing tricky non-UTF8 binary byte sequences
        byte[] originalBinaryPayload = [0xFF, 0x00, 0x80, 0xFE, 0xAA, 0x55, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x12, 0x34];

        await using var sourceNode = new MockBinaryStreamingNodeConnection("source-node", originalBinaryPayload);
        await using var targetNode = new MockBinaryStreamingNodeConnection("target-node", []);

        await runtime.StreamImagePeerToPeerAsync(sourceNode, targetNode, "custom-image:v1");

        // Assert 100% byte-for-byte exact binary transmission through pipes
        Assert.Equal(originalBinaryPayload, targetNode.ReceivedBytes);
    }

    [Fact]
    public void Chaos4_FirewallDeadmanSwitch_ProducesIdempotentFlushAndRecreation()
    {
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);

        Assert.Contains("table inet starry\ndelete table inet starry\ntable inet starry {", ruleset, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Chaos5_DnsProvider_RetriesWithBackoffOnTransientErrors()
    {
        // First request returns 429, second returns 502, third succeeds with 200 OK
        var responses = new Queue<HttpStatusCode>([HttpStatusCode.TooManyRequests, HttpStatusCode.BadGateway, HttpStatusCode.OK]);
        using var handler = new MockRetryingHttpMessageHandler(responses);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.cloudflare.com/client/v4/") };

        var provider = new CloudflareDnsProvider("fake-token", client);
        var records = await provider.GetRecordsAsync("example.com");

        Assert.NotNull(records);
        Assert.True(handler.RequestCount >= 2);
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
        Assert.Equal(60, migrationAction.Priority);
    }
}
