namespace Starry.Infrastructure.Orchestration;

public sealed record PlanAction(
    string Id,
    PlanActionType ActionType,
    string Target,
    string Description,
    int Priority = 50,
    IReadOnlyList<string>? Dependencies = null)
{
    public IReadOnlyList<string> Dependencies { get; init; } = Dependencies ?? [];
}
