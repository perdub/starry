namespace Starry.Core.Processes;

/// <summary>
/// Execution outcome of a completed process.
/// </summary>
public sealed record ProcessResult(
    int ExitCode,
    string StandardOutput,
    string StandardError,
    TimeSpan ElapsedTime)
{
    public bool IsSuccess => ExitCode == 0;
}
