namespace Starry.Infrastructure.Configuration;

public sealed record ConfigValidationResult(
    bool IsValid,
    IReadOnlyList<ValidationIssue> Issues);
