using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Orchestration;
using Xunit;

namespace Starry.E2ETests;

public class V3PipelineIntegrationTests
{
    private sealed class MockTestNodeConnection(string name, string host) : INodeConnection
    {
        public string NodeName => name;
        public string Host => host;
        public int Port => 22;
        public string? DistroId => "alpine";
        public List<string> ExecutedCommands { get; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            ExecutedCommands.Add($"{command.Executable} {string.Join(" ", command.Arguments)}");
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            ExecutedCommands.Add($"{command.Executable} {string.Join(" ", command.Arguments)}");
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockV3DockerHandler();
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockV3DockerHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.PathAndQuery ?? string.Empty;
            if (path.Contains("/containers/json", StringComparison.Ordinal))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"Id\":\"c1\",\"Names\":[\"/shop-web\"],\"State\":\"running\",\"Status\":\"Up\"}]")
                });
            }

            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":\"c1\"}")
            });
        }
    }

    private sealed class MockMockCfDnsProvider : IDnsProvider
    {
        public string ProviderName => "cloudflare-mock";
        public List<(string Zone, DnsRecord Record)> UpsertedRecords { get; } = [];
        public List<(string Zone, string Hostname, int Port)> OriginRules { get; } = [];

        public Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default)
        {
            return Task.FromResult<IReadOnlyList<DnsRecord>>([]);
        }

        public Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default)
        {
            UpsertedRecords.Add((zoneDomain, record));
            return Task.CompletedTask;
        }

        public Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task V3Pipeline_LoadsDirectory_ResolvesGraph_AndExecutesPlanEndToEnd()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_v3_e2e_" + Guid.NewGuid().ToString("N"));
        var filesDir = Path.Combine(tempDir, "files");
        var nodesDir = Path.Combine(tempDir, "nodes");
        var servicesDir = Path.Combine(tempDir, "services");

        Directory.CreateDirectory(tempDir);
        Directory.CreateDirectory(filesDir);
        Directory.CreateDirectory(nodesDir);
        Directory.CreateDirectory(servicesDir);

        try
        {
            const string starryYaml = """
                version: "3.0"
                cluster_name: "kessoku-v3"
                mesh:
                  prefix: "fd67:b00b::/32"
                  port: 51820
                  keepalive: 25

                presets:
                  base_app:
                    restart: "always"
                    working_dir: "/app"
                    cap_add: ["NET_ADMIN"]
                    shm_size: 67108864
                    mem_limit: "512m"
                    networks: ["app_net"]
                    egress: "$nodes.germany"

                volumes:
                  pg_data:
                    path: "/var/data/postgres"

                ports:
                  alt_web:
                    from: "$nodes.finland.ipv4:8443"
                    to: "shop-web:3000"

                domains:
                  rocks:
                    bocchi:
                      dns_provider: "cf"
                      node: "$nodes.germany"
                      shop:
                        port: "$ports.alt_web"
                        http: "shop-web:3000"
                      vpn:
                        sni: "xray:8443"
                        proxy_protocol: true
                """;

            const string germanyYaml = """
                ssh: "root@142.132.10.20:22"
                is_public: true
                """;

            const string finlandYaml = """
                ssh: "root@65.21.10.30:22"
                is_public: true
                """;

            const string shopServiceYaml = """
                $: "$presets.base_app"
                image: "shop:v3"
                node: "germany"
                command: "node server.js"
                volumes:
                  /var/lib/postgresql/data: "$volumes.pg_data"
                files:
                  /etc/nginx/nginx.conf: "$files.['nginx.conf']"
                """;

            const string nginxConf = """
                # Upstream for shop
                upstream backend {
                    server $nodes.germany.host:3000;
                }
                """;

            await File.WriteAllTextAsync(Path.Combine(tempDir, "starry.yaml"), starryYaml);
            await File.WriteAllTextAsync(Path.Combine(nodesDir, "germany.yaml"), germanyYaml);
            await File.WriteAllTextAsync(Path.Combine(nodesDir, "finland.yaml"), finlandYaml);
            await File.WriteAllTextAsync(Path.Combine(servicesDir, "shop-web.yaml"), shopServiceYaml);
            await File.WriteAllTextAsync(Path.Combine(filesDir, "nginx.conf"), nginxConf);

            // 1. Test VirtualConfigEngine.Load with automatic graph resolution
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);

            // Verify files/* auto-loading and string interpolation
            var loadedNginxConf = tree.Root["files"]?["nginx.conf"]?.ToString();
            Assert.NotNull(loadedNginxConf);
            Assert.Contains("server 142.132.10.20:3000;", loadedNginxConf, StringComparison.Ordinal);

            // Verify $: object inheritance and property merging
            var shopObj = tree.Root["services"]?["shop-web"] as JsonObject;
            Assert.NotNull(shopObj);
            Assert.Equal("always", shopObj["restart"]?.ToString());
            Assert.Equal("/app", shopObj["working_dir"]?.ToString());
            Assert.Equal("512m", shopObj["mem_limit"]?.ToString());

            // 2. Test Domain Routing Traversal
            var routes = DomainRoutingService.TraverseDomains(tree);
            Assert.Equal(2, routes.Count);

            var shopRoute = routes.FirstOrDefault(r => string.Equals(r.FullHostname, "shop.bocchi.rocks", StringComparison.Ordinal));
            Assert.NotNull(shopRoute);
            Assert.Equal("bocchi.rocks", shopRoute.ZoneDomain);
            Assert.Equal("shop-web:3000", shopRoute.HttpTarget);
            Assert.Equal(8443, shopRoute.EntryPort);

            var vpnRoute = routes.FirstOrDefault(r => string.Equals(r.FullHostname, "vpn.bocchi.rocks", StringComparison.Ordinal));
            Assert.NotNull(vpnRoute);
            Assert.Equal("xray:8443", vpnRoute.SniTarget);
            Assert.True(vpnRoute.ProxyProtocol);

            // 3. Test MigrationPlanner DAG generation
            var plan = MigrationPlanner.BuildPlan(tree);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.CreateNode);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.ConfigureWireGuardMesh);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.ConfigureNftables);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.DeployService);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.UpdateDns);

            // 4. Test ClusterOrchestrator Execution
            var mockDns = new MockMockCfDnsProvider();
            var orchestrator = new ClusterOrchestrator(dnsProvider: mockDns);

            await using var node1 = new MockTestNodeConnection("germany", "142.132.10.20");
            await using var node2 = new MockTestNodeConnection("finland", "65.21.10.30");

            await orchestrator.ExecutePlanAsync(plan, tree, [node1, node2]);

            // Verify DNS records were upserted
            Assert.Contains(mockDns.UpsertedRecords, r => string.Equals(r.Record.Name, "shop.bocchi.rocks", StringComparison.Ordinal) && string.Equals(r.Zone, "bocchi.rocks", StringComparison.Ordinal));

            // 5. Test Split with v3 structures
            var splitDir = Path.Combine(tempDir, "split_output");
            engine.Split(tree, splitDir, "yaml");

            Assert.True(File.Exists(Path.Combine(splitDir, "ports.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "volumes.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "domains", "domains.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "files", "nginx.conf")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
