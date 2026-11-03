using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Remote node connection over SSH with Jump-host (-J) support, ControlMaster multiplexing,
/// in-memory private key injection, and secure process execution.
/// </summary>
public sealed class SshNodeConnection : INodeConnection
{
    private static readonly string SshExecutablePath = File.Exists("/usr/bin/ssh") ? "/usr/bin/ssh" : "ssh";
    private static readonly Uri DefaultRemoteSocketUri = new("http://127.0.0.1/");

    private readonly SshNodeOptions _options;
    private readonly IProcessRunner _runner;
    private readonly List<Process> _tunnelProcesses = [];
    private readonly List<string> _tempSocketPaths = [];
    private string? _tempKeyPath;
    private string? _controlSocketPath;
    private bool _initialized;

    public string NodeName => _options.NodeName;
    public string Host => _options.Host;
    public int Port => _options.Port;
    public string? DistroId { get; private set; }

    public SshNodeConnection(SshNodeOptions options, IProcessRunner? runner = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
        _runner = runner ?? new ProcessRunner();
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_initialized)
        {
            return;
        }

        Directory.CreateDirectory(_options.ControlPathDir);
        _controlSocketPath = Path.Combine(_options.ControlPathDir, $"starry-ssh-{_options.User}@{_options.Host}:{_options.Port}-{Guid.NewGuid():N}");

        if (!string.IsNullOrWhiteSpace(_options.PrivateKeyContent))
        {
            _tempKeyPath = Path.Combine(_options.ControlPathDir, $".starry-key-{Guid.NewGuid():N}.key");
            await File.WriteAllTextAsync(_tempKeyPath, _options.PrivateKeyContent.Trim() + "\n", ct).ConfigureAwait(false);

            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(_tempKeyPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            }
        }

