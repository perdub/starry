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

        CreateDirectories(targetDir);
        WriteDefaultConfigs(targetDir, projectName);

        var masterKey = CryptoService.GenerateMasterKeyBase64();
        SaveMasterKeyFile(targetDir, masterKey);
        TrySyncMasterKeyToGitHub(masterKey);
        return (masterKey, targetDir);
    }

    private static void SaveMasterKeyFile(string targetDir, string masterKey)
    {
        var starryDir = Path.Combine(targetDir, ".starry");
        Directory.CreateDirectory(starryDir);
        var keyPath = Path.Combine(starryDir, "master.key");
        File.WriteAllText(keyPath, masterKey.Trim() + "\n");
        if (!OperatingSystem.IsWindows())
        {
            File.SetUnixFileMode(keyPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Best effort GitHub CLI secret push")]
    private static void TrySyncMasterKeyToGitHub(string masterKey)
    {
        try
        {
            var runner = new Starry.Infrastructure.Processes.ProcessRunner();
            var gitRes = runner.RunAsync(new Starry.Core.Processes.ProcessSpec
            {
                Executable = "git",
                Arguments = ["remote", "-v"],
                Timeout = TimeSpan.FromSeconds(2)
            }).GetAwaiter().GetResult();

            if (!gitRes.IsSuccess || string.IsNullOrWhiteSpace(gitRes.StandardOutput))
            {
                return;
            }

            runner.RunAsync(new Starry.Core.Processes.ProcessSpec
            {
                Executable = "gh",
                Arguments = ["secret", "set", "STARRY_MASTER_KEY"],
                StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(masterKey),
                Timeout = TimeSpan.FromSeconds(5)
            }).GetAwaiter().GetResult();
        }
        catch
        {
            // gh or git not available or timed out
        }
    }

    private static void CreateDirectories(string targetDir)
    {
        Directory.CreateDirectory(targetDir);
        Directory.CreateDirectory(Path.Combine(targetDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(targetDir, "services"));
        Directory.CreateDirectory(Path.Combine(targetDir, "files"));
        Directory.CreateDirectory(Path.Combine(targetDir, "domains"));

        File.WriteAllText(Path.Combine(targetDir, "nodes", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "services", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "files", ".gitkeep"), string.Empty);
    }

    private static void WriteDefaultConfigs(string targetDir, string? projectName)
    {
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
            File.WriteAllText(portsPath, "ports: {}\n");
        }

        var volumesPath = Path.Combine(targetDir, "volumes.yaml");
        if (!File.Exists(volumesPath))
        {
            File.WriteAllText(volumesPath, "volumes: {}\n");
        }
    }
}
