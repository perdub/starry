namespace Starry.Core.Processes;

/// <summary>
/// Strongly-typed process runner interface designed to prevent CLI command injection
/// and safely stream secrets via standard input.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Executes a system process specified by ProcessSpec.
    /// </summary>
    /// <param name="spec">Process execution parameters.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Execution result including exit code and output.</returns>
    Task<ProcessResult> RunAsync(ProcessSpec spec, CancellationToken ct = default);
}
