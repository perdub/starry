using System.Globalization;
using System.Net;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Dsl;
using Starry.Infrastructure.Agent;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Diagnostics;
using Starry.Infrastructure.Localization;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.UnitTests;

public class V34FeaturesTests
{
    private sealed class MockTestNode : INodeConnection
    {
        public string NodeName => "node-v34";
        public string Host => "198.51.100.20";
        public int Port => 22;
        public string? DistroId => "debian";
        public List<string> CommandsRun { get; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            var cmd = $"{command.Executable} {string.Join(" ", command.Arguments)}";
            CommandsRun.Add(cmd);

            if (cmd.Contains("date -u +%s", StringComparison.Ordinal))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
                return Task.FromResult(new ProcessResult(0, now, string.Empty, TimeSpan.Zero));
            }

            if (cmd.Contains("sysctl", StringComparison.Ordinal))
            {
                return Task.FromResult(new ProcessResult(0, "1", string.Empty, TimeSpan.Zero));
            }

            if (cmd.Contains("test -c /dev/net/tun", StringComparison.Ordinal) ||
                cmd.Contains("nft -v", StringComparison.Ordinal))
            {
                return Task.FromResult(new ProcessResult(0, "nftables v1.0.6", string.Empty, TimeSpan.Zero));
            }

            if (cmd.Contains("df -m", StringComparison.Ordinal))
            {
                return Task.FromResult(new ProcessResult(0, "Filesystem 1M-blocks Used Available Use% Mounted on\n/dev/root 50000 10000 40000 20% /", string.Empty, TimeSpan.Zero));
            }

            if (cmd.Contains("df -i", StringComparison.Ordinal))
            {
                return Task.FromResult(new ProcessResult(0, "Filesystem Inodes IUsed IFree IUse% Mounted on\n/dev/root 1000000 50000 950000 5% /", string.Empty, TimeSpan.Zero));
            }

            if (cmd.Contains("ss -lnt", StringComparison.Ordinal))
            {
                return Task.FromResult(new ProcessResult(0, ":22 :2222 :51820", string.Empty, TimeSpan.Zero));
            }

            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            CommandsRun.Add($"{command.Executable} {string.Join(" ", command.Arguments)}");
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            return Task.FromResult(new HttpClient());
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    [Fact]
    public async Task SshNodeConnection_BuildsWindowsSafeArguments()
    {
        var options = new SshNodeOptions
        {
            NodeName = "bocchi",
            Host = "192.0.2.1",
            Port = 2222,
            User = "starry",
            ControlPathDir = "/tmp/control"
        };

        await using var conn = new SshNodeConnection(options);
        var args = conn.BuildSshArguments();

        Assert.Contains("BatchMode=yes", string.Join(" ", args), StringComparison.Ordinal);
        Assert.Contains("StrictHostKeyChecking=accept-new", string.Join(" ", args), StringComparison.Ordinal);
        Assert.Contains("UserKnownHostsFile=", string.Join(" ", args), StringComparison.Ordinal);
        Assert.DoesNotContain("/dev/null", string.Join(" ", args), StringComparison.Ordinal);
    }

    [Fact]
    public void MasterKey_PersistenceAndResolution_WorksWithFile()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_master_key_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var (key, targetDir) = RepositoryInitializer.Initialize("test-proj", skipFolderCreate: true, baseDir: tempDir);
            Assert.NotEmpty(key);

            var keyFile = Path.Combine(targetDir, ".starry", "master.key");
            Assert.True(File.Exists(keyFile));

