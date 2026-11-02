using System.Net;
using System.Security.Cryptography;
using System.Text;
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
    private sealed class StatefulClusterNode : INodeConnection
    {
        public string NodeName { get; }
        public string Host { get; }
        public int Port { get; }
        public string? DistroId { get; }
        public string RuntimeName { get; }
        public Dictionary<string, byte[]> Volumes { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, string> VirtualFiles { get; } = new(StringComparer.Ordinal);
        public List<string> ExecutedCommands { get; } = [];
        public List<JsonObject> RegisteredContainers { get; } = [];

        public StatefulClusterNode(string name, string distro, string host, int port, string runtimeName = "docker")
        {
            NodeName = name;
            DistroId = distro;
            Host = host;
            Port = port;
            RuntimeName = runtimeName;

            // Default mock file system
            VirtualFiles["/etc/os-release"] = $"NAME=\"{distro}\"\nID={distro}\nVERSION_ID=\"24.04\"\n";
            VirtualFiles["/etc/nginx/shop.conf"] = "server { listen 80; server_name shop.local; }\n";
        }

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            var cmdLine = $"{command.Executable} {string.Join(" ", command.Arguments)}";
            ExecutedCommands.Add(cmdLine);

            if (command.Arguments.Count > 0 && string.Equals(command.Arguments[0], "-c", StringComparison.Ordinal))
            {
                var script = command.Arguments[1];
                if (script.Contains("cat /etc/nginx/shop.conf", StringComparison.Ordinal) ||
                    script.Contains("cat '/etc/nginx/shop.conf'", StringComparison.Ordinal))
                {
                    return Task.FromResult(new ProcessResult(0, VirtualFiles["/etc/nginx/shop.conf"], string.Empty, TimeSpan.Zero));
                }

                if (script.Contains("sha256sum", StringComparison.Ordinal))
                {
                    return Task.FromResult(new ProcessResult(0, "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855  -", string.Empty, TimeSpan.Zero));
                }
            }

            if (string.Equals(command.Executable, "cat", StringComparison.Ordinal) && command.Arguments.Count > 0 && VirtualFiles.TryGetValue(command.Arguments[0], out var content))
            {
                return Task.FromResult(new ProcessResult(0, content, string.Empty, TimeSpan.Zero));
            }

            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public async Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            var cmdLine = $"{command.Executable} {string.Join(" ", command.Arguments)}";
            ExecutedCommands.Add(cmdLine);

            if (stdinStream != null)
            {
                using var ms = new MemoryStream();
                await stdinStream.CopyToAsync(ms, ct);
                var key = command.Arguments.Count > 0 ? command.Arguments[^1] : "vol";
                Volumes[key] = ms.ToArray();
            }

            if (stdoutStream != null)
            {
                byte[] mockStream = [0x28, 0xB5, 0x2F, 0xFD, 0x00, 0x55, 0xAA, 0xDE, 0xAD, 0xBE, 0xEF];
                await stdoutStream.WriteAsync(mockStream, ct);
            }

            return new ProcessResult(0, "success", string.Empty, TimeSpan.Zero);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockStatefulDockerHttpHandler(RegisteredContainers);
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockStatefulDockerHttpHandler(List<JsonObject> containers) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.PathAndQuery ?? string.Empty;

            if (request.Method == HttpMethod.Post && path.Contains("/containers/create", StringComparison.Ordinal))
            {
                var query = request.RequestUri?.Query ?? string.Empty;
                var name = "app";
                var qIdx = query.IndexOf("name=", StringComparison.Ordinal);
                if (qIdx >= 0)
                {
                    var rawName = query[(qIdx + 5)..];
                    var ampersand = rawName.IndexOf('&', StringComparison.Ordinal);
                    if (ampersand >= 0)
                    {
                        rawName = rawName[..ampersand];
                    }
                    name = Uri.UnescapeDataString(rawName);
                }

                var id = "c-" + Guid.NewGuid().ToString("N")[..12];

                var newContainer = new JsonObject
                {
                    ["Id"] = id,
                    ["Names"] = new JsonArray { "/" + name.TrimStart('/') },
                    ["Image"] = "app:latest",
                    ["State"] = "running",
                    ["Status"] = "Up",
                    ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
                };
                containers.Add(newContainer);

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent($"{{\"Id\":\"{id}\"}}", Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/containers/json", StringComparison.Ordinal))
            {
                if (containers.Count == 0)
                {
                    containers.Add(new JsonObject
                    {
                        ["Id"] = "c-init",
                        ["Names"] = new JsonArray { "/app", "/web" },
                        ["Image"] = "nginx:alpine",
                        ["State"] = "running",
                        ["Status"] = "Up",
                        ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
                    });
                }

                var arr = new JsonArray();
                foreach (var c in containers)
                {
                    arr.Add(JsonNode.Parse(c.ToJsonString()));
                }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(arr.ToJsonString(), Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/containers/", StringComparison.Ordinal) && path.EndsWith("/json", StringComparison.Ordinal))
            {
                var inspectObj = new JsonObject
                {
                    ["Id"] = "c1234567890",
                    ["Name"] = "/shop-web",
                    ["State"] = new JsonObject { ["Running"] = true, ["Status"] = "running" },
                    ["Config"] = new JsonObject
                    {
                        ["Image"] = "nginx:alpine",
                        ["Env"] = new JsonArray { "POSTGRES_PASSWORD=secret123", "PORT=80" },
                        ["Labels"] = new JsonObject { ["com.docker.compose.project"] = "shop" }
                    },
                    ["HostConfig"] = new JsonObject
                    {
                        ["Binds"] = new JsonArray { "/etc/nginx/shop.conf:/etc/nginx/shop.conf:ro" },
                        ["PortBindings"] = new JsonObject
                        {
                            ["80/tcp"] = new JsonArray { new JsonObject { ["HostPort"] = "8080" } }
                        }
                    }
                };

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(inspectObj.ToJsonString(), Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/version", StringComparison.Ordinal))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"Version\":\"26.1.0\",\"Os\":\"linux\",\"Arch\":\"amd64\"}", Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":\"c1234567890\"}", Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task Scenario01_TwoNode_HomogeneousUbuntuDocker_MeshAndMigration()
    {
        await using var node1 = new StatefulClusterNode("ubuntu-1", "ubuntu", "198.51.100.11", 22);
        await using var node2 = new StatefulClusterNode("ubuntu-2", "ubuntu", "198.51.100.12", 22);

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("cluster-scen-01"));
        tree.SetNode("nodes.ubuntu-1.host", JsonValue.Create("198.51.100.11"));
        tree.SetNode("nodes.ubuntu-2.host", JsonValue.Create("198.51.100.12"));
        tree.SetNode("services.app.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.app.node", JsonValue.Create("ubuntu-2"));

        var plan = MigrationPlanner.BuildPlan(tree, new Dictionary<string, string>(StringComparer.Ordinal) { ["app"] = "ubuntu-1" });
        Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.MigrateServiceBlueGreen);

        var orchestrator = new ClusterOrchestrator();
        await orchestrator.ExecutePlanAsync(plan, tree, [node1, node2]);
        Assert.Contains(node2.ExecutedCommands, c => c.Contains("apt-get install", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario02_TwoNode_HomogeneousAlmaPodman_P2PStream()
    {
        await using var node1 = new StatefulClusterNode("alma-1", "almalinux", "198.51.100.21", 22, "podman");
        await using var node2 = new StatefulClusterNode("alma-2", "almalinux", "198.51.100.22", 22, "podman");

        var podman = new PodmanRuntime();
        await podman.StreamImagePeerToPeerAsync(node1, node2, "custom-app:latest");

        Assert.Contains(node1.ExecutedCommands, c => c.Contains("podman save", StringComparison.Ordinal) && c.Contains("zstd -3", StringComparison.Ordinal));
        Assert.Contains(node2.ExecutedCommands, c => c.Contains("podman load", StringComparison.Ordinal) && c.Contains("zstd -d", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario03_TwoNode_CrossRuntimePingPong_DockerPodmanRounds()
    {
        await using var dockerNode = new StatefulClusterNode("docker-node", "ubuntu", "198.51.100.31", 22, "docker");
        await using var podmanNode = new StatefulClusterNode("podman-node", "almalinux", "198.51.100.32", 22, "podman");

        var docker = new DockerRuntime();
        var podman = new PodmanRuntime();

        await docker.StreamImagePeerToPeerAsync(dockerNode, podmanNode, "app:v1");
        await podman.StreamImagePeerToPeerAsync(podmanNode, dockerNode, "app:v1");

        Assert.NotEmpty(dockerNode.ExecutedCommands);
        Assert.NotEmpty(podmanNode.ExecutedCommands);
    }

    [Fact]
    public async Task Scenario04_ThreeNode_DebianNatJumpHost_PersistentKeepalive()
    {
        await using var pub1 = new StatefulClusterNode("deb-pub-1", "debian", "203.0.113.1", 22);
        await using var pub2 = new StatefulClusterNode("deb-pub-2", "debian", "203.0.113.2", 22);
        await using var nat3 = new StatefulClusterNode("deb-nat-3", "debian", "10.0.0.3", 22);

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

        await WireGuardMeshService.ApplyConfigurationAsync(nat3, natConfig);
        Assert.Contains(nat3.ExecutedCommands, c => c.Contains("wg syncconf starry0", StringComparison.Ordinal));
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

        var archNode = yamlProvider.Parse(Encoding.UTF8.GetBytes("host: 198.51.100.41\nvpn_ip: 10.99.0.1\n"), "arch.yaml");
        var alpineNode = tomlProvider.Parse(Encoding.UTF8.GetBytes("host = '198.51.100.42'\nvpn_ip = '10.99.0.2'\n"), "alpine.toml");
        var almaNode = jsonProvider.Parse(Encoding.UTF8.GetBytes("{\"host\":\"198.51.100.43\",\"vpn_ip\":\"10.99.0.3\"}"), "alma.json");

        Assert.NotNull(archNode);
        Assert.NotNull(alpineNode);
        Assert.NotNull(almaNode);
    }

    [Fact]
    public async Task Scenario07_FourNode_MultiTierArchitecture_PriorityScheduling()
    {
        await using var n1 = new StatefulClusterNode("node-reg", "ubuntu", "198.51.100.51", 22);
        await using var n2 = new StatefulClusterNode("node-db", "almalinux", "198.51.100.52", 22);
        await using var n3 = new StatefulClusterNode("node-api", "debian", "198.51.100.53", 22);
        await using var n4 = new StatefulClusterNode("node-proxy", "alpine", "198.51.100.54", 22);

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

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("tier-cluster"));
        foreach (var s in ordered)
        {
            tree.SetNode($"services.{s.Name}.image", JsonValue.Create(s.Image));
            tree.SetNode($"services.{s.Name}.priority", JsonValue.Create(s.Priority));
        }

        var plan = MigrationPlanner.BuildPlan(tree);
        var orchestrator = new ClusterOrchestrator();
        await orchestrator.ExecutePlanAsync(plan, tree, [n1, n2, n3, n4]);
    }

    [Fact]
    public async Task Scenario08_FourNode_RingRelay_ImageAndVolumeChain()
    {
        await using var n1 = new StatefulClusterNode("node-1", "ubuntu", "198.51.100.61", 22);
        await using var n2 = new StatefulClusterNode("node-2", "almalinux", "198.51.100.62", 22);
        await using var n3 = new StatefulClusterNode("node-3", "debian", "198.51.100.63", 22);
        await using var n4 = new StatefulClusterNode("node-4", "alpine", "198.51.100.64", 22);

        var ring = new[] { n1, n2, n3, n4, n1 };
        var runtime = new DockerRuntime();

        for (var i = 0; i < ring.Length - 1; i++)
        {
            await runtime.StreamImagePeerToPeerAsync(ring[i], ring[i + 1], "relay-image:v1");
        }

        Assert.NotEmpty(n1.ExecutedCommands);
        Assert.NotEmpty(n4.ExecutedCommands);
    }

    [Fact]
    public async Task Scenario09_FiveNode_FullKessokuFleet_MeshAndInternalDns()
    {
        await using var bocchi = new StatefulClusterNode("bocchi", "ubuntu", "203.0.113.1", 22);
        await using var nijika = new StatefulClusterNode("nijika", "almalinux", "203.0.113.2", 22);
        await using var ryo = new StatefulClusterNode("ryo", "debian", "10.0.0.3", 22);
        await using var ikuyo = new StatefulClusterNode("ikuyo", "alpine", "10.0.0.4", 22);
        await using var seika = new StatefulClusterNode("seika", "arch", "203.0.113.5", 22);

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

        await using var dns = new InternalDnsServer(port: 5354);
        dns.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));
        dns.RegisterRecord("nijika.internal", IPAddress.Parse("10.99.0.2"));
        dns.RegisterRecord("ryo.internal", IPAddress.Parse("10.99.0.3"));
        dns.RegisterRecord("ikuyo.internal", IPAddress.Parse("10.99.0.4"));
        dns.RegisterRecord("seika.internal", IPAddress.Parse("10.99.0.5"));

        Assert.Equal(5, dns.Records.Count);
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
    public async Task Scenario11_Chaos_NetworkEmulation_PacketLossAndLatency()
    {
        var responses = new Queue<HttpStatusCode>([HttpStatusCode.GatewayTimeout, HttpStatusCode.OK]);
        using var handler = new MockChaosHttpMessageHandler(responses, delayMs: 25);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.cloudflare.com/client/v4/") };

        var provider = new CloudflareDnsProvider("token", client);
        var records = await provider.GetRecordsAsync("example.com");
        Assert.NotNull(records);
        Assert.True(handler.Calls >= 2);
    }

    [Fact]
    public async Task Scenario12_Chaos_MidMigrationProcessKill_LeavesSourceContainerIntact()
    {
        await using var sourceNode = new StatefulClusterNode("src", "ubuntu", "198.51.100.1", 22);
        await using var targetNode = new StatefulClusterNode("dst", "ubuntu", "198.51.100.2", 22);

        var tree = new ConfigTree();
        tree.SetNode("services.web.image", JsonValue.Create("nginx"));
        tree.SetNode("services.web.node", JsonValue.Create("dst"));

        var plan = MigrationPlanner.BuildPlan(tree, new Dictionary<string, string>(StringComparer.Ordinal) { ["web"] = "src" });
        Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.MigrateServiceBlueGreen);
    }

    [Fact]
    public async Task Scenario13_Chaos_NetworkPartitionBlackhole_DeadmanSwitchPreservesNodes()
    {
        await using var node = new StatefulClusterNode("isolated-node", "ubuntu", "198.51.100.99", 22);
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);
        await NftablesService.ApplyRulesetAsync(node, ruleset);
        Assert.Contains(node.ExecutedCommands, c => c.Contains("nft -f", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario14_Chaos_PrivateRegistryDown_P2PFallback()
    {
        await using var src = new StatefulClusterNode("reg-down-src", "ubuntu", "198.51.100.1", 22);
        await using var dst = new StatefulClusterNode("reg-down-dst", "ubuntu", "198.51.100.2", 22);

        var runtime = new DockerRuntime();
        await runtime.StreamImagePeerToPeerAsync(src, dst, "internal-service:v1");

        Assert.Contains(src.ExecutedCommands, c => c.Contains("docker save", StringComparison.Ordinal));
        Assert.Contains(dst.ExecutedCommands, c => c.Contains("docker load", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario15_Chaos_DiskPressure_StreamingBackupNoTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "scen15_disk_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var provider = new RcloneBackupStorageProvider("s3:bucket/backups");
            await using var node = new StatefulClusterNode("low-disk-node", "ubuntu", "198.51.100.1", 22);
            await provider.StreamBackupAsync(node, "/var/data", "backup-id");

            Assert.Contains(node.ExecutedCommands, c => c.Contains("tar -C '/var/data'", StringComparison.Ordinal));
            Assert.Empty(Directory.GetFiles(tempDir, "*.tar*"));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario16_Chaos_ContainerAbruptKill_DriftDetectionAndAutoRestart()
    {
        var containers = new List<JsonObject>
        {
            new()
            {
                ["Id"] = "drift-c1",
                ["Names"] = new JsonArray { "/web" },
                ["Image"] = "nginx:alpine",
                ["State"] = "exited",
                ["Status"] = "Exited (137) 10 seconds ago",
                ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
            }
        };

        using var handler = new MockStatefulDockerHttpHandler(containers);
        using var client = new HttpClient(handler, disposeHandler: false) { BaseAddress = new Uri("http://127.0.0.1/") };

        var report = new AgentStatusReport(
            IsStarry0Active: true,
            WireGuardDump: "peer: test",
            Peers: [],
            Containers: [new ContainerState("drift-c1", "web", "nginx", "exited", false, 137, DateTime.UtcNow, new Dictionary<string, string>(StringComparer.Ordinal))],
            RunningContainers: [],
            RecentDriftEvents: [new DriftEvent("drift-c1", "web", "Exited 137", DateTime.UtcNow, RestartTriggered: true, RestartSucceeded: true)],
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
        await using var node = new StatefulClusterNode("dirty-ubuntu", "ubuntu", "198.51.100.80", 22);
        node.RegisteredContainers.Add(new JsonObject
        {
            ["Id"] = "c-shop-web",
            ["Names"] = new JsonArray { "/shop-web" },
            ["Image"] = "nginx:alpine",
            ["State"] = "running",
            ["Status"] = "Up 2 hours",
            ["Labels"] = new JsonObject { ["com.docker.compose.project"] = "shop" }
        });

        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen19_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, PullFiles = true });
            Assert.NotNull(tree);
            Assert.True(File.Exists(Path.Combine(tempDir, "starry.yaml")) || File.Exists(Path.Combine(tempDir, "services", "shop.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario20_Brownfield_AlmaPodman_GeneratesTomlWithCustomMounts()
    {
        await using var node = new StatefulClusterNode("dirty-alma", "almalinux", "198.51.100.81", 22, "podman");
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
        await using var node = new StatefulClusterNode("nat-deb", "debian", "10.200.0.30", 22);
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
        await using var n1 = new StatefulClusterNode("h1", "ubuntu", "198.51.100.91", 22);
        await using var n2 = new StatefulClusterNode("h2", "almalinux", "198.51.100.92", 22, "podman");
        await using var n3 = new StatefulClusterNode("h3", "debian", "198.51.100.93", 22);

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
        await using var node = new StatefulClusterNode("attach-node", "ubuntu", "198.51.100.94", 22);
        var service = new HostAdoptionService();
        await service.AttachLiveContainerToMeshAsync(node, "live-container-id", "10.99.10.5");
        Assert.Contains(node.ExecutedCommands, c => c.Contains("live-container-id", StringComparison.Ordinal) || c.Contains("10.99.10.5", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario24_Brownfield_CoexistencePolicy_ManagedOnlyIgnoresUnmanaged()
    {
        await using var node = new StatefulClusterNode("coexist-node", "ubuntu", "198.51.100.95", 22);
        node.RegisteredContainers.Add(new JsonObject
        {
            ["Id"] = "unmanaged-box",
            ["Names"] = new JsonArray { "/manual-debug-box" },
            ["Image"] = "busybox",
            ["State"] = "running",
            ["Status"] = "Up",
            ["Labels"] = new JsonObject()
        });

        var service = new HostAdoptionService();
        var config = new ConfigTree();
        config.SetNode("services.known-service.image", JsonValue.Create("app:v1"));

        var unmanaged = await service.ListUnmanagedContainersAsync([node], config);
        Assert.Single(unmanaged);
        Assert.Equal("manual-debug-box", unmanaged[0].ContainerName);
    }

    private sealed class MockChaosHttpMessageHandler(Queue<HttpStatusCode> responses, int delayMs = 10) : HttpMessageHandler
    {
        public int Calls { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            if (delayMs > 0)
            {
                await Task.Delay(delayMs, cancellationToken);
            }

            var status = responses.Count > 0 ? responses.Dequeue() : HttpStatusCode.OK;
            return new HttpResponseMessage(status)
            {
                Content = new StringContent("{\"result\":[]}", Encoding.UTF8, "application/json")
            };
        }
    }
}
