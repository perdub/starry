using System.Diagnostics;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Processes;

namespace Starry.E2ETests;

/// <summary>
/// Sets up a live multi-node Linux SSH cluster running inside isolated Docker containers
/// for testing real SSH transport, ControlMaster multiplexing, volume streaming, and key rotation.
/// </summary>
public sealed class LiveDockerSshClusterFixture : IAsyncDisposable
{
    private static readonly ProcessRunner Runner = new();
    private readonly string _tempKeyDir = Path.Combine(Path.GetTempPath(), $"starry-e2e-keys-{Guid.NewGuid():N}");
    private string _privateKeyPath = string.Empty;
    private string _privateKeyContent = string.Empty;
    private string _publicKeyContent = string.Empty;

    public bool IsAvailable { get; private set; }
    public SshNodeConnection? Node1 { get; private set; }
    public SshNodeConnection? Node2 { get; private set; }

    public async Task InitializeAsync()
    {
        // 1. Check if Docker daemon is responsive
        try
        {
            var checkRes = await Runner.RunAsync(new Core.Processes.ProcessSpec
            {
                Executable = "docker",
                Arguments = ["info"],
                Timeout = TimeSpan.FromSeconds(5)
            });

            if (!checkRes.IsSuccess)
            {
                return;
            }
        }
        catch
        {
            return;
        }

        Directory.CreateDirectory(_tempKeyDir);
        _privateKeyPath = Path.Combine(_tempKeyDir, "id_ed25519");

        // 2. Generate Ed25519 SSH keypair
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "ssh-keygen",
            Arguments = ["-t", "ed25519", "-N", "", "-f", _privateKeyPath, "-q"]
        });

        _privateKeyContent = await File.ReadAllTextAsync(_privateKeyPath);
        _publicKeyContent = (await File.ReadAllTextAsync(_privateKeyPath + ".pub")).Trim();

        // 3. Create Docker network starry-e2e-pub
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["network", "create", "--subnet", "172.28.0.0/16", "starry-e2e-pub"]
        });

        // 4. Ensure starry-ssh-node image exists
        var imgCheck = await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["image", "inspect", "starry-ssh-node"]
        });

        if (!imgCheck.IsSuccess)
        {
            var dockerfile = "FROM alpine:3.20\nRUN apk add --no-cache openssh-server tar zstd rsync curl iproute2 bash\nRUN ssh-keygen -A && mkdir -p /root/.ssh && chmod 700 /root/.ssh\nRUN echo 'PermitRootLogin yes' >> /etc/ssh/sshd_config\nEXPOSE 22\nCMD [\"/usr/sbin/sshd\", \"-D\", \"-e\"]\n";
            await Runner.RunAsync(new Core.Processes.ProcessSpec
            {
                Executable = "docker",
                Arguments = ["build", "-t", "starry-ssh-node", "-"],
                StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(dockerfile)
            });
        }

        // 5. Spin up node-1 (172.28.0.11) and node-2 (172.28.0.12)
        await StartContainerAsync("starry-live-node-1", "172.28.0.11");
        await StartContainerAsync("starry-live-node-2", "172.28.0.12");

        // Wait up to 5s for SSH listeners to be ready
        await Task.Delay(1500);

        Node1 = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "live-node-1",
            Host = "172.28.0.11",
            Port = 22,
            User = "root",
            PrivateKeyContent = _privateKeyContent
        });

        Node2 = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "live-node-2",
            Host = "172.28.0.12",
            Port = 22,
            User = "root",
            PrivateKeyContent = _privateKeyContent
        });

        await Node1.InitializeAsync();
        await Node2.InitializeAsync();

        IsAvailable = true;
    }

    private async Task StartContainerAsync(string containerName, string ipAddress)
    {
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["rm", "-f", containerName]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["run", "-d", "--name", containerName, "--net", "starry-e2e-pub", "--ip", ipAddress, "starry-ssh-node"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["exec", containerName, "mkdir", "-p", "/root/.ssh"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["exec", "-i", containerName, "sh", "-c", "cat > /root/.ssh/authorized_keys && chmod 600 /root/.ssh/authorized_keys"],
            StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(_publicKeyContent)
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (Node1 != null)
        {
            await Node1.DisposeAsync();
        }

        if (Node2 != null)
        {
            await Node2.DisposeAsync();
        }

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["rm", "-f", "starry-live-node-1", "starry-live-node-2"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["network", "rm", "starry-e2e-pub"]
        });

        if (Directory.Exists(_tempKeyDir))
        {
            try
            {
                Directory.Delete(_tempKeyDir, recursive: true);
            }
            catch (IOException)
            {
                // Ignore cleanup error
            }
        }
    }
}
