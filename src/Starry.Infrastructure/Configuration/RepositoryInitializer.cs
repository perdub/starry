using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Initializes a new Starry GitOps repository directory with standard layout and gitignore.
/// </summary>
public static class RepositoryInitializer
{
    public static (string MasterKeyBase64, string TargetDir) Initialize(string? targetDir = null, string clusterName = "starry-fleet")
    {
        var resolvedDir = string.IsNullOrWhiteSpace(targetDir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(targetDir);

        Directory.CreateDirectory(resolvedDir);
        Directory.CreateDirectory(Path.Combine(resolvedDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(resolvedDir, "services"));
        Directory.CreateDirectory(Path.Combine(resolvedDir, "files"));

        File.WriteAllText(Path.Combine(resolvedDir, "nodes", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(resolvedDir, "services", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(resolvedDir, "files", ".gitkeep"), string.Empty);

        var gitignorePath = Path.Combine(resolvedDir, ".gitignore");
        if (!File.Exists(gitignorePath))
        {
            const string gitignoreContent = """
                .starry/
                *.tmp
                *.bak
                """;
            File.WriteAllText(gitignorePath, gitignoreContent);
        }

        var mainConfigPath = Path.Combine(resolvedDir, "starry.yaml");
        if (!File.Exists(mainConfigPath))
        {
            var configContent = $"""
                version: "1.0"
                cluster_name: "{clusterName}"

                mesh:
                  cidr: "10.99.0.0/16"
                  port: 51820
                  keepalive: 25
                """;
            File.WriteAllText(mainConfigPath, configContent);
        }

        var masterKey = CryptoService.GenerateMasterKeyBase64();
        return (masterKey, resolvedDir);
    }
}
