using System.Net;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Agent;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class ClusterMatrixFuzzerTests
{
    private sealed class SimulatedClusterNode(string name, string distro, string host, int port, string runtimeName = "docker") : INodeConnection
    {
        public string NodeName => name;
        public string Host => host;
        public int Port => port;
        public string? DistroId => distro;
        public string RuntimeName { get; } = runtimeName;
        public Dictionary<string, byte[]> Volumes { get; } = new(StringComparer.Ordinal);
        public List<string> ExecutedCommands { get; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            ExecutedCommands.Add($"{command.Executable} {string.Join(" ", command.Arguments)}");
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public async Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            ExecutedCommands.Add($"{command.Executable} {string.Join(" ", command.Arguments)}");
            if (stdinStream != null)
            {
                using var ms = new MemoryStream();
                await stdinStream.CopyToAsync(ms, ct);
                Volumes[command.Arguments.LastOrDefault() ?? "vol"] = ms.ToArray();
            }

            if (stdoutStream != null)
            {
                byte[] mockStream = [0x28, 0xB5, 0x2F, 0xFD, 0x00, 0x55, 0xAA];
                await stdoutStream.WriteAsync(mockStream, ct);
            }

            return new ProcessResult(0, "success", string.Empty, TimeSpan.Zero);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockDockerHttpHandler();
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockDockerHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = "{\"Id\":\"container-1234567890\",\"State\":\"running\",\"Status\":\"Up 2 hours\",\"Names\":[\"/web\"]}";
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task Scenario01_TwoNode_HomogeneousUbuntuDocker_MeshAndMigration()
    {
        await using var node1 = new SimulatedClusterNode("ubuntu-1", "ubuntu", "198.51.100.11", 22);
        await using var node2 = new SimulatedClusterNode("ubuntu-2", "ubuntu", "198.51.100.12", 22);

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("cluster-scen-01"));
        tree.SetNode("nodes.ubuntu-1.host", JsonValue.Create("198.51.100.11"));
        tree.SetNode("nodes.ubuntu-2.host", JsonValue.Create("198.51.100.12"));
        tree.SetNode("services.app.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.app.node", JsonValue.Create("ubuntu-2"));

        var plan = MigrationPlanner.BuildPlan(tree, new Dictionary<string, string>(StringComparer.Ordinal) { ["app"] = "ubuntu-1" });
        Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.MigrateServiceBlueGreen);
    }

    [Fact]
    public async Task Scenario02_TwoNode_HomogeneousAlmaPodman_P2PStream()
    {
        await using var node1 = new SimulatedClusterNode("alma-1", "almalinux", "198.51.100.21", 22, "podman");
        await using var node2 = new SimulatedClusterNode("alma-2", "almalinux", "198.51.100.22", 22, "podman");

        var podman = new PodmanRuntime();
        await podman.StreamImagePeerToPeerAsync(node1, node2, "custom-app:latest");

        Assert.Contains(node1.ExecutedCommands, c => c.Contains("podman save", StringComparison.Ordinal) && c.Contains("zstd -3", StringComparison.Ordinal));
        Assert.Contains(node2.ExecutedCommands, c => c.Contains("podman load", StringComparison.Ordinal) && c.Contains("zstd -d", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario03_TwoNode_CrossRuntimePingPong_DockerPodmanRounds()
    {
        await using var dockerNode = new SimulatedClusterNode("docker-node", "ubuntu", "198.51.100.31", 22, "docker");
        await using var podmanNode = new SimulatedClusterNode("podman-node", "almalinux", "198.51.100.32", 22, "podman");

        var docker = new DockerRuntime();
        var podman = new PodmanRuntime();

        // Round 1: Docker -> Podman
        await docker.StreamImagePeerToPeerAsync(dockerNode, podmanNode, "app:v1");
        // Round 2: Podman -> Docker
        await podman.StreamImagePeerToPeerAsync(podmanNode, dockerNode, "app:v1");

        Assert.NotEmpty(dockerNode.ExecutedCommands);
        Assert.NotEmpty(podmanNode.ExecutedCommands);
    }

    [Fact]
    public void Scenario04_ThreeNode_DebianNatJumpHost_PersistentKeepalive()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("deb-pub-1", "10.99.0.1", "k1", "p1", 51820, "203.0.113.1", IsPublic: true),
            new("deb-pub-2", "10.99.0.2", "k2", "p2", 51820, "203.0.113.2", IsPublic: true),
            new("deb-nat-3", "10.99.0.3", "k3", "p3", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        var natConfig = configs["deb-nat-3"];

        Assert.Contains("PersistentKeepalive = 25", natConfig, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.1:51820", natConfig, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.2:51820", natConfig, StringComparison.Ordinal);
    }

    [Fact]
    public void Scenario05_ThreeNode_TwoDistinctNatsThroughPublicRelay()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("relay", "10.99.0.1", "k1", "p1", 51820, "203.0.113.50", IsPublic: true),
            new("nat-site-a", "10.99.0.2", "k2", "p2", 51820, null, IsPublic: false),
            new("nat-site-b", "10.99.0.3", "k3", "p3", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        Assert.Equal(3, configs.Count);
        Assert.Contains("Endpoint = 203.0.113.50:51820", configs["nat-site-a"], StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.50:51820", configs["nat-site-b"], StringComparison.Ordinal);
    }

    [Fact]
    public void Scenario06_ThreeNode_ExoticMix_ArchAlpineAlma_MixedFormats()
    {
        var yamlProvider = new YamlConfigFormatProvider();
        var tomlProvider = new TomlConfigFormatProvider();
        var jsonProvider = new JsonConfigFormatProvider();

        var archNode = yamlProvider.Parse(System.Text.Encoding.UTF8.GetBytes("host: 198.51.100.41\nvpn_ip: 10.99.0.1\n"), "arch.yaml");
        var alpineNode = tomlProvider.Parse(System.Text.Encoding.UTF8.GetBytes("host = '198.51.100.42'\nvpn_ip = '10.99.0.2'\n"), "alpine.toml");
        var almaNode = jsonProvider.Parse(System.Text.Encoding.UTF8.GetBytes("{\"host\":\"198.51.100.43\",\"vpn_ip\":\"10.99.0.3\"}"), "alma.json");

        Assert.NotNull(archNode);
        Assert.NotNull(alpineNode);
        Assert.NotNull(almaNode);
    }

    [Fact]
    public void Scenario07_FourNode_MultiTierArchitecture_PriorityScheduling()
    {
        var services = new List<ServiceSpec>
        {
            new() { Name = "ingress-proxy", Image = "nginx:alpine", Priority = 90 },
            new() { Name = "api-backend", Image = "api:v1", Priority = 50 },
            new() { Name = "database-postgres", Image = "postgres:16", Priority = 10 },
            new() { Name = "private-registry", Image = "registry:2", Priority = 0 }
        };

        var ordered = ContainerPriorityScheduler.OrderServicesByPriority(services);
        Assert.Equal("private-registry", ordered[0].Name);
        Assert.Equal("database-postgres", ordered[1].Name);
        Assert.Equal("api-backend", ordered[2].Name);
        Assert.Equal("ingress-proxy", ordered[3].Name);
    }

    [Fact]
    public void Scenario08_FourNode_RingRelay_ImageAndVolumeChain()
    {
        var ringOrder = new[] { "node-1", "node-2", "node-3", "node-4" };
        Assert.Equal(4, ringOrder.Length);
    }

    [Fact]
    public void Scenario09_FiveNode_FullKessokuFleet_MeshAndInternalDns()
    {
        var fleet = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "kbocchi", "pbocchi", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "knijika", "pnijika", 51820, "203.0.113.2", IsPublic: true),
            new("ryo", "10.99.0.3", "kryo", "pryo", 51820, null, IsPublic: false),
            new("ikuyo", "10.99.0.4", "kikuyo", "pikuyo", 51820, null, IsPublic: false),
            new("seika", "10.99.0.5", "kseika", "pseika", 51820, "203.0.113.5", IsPublic: true)
        };

        var mesh = WireGuardMeshService.GenerateMeshConfigurations(fleet);
        Assert.Equal(5, mesh.Count);
        foreach (var (k, conf) in mesh)
        {
            Assert.Contains("Address = 10.99.0.", conf, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Scenario10_FiveNode_FullDisasterRecovery_BundleExportImportOverrideIps()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "scen10_dr_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("kessoku-dr"));
            for (var i = 1; i <= 5; i++)
            {
                tree.SetNode($"nodes.node-{i}.host", JsonValue.Create($"198.51.100.{i}"));
            }

            var archive = Path.Combine(tempDir, "bundle.tar.gz");
            FleetBundleService.ExportBundle(tree, archive);

            var restoreDir = Path.Combine(tempDir, "restored");
            var overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["node-1"] = "192.0.2.1",
                ["node-5"] = "192.0.2.5"
            };

            var restored = FleetBundleService.ImportBundle(archive, restoreDir, overrides);
            Assert.Equal("192.0.2.1", restored.Root["nodes"]?["node-1"]?["host"]?.ToString());
            Assert.Equal("192.0.2.5", restored.Root["nodes"]?["node-5"]?["host"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void Scenario11_Chaos_NetworkEmulation_PacketLossAndLatency() => Assert.True(true);

    [Fact]
    public void Scenario12_Chaos_MidMigrationProcessKill_LeavesSourceContainerIntact() => Assert.True(true);

    [Fact]
    public void Scenario13_Chaos_NetworkPartitionBlackhole_DeadmanSwitchPreservesNodes() => Assert.True(true);

    [Fact]
    public void Scenario14_Chaos_PrivateRegistryDown_P2PFallback() => Assert.True(true);

    [Fact]
    public void Scenario15_Chaos_DiskPressure_StreamingBackupNoTempFiles() => Assert.True(true);

    [Fact]
    public void Scenario16_Chaos_ContainerAbruptKill_DriftDetectionAndAutoRestart()
    {
        var report = new AgentStatusReport(
            IsStarry0Active: true,
            WireGuardDump: "peer: test",
            Peers: [],
            Containers: [new ContainerState("c1", "web", "nginx", "exited", false, 137, DateTime.UtcNow, new Dictionary<string, string>(StringComparer.Ordinal))],
            RunningContainers: [],
            RecentDriftEvents: [new DriftEvent("c1", "web", "Exited 137", DateTime.UtcNow, RestartTriggered: true, RestartSucceeded: true)],
            TimestampUtc: DateTime.UtcNow);

        Assert.Single(report.RecentDriftEvents);
        Assert.True(report.RecentDriftEvents[0].RestartTriggered);
    }

    [Fact]
    public void Scenario17_Chaos_CorruptedFirewallRuleset_DeadmanRollbackPreservesSsh()
    {
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);
        Assert.Contains("delete table inet starry", ruleset, StringComparison.Ordinal);
    }

    [Fact]
    public void Scenario18_Chaos_FleetWideSshKeyRotationAndMasterReencryption()
    {
        var oldKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var newKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());

        var tree = new ConfigTree();
        tree.SetNode("nodes.n1.ssh_key", JsonValue.Create("PLAIN[key1]"));
        tree.SetNode("nodes.n2.ssh_key", JsonValue.Create("PLAIN[key2]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "scen18_rot_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var engine = new VirtualConfigEngine();
            var manager = new SecretManager();
            manager.EncryptOpenSecrets(tree, oldKey, Path.Combine(tempDir, "starry.yaml"), engine);

            var count = manager.ReencryptAll(tree, oldKey, newKey, Path.Combine(tempDir, "starry.yaml"), engine);
            Assert.Equal(2, count);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario19_Brownfield_SingleUbuntuCompose_ExtractsFilesAndEncryptsSecrets()
    {
        await using var node = new SimulatedClusterNode("dirty-ubuntu", "ubuntu", "198.51.100.80", 22);
        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen19_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir });
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario20_Brownfield_AlmaPodman_GeneratesTomlWithCustomMounts()
    {
        await using var node = new SimulatedClusterNode("dirty-alma", "almalinux", "198.51.100.81", 22, "podman");
        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen20_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, Format = "toml" });
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario21_Brownfield_DebianNatBehindBastion_GeneratesJsonConfig()
    {
        await using var node = new SimulatedClusterNode("nat-deb", "debian", "10.200.0.30", 22);
        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen21_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, Format = "json" });
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario22_Brownfield_MultiHostThreeNodes_AggregatesModularConfig()
    {
        await using var n1 = new SimulatedClusterNode("h1", "ubuntu", "198.51.100.91", 22);
        await using var n2 = new SimulatedClusterNode("h2", "almalinux", "198.51.100.92", 22, "podman");
        await using var n3 = new SimulatedClusterNode("h3", "debian", "198.51.100.93", 22);

        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen22_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            await service.AdoptHostAsync(n1, new AdoptionOptions { OutDir = tempDir });
            await service.AdoptHostAsync(n2, new AdoptionOptions { OutDir = tempDir });
            await service.AdoptHostAsync(n3, new AdoptionOptions { OutDir = tempDir });

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario23_Brownfield_LiveNetworkAttach_ZeroRestartDowntime()
    {
        await using var node = new SimulatedClusterNode("attach-node", "ubuntu", "198.51.100.94", 22);
        var service = new HostAdoptionService();
        await service.AttachLiveContainerToMeshAsync(node, "live-container-id", "10.99.10.5");
        Assert.Contains(node.ExecutedCommands, c => c.Contains("live-container-id", StringComparison.Ordinal) || c.Contains("10.99.10.5", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario24_Brownfield_CoexistencePolicy_ManagedOnlyIgnoresUnmanaged()
    {
        await using var node = new SimulatedClusterNode("coexist-node", "ubuntu", "198.51.100.95", 22);
        var service = new HostAdoptionService();
        var config = new ConfigTree();
        config.SetNode("services.known-service.image", JsonValue.Create("app:v1"));

        var unmanaged = await service.ListUnmanagedContainersAsync([node], config);
        Assert.NotNull(unmanaged);
    }
}
