using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Initializes a new Starry GitOps repository directory with standard layout, .gitignore, and fd67:b00b::/32 mesh.
/// </summary>
public static class RepositoryInitializer
{
    public static (string MasterKeyBase64, string TargetDir) Initialize(
        string? projectName = null,
        bool skipFolderCreate = false,
        string? baseDir = null)
    {
        var root = string.IsNullOrWhiteSpace(baseDir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(baseDir);
        var targetDir = !skipFolderCreate && !string.IsNullOrWhiteSpace(projectName)
            ? Path.Combine(root, projectName)
            : root;

        Directory.CreateDirectory(targetDir);
        Directory.CreateDirectory(Path.Combine(targetDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(targetDir, "services"));
        Directory.CreateDirectory(Path.Combine(targetDir, "files"));
        Directory.CreateDirectory(Path.Combine(targetDir, "domains"));

        File.WriteAllText(Path.Combine(targetDir, "nodes", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "services", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "files", ".gitkeep"), string.Empty);

        var gitignorePath = Path.Combine(targetDir, ".gitignore");
        if (!File.Exists(gitignorePath))
        {
            const string gitignoreContent = """
                .starry/
                *.tmp
                *.bak
                """;
            File.WriteAllText(gitignorePath, gitignoreContent);
        }

        var mainConfigPath = Path.Combine(targetDir, "starry.yaml");
        if (!File.Exists(mainConfigPath))
        {
            var clusterName = !string.IsNullOrWhiteSpace(projectName) ? projectName : "starry-fleet";
            var configContent = $"""
                version: "3.0"
                cluster_name: "{clusterName}"

                mesh:
                  prefix: "fd67:b00b::/32"
                  port: 51820
                  keepalive: 25
                """;
            File.WriteAllText(mainConfigPath, configContent);
        }

        var portsPath = Path.Combine(targetDir, "ports.yaml");
        if (!File.Exists(portsPath))
        {
            const string portsContent = """
                ports: {}
                """;
            File.WriteAllText(portsPath, portsContent);
        }

        var volumesPath = Path.Combine(targetDir, "volumes.yaml");
        if (!File.Exists(volumesPath))
        {
            const string volumesContent = """
                volumes: {}
                """;
            File.WriteAllText(volumesPath, volumesContent);
        }

        var masterKey = CryptoService.GenerateMasterKeyBase64();
        return (masterKey, targetDir);
    }
}
