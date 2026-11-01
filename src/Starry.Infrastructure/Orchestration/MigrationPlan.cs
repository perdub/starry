namespace Starry.Infrastructure.Orchestration;

public sealed record MigrationPlan(
    string ClusterName,
    IReadOnlyList<PlanAction> Actions)
{
    public bool HasChanges => Actions.Count > 0;
}
