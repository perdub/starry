namespace Starry.Infrastructure.Configuration;

public sealed record ValidationIssue(
    string Path,
    string Message,
    string? FilePath = null,
    bool IsWarning = false);
