namespace Starry.Infrastructure.Diagnostics;

public sealed record PreflightCheckResult(
    string CheckName,
    bool Passed,
    string Details,
    bool WasFixed = false);
