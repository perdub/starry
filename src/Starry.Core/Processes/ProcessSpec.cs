namespace Starry.Core.Processes;

/// <summary>
/// Specification for starting a secure child process with protection against command injection.
/// </summary>
public sealed record ProcessSpec
{
    public required string Executable { get; init; }
    public IReadOnlyList<string> Arguments { get; init; } = [];
    public string? WorkingDirectory { get; init; }
    public IReadOnlyDictionary<string, string> EnvironmentVariables { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public ReadOnlyMemory<byte>? StandardInputBytes { get; init; }
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(5);
}