            var resolvedKey = MasterKeyResolver.ResolveKey(configDir: targetDir);
            Assert.NotNull(resolvedKey);
            Assert.Equal(key, Convert.ToBase64String(resolvedKey));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void SecretManager_UpdateGetList_PreservesGraphReferences()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_secret_mgr_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var starryYaml = """
                version: "3.0"
                cluster_name: "test-cluster"
                services:
                  web:
                    image: "nginx"
                    env:
                      UPSTREAM_HOST: "$nodes.germany.host"
                      DB_PASS: "PLAIN[initial_secret]"
                """;
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), starryYaml);

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir, resolveGraph: false);

            var masterKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
            var manager = new SecretManager();

            manager.SetSecret(tree, "services.web.env.DB_PASS", "my_new_secret", masterKey, tempDir, engine);

            var diskContent = File.ReadAllText(Path.Combine(tempDir, "starry.yaml"));
            Assert.Contains("$nodes.germany.host", diskContent, StringComparison.Ordinal);
            Assert.Contains("ENC[AES256_GCM", diskContent, StringComparison.Ordinal);

            var reloadedTree = engine.Load(tempDir, resolveGraph: false);
            var plaintext = manager.GetSecret(reloadedTree, "services.web.env.DB_PASS", masterKey);
            Assert.Equal("my_new_secret", plaintext);

            var list = manager.ListSecrets(reloadedTree);
            Assert.Single(list);
            Assert.Equal("services.web.env.DB_PASS", list[0].Path);
            Assert.True(list[0].IsEncrypted);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ConfigValidator_AllowsEncryptedAndGraphHostAndVpnIp()
    {
        var tree = new ConfigTree();
        var nodesObj = new JsonObject
        {
            ["germany"] = new JsonObject
            {
                ["host"] = "ENC[AES256_GCM:v1:...]",
                ["port"] = 22,
                ["vpn_ip"] = "$secrets.vpn_ip"
            },
            ["finland"] = new JsonObject
            {
                ["ssh"] = "ENC[AES256_GCM:v1:...]",
                ["port"] = 22
            }
        };
        tree.Root["nodes"] = nodesObj;

        var result = ConfigValidator.Validate(tree);
        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Issues, i => !i.IsWarning);
    }

    [Fact]
    public void VirtualConfigEngine_LoadsNestedFilesAndResolvesViaDsl()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_nested_files_" + Guid.NewGuid().ToString("N"));
        var filesDir = Path.Combine(tempDir, "files", "stickerbot");
        Directory.CreateDirectory(filesDir);
        try
        {
            File.WriteAllText(Path.Combine(filesDir, "config.json"), "{\"token\":\"12345\"}");
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), """
                version: "3.0"
                cluster_name: "test"
                services:
                  bot:
                    image: "stickerbot"
                    files:
                      /app/config.json: "$files.stickerbot.['config.json']"
                """);

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);

            Assert.NotNull(tree.Root["files"]?["stickerbot"]?["config.json"]);
            Assert.Contains("12345", tree.Root["files"]?["stickerbot"]?["config.json"]?.ToString(), StringComparison.Ordinal);

            var splitDir = Path.Combine(tempDir, "split_out");
            engine.Split(tree, splitDir, "yaml");
            Assert.True(File.Exists(Path.Combine(splitDir, "files", "stickerbot", "config.json")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void BaseSocketContainerRuntime_SpecHash_IsDeterministic()
    {
        var spec1 = new ServiceSpec
        {
            Name = "shop",
            Image = "shop:v1",
            Ports = ["80:80", "443:443"],
            Volumes = ["data:/app/data"],
            Environment = new Dictionary<string, string>(StringComparer.Ordinal) { ["ENV"] = "prod" }
        };

        var spec2 = spec1 with { };
        var spec3 = spec1 with { Image = "shop:v2" };

        var hash1 = BaseSocketContainerRuntime.ComputeSpecHash(spec1);
        var hash2 = BaseSocketContainerRuntime.ComputeSpecHash(spec2);
        var hash3 = BaseSocketContainerRuntime.ComputeSpecHash(spec3);

        Assert.Equal(hash1, hash2, StringComparer.Ordinal);
        Assert.NotEqual(hash1, hash3, StringComparer.Ordinal);
    }

    [Fact]
    public async Task HostPreflightInspector_PerformsAllChecks()
    {
        await using var mockNode = new MockTestNode();
        var results = await HostPreflightInspector.RunDiagnosticsAsync(mockNode, fix: true);

        Assert.Equal(8, results.Count);
        Assert.Contains(results, r => string.Equals(r.CheckName, "UTC Clock Skew", StringComparison.Ordinal) && r.Passed);
        Assert.Contains(results, r => string.Equals(r.CheckName, "Sysctl Routing Flags", StringComparison.Ordinal) && r.Passed);
        Assert.Contains(results, r => string.Equals(r.CheckName, "WireGuard TUN Device", StringComparison.Ordinal) && r.Passed);
        Assert.Contains(results, r => string.Equals(r.CheckName, "Firewall (nftables)", StringComparison.Ordinal) && r.Passed);
    }

    [Fact]
    public void Generators_NftablesRuleset_RespectsNetworkIsolation()
    {
        var subnetPairs = new List<Generators.MeshSubnetPair>
        {
            new("fd67:b00b:0001:0001::/64", "fd67:b00b:0001:0002::/64")
        };

        var fsharpPairs = Microsoft.FSharp.Collections.ListModule.OfSeq(subnetPairs);
        var ruleset = Generators.generateNftablesRuleset(22, 51820, [80, 443], [], fsharpPairs, Microsoft.FSharp.Core.FSharpOption<string>.None);

        Assert.Contains("ip6 saddr . ip6 daddr @allowed_mesh_pairs accept", ruleset, StringComparison.Ordinal);
        Assert.DoesNotContain("oifname \"starry0\" ip6 saddr . ip6 daddr @allowed_mesh_pairs accept\n        iifname \"starry0\" oifname \"starry0\" accept", ruleset, StringComparison.Ordinal);
    }

    [Fact]
    public void DomainEndpointAllocator_ExposesPortAndLists()
    {
        var tree = new ConfigTree();
        var allocated = DomainEndpointAllocator.ExposeService(tree, "web-rule", "shop-web", 3000, "both", 8080, "tcp");

        Assert.Equal("web-rule", allocated.RuleName);
        Assert.Equal(":8080", allocated.From);
        Assert.Equal("shop-web:3000", allocated.To);

        var ports = DomainEndpointAllocator.ListPorts(tree);
        Assert.Single(ports);
        Assert.Equal("web-rule", ports[0].RuleName);
    }
}
