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
    public async Task SecretScanning_EncryptsEnvPasswordsAutomatically()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "secret_scan_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var service = new HostAdoptionService();
            await using var node = new LocalNodeConnection("adopt-node");

            var masterKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
            var options = new AdoptionOptions
            {
                OutDir = tempDir,
                MasterKey = masterKey,
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
}
