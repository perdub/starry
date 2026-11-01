using System.Net.Sockets;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Node connection implementation executing directly on the local machine.
/// </summary>
public sealed class LocalNodeConnection : INodeConnection
{
    private readonly IProcessRunner _runner;

    public string NodeName { get; }
    public string Host => "localhost";
    public int Port => 0;
    public string? DistroId { get; private set; }

    private static readonly Uri LocalHostUri = new("http://127.0.0.1/");

    public LocalNodeConnection(string nodeName = "local", IProcessRunner? runner = null)
    {
        NodeName = nodeName;
        _runner = runner ?? new ProcessRunner();
    }

    public async Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _runner.RunAsync(command, ct).ConfigureAwait(false);
    }

    public async Task<ProcessResult> ExecuteStreamingCommandAsync(
        ProcessSpec command,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _runner.RunStreamingAsync(command, stdinStream, stdoutStream, ct).ConfigureAwait(false);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "SocketsHttpHandler ownership transferred to HttpClient")]
    public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(socketPath);

        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                var endpoint = new UnixDomainSocketEndPoint(socketPath);
                await socket.ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
                return new NetworkStream(socket, ownsSocket: true);
            }
        };

        var client = new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = LocalHostUri
        };

        return Task.FromResult(client);
    }

    public async Task InitializeDistroAsync(CancellationToken ct = default)
    {
        if (File.Exists("/etc/os-release"))
        {
            var content = await File.ReadAllTextAsync("/etc/os-release", ct).ConfigureAwait(false);
            DistroId = ParseDistroId(content);
        }
    }

    public static string? ParseDistroId(string osReleaseContent)
    {
        using var reader = new StringReader(osReleaseContent);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("ID=", StringComparison.OrdinalIgnoreCase))
            {
                var id = line["ID=".Length..].Trim().Trim('"').Trim('\'');
                return id.ToUpperInvariant();
            }
        }
        return null;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