        var osReleaseResult = await ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "cat",
            Arguments = ["/etc/os-release"]
        }, ct).ConfigureAwait(false);

        if (osReleaseResult.IsSuccess)
        {
            DistroId = LocalNodeConnection.ParseDistroId(osReleaseResult.StandardOutput);
        }

        _initialized = true;
    }

    public async Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var sshSpec = BuildSshProcessSpec(command);
        return await _runner.RunAsync(sshSpec, ct).ConfigureAwait(false);
    }

    public async Task<ProcessResult> ExecuteStreamingCommandAsync(
        ProcessSpec command,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var sshSpec = BuildSshProcessSpec(command);
        return await _runner.RunStreamingAsync(sshSpec, stdinStream, stdoutStream, ct).ConfigureAwait(false);
    }

    private ProcessSpec BuildSshProcessSpec(ProcessSpec command)
    {
        var sshArgs = new List<string>(BuildSshArguments())
        {
            $"{_options.User}@{_options.Host}"
        };

        if (string.Equals(command.Executable, "sh", StringComparison.Ordinal) &&
            command.Arguments.Count == 2 &&
            string.Equals(command.Arguments[0], "-c", StringComparison.Ordinal))
        {
            sshArgs.Add(command.Arguments[1]);
        }
        else
        {
            sshArgs.Add(command.Executable);
            foreach (var arg in command.Arguments)
            {
                sshArgs.Add(arg);
            }
        }

        return new ProcessSpec
        {
            Executable = SshExecutablePath,
            Arguments = sshArgs,
            StandardInputBytes = command.StandardInputBytes,
            Timeout = command.Timeout
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "SocketsHttpHandler ownership transferred to HttpClient")]
    public async Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(socketPath);

        var localSocketPath = Path.Combine(_options.ControlPathDir, $".sock-{Guid.NewGuid():N}.sock");
        _tempSocketPaths.Add(localSocketPath);

        var tunnelArgs = new List<string>(BuildSshArguments())
        {
            "-N",
            "-L",
            $"{localSocketPath}:{socketPath}",
            $"{_options.User}@{_options.Host}"
        };

        var startInfo = new ProcessStartInfo
        {
            FileName = SshExecutablePath,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var arg in tunnelArgs)
        {
            startInfo.ArgumentList.Add(arg);
        }

        var process = new Process { StartInfo = startInfo };
        process.Start();
        _tunnelProcesses.Add(process);

        // Poll up to 10 seconds for the forwarded Unix domain socket to appear
        var attempts = 0;
        const int maxAttempts = 200; // 200 * 50ms = 10 seconds
        while (!File.Exists(localSocketPath) && attempts < maxAttempts)
        {
            if (process.HasExited)
            {
                throw new InvalidOperationException($"SSH socket tunnel exited prematurely with code {process.ExitCode}.");
            }

            await Task.Delay(50, ct).ConfigureAwait(false);
            attempts++;
        }

        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                var endpoint = new UnixDomainSocketEndPoint(localSocketPath);
                await socket.ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
                return new NetworkStream(socket, ownsSocket: true);
            }
        };

        var client = new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = DefaultRemoteSocketUri
        };

        return client;
    }

    public IReadOnlyList<string> BuildSshArguments()
    {
        var args = new List<string>
        {
            "-p", _options.Port.ToString(CultureInfo.InvariantCulture),
            "-o", "BatchMode=yes",
            "-o", "StrictHostKeyChecking=no",
            "-o", "UserKnownHostsFile=/dev/null",
            "-o", "IdentitiesOnly=yes"
        };

        if (!string.IsNullOrEmpty(_controlSocketPath))
        {
            args.Add("-o");
            args.Add("ControlMaster=auto");
            args.Add("-o");
            args.Add($"ControlPath={_controlSocketPath}");
            args.Add("-o");
            args.Add("ControlPersist=10m");
        }

        if (!string.IsNullOrEmpty(_options.JumpHost))
        {
            args.Add("-J");
            args.Add(_options.JumpHost);
        }

        if (!string.IsNullOrEmpty(_tempKeyPath))
        {
            args.Add("-i");
            args.Add(_tempKeyPath);
        }

        return args;
    }

    public async ValueTask DisposeAsync()
    {
        CleanupTunnelProcesses();
        CleanupTempSockets();
        await CloseControlMasterAsync().ConfigureAwait(false);
        CleanupTempKey();
    }

    private void CleanupTunnelProcesses()
    {
        foreach (var tunnel in _tunnelProcesses)
        {
            try
            {
                if (!tunnel.HasExited)
                {
                    tunnel.Kill(entireProcessTree: true);
                }
                tunnel.Dispose();
            }
            catch (InvalidOperationException)
            {
                // Process already terminated
            }
        }
        _tunnelProcesses.Clear();
    }

    private void CleanupTempSockets()
    {
        foreach (var sockPath in _tempSocketPaths.Where(File.Exists))
        {
            try
            {
                File.Delete(sockPath);
            }
            catch (IOException)
            {
                // Ignore cleanup error
            }
        }
        _tempSocketPaths.Clear();
    }

    private async Task CloseControlMasterAsync()
    {
        if (string.IsNullOrEmpty(_controlSocketPath) || !File.Exists(_controlSocketPath))
        {
            return;
        }

        var closeArgs = new List<string>
        {
            "-O", "exit",
            "-o", $"ControlPath={_controlSocketPath}",
            $"{_options.User}@{_options.Host}"
        };

        try
        {
            await _runner.RunAsync(new ProcessSpec
            {
                Executable = SshExecutablePath,
                Arguments = closeArgs,
                Timeout = TimeSpan.FromSeconds(3)
            }).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Ignore failure during connection shutdown
        }
        catch (InvalidOperationException)
        {
            // Ignore failure during connection shutdown
        }
    }

    private void CleanupTempKey()
    {
        if (!string.IsNullOrEmpty(_tempKeyPath) && File.Exists(_tempKeyPath))
        {
            try
            {
                File.Delete(_tempKeyPath);
            }
            catch (IOException)
            {
                // Best effort key wipe
            }
        }
    }
}
