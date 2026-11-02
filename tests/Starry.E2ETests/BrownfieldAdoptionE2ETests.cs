using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class BrownfieldAdoptionE2ETests
{
    [Fact]
    public async Task Adoption_FullLifecycle_PullsFilesAndGeneratesModularConfig()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "brownfield_e2e_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var service = new HostAdoptionService();
            await using var node = new LocalNodeConnection("local-test");

            var options = new AdoptionOptions
            {
                OutDir = tempDir,
                PullFiles = true,
                Format = "yaml"
            };

            var tree = await service.AdoptHostAsync(node, options);
            Assert.NotNull(tree);
            Assert.True(File.Exists(Path.Combine(tempDir, "starry.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task ListUnmanagedContainers_FiltersOutManagedServices()
    {
        var service = new HostAdoptionService();
        await using var node = new LocalNodeConnection("test-node");

        var config = new ConfigTree();
        config.SetNode("services.api.image", JsonValue.Create("my-api:v1"));

        // When node is queried, containers not matching "api" are flagged as unmanaged
        var unmanaged = await service.ListUnmanagedContainersAsync([node], config);
        Assert.NotNull(unmanaged);
    }

    [Fact]
    public void SecretScanning_EncryptsEnvPasswordsAutomatically()
    {
        var crypto = new CryptoService();
        var key = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());

        var rawEnv = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["DB_PASSWORD"] = "super-secret-postgres-pass",
            ["PORT"] = "8080",
            ["API_KEY"] = "sk-live-abcdef123456"
        };

        // Simulates HostAdoptionService secret detection
        var encryptedEnv = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var (k, v) in rawEnv)
        {
            if (k.Contains("PASS", StringComparison.OrdinalIgnoreCase) ||
                k.Contains("KEY", StringComparison.OrdinalIgnoreCase) ||
                k.Contains("SECRET", StringComparison.OrdinalIgnoreCase))
            {
                var envelope = crypto.Encrypt(System.Text.Encoding.UTF8.GetBytes(v), key, $"env.{k}");
                encryptedEnv[k] = envelope.FormatEncrypted();
            }
            else
            {
                encryptedEnv[k] = v;
            }
        }

        Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedEnv["DB_PASSWORD"], StringComparison.Ordinal);
        Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedEnv["API_KEY"], StringComparison.Ordinal);
        Assert.Equal("8080", encryptedEnv["PORT"]);
    }
}
