using Starry.Core.Processes;

namespace Starry.Core.Nodes;

/// <summary>
/// Represents metadata and communication contract for a cluster node (local or remote via SSH).
/// </summary>
public interface INodeConnection : IAsyncDisposable
{
    /// <summary>
    /// Node unique identifier name (e.g. "bocchi", "nijika", "ryo").
    /// </summary>
    string NodeName { get; }

    /// <summary>
    /// Target host address or IP.
    /// </summary>
    string Host { get; }

    /// <summary>
    /// Target SSH port.
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Operating system distribution identifier (e.g. "ubuntu", "debian", "almalinux", "alpine", "arch").
    /// </summary>
    string? DistroId { get; }

    /// <summary>
    /// Executes a command on the target node.
    /// </summary>
    /// <param name="command">Command and arguments.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default);

    /// <summary>
    /// Opens an HTTP client to communicate with the container engine socket over SSH/Unix domain socket.
    /// </summary>
    /// <param name="socketPath">Socket path on the node.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Configured HttpClient.</returns>
    Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default);
}
