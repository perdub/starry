namespace Starry.Core.Processes;

/// <summary>
/// Strongly-typed process runner interface designed to prevent CLI command injection
/// and safely stream secrets via standard input or binary streams.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Executes a system process specified by ProcessSpec with buffered text output.
    /// </summary>
    Task<ProcessResult> RunAsync(ProcessSpec spec, CancellationToken ct = default);

    /// <summary>
    /// Executes a system process with direct binary streaming for standard input and output.
    /// </summary>
    Task<ProcessResult> RunStreamingAsync(
        ProcessSpec spec,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default);
}
