using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Orchestration;
using Xunit;

namespace Starry.UnitTests;

public class MigrationPlannerTests
{
    [Fact]
    public void BuildPlan_GeneratesBootstrapsNetworkAndDeploymentsInOrder()
    {
        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("kessoku-mesh"));
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));
        tree.SetNode("services.db.priority", JsonValue.Create(10));
        tree.SetNode("services.db.node", JsonValue.Create("bocchi"));
        tree.SetNode("services.web.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.web.priority", JsonValue.Create(80));
        tree.SetNode("services.web.node", JsonValue.Create("nijika"));

        var plan = MigrationPlanner.BuildPlan(tree);

        Assert.True(plan.HasChanges);
        Assert.Equal("kessoku-mesh", plan.ClusterName);

        // Nodes come first (priority 0)
        var bootstrapActions = plan.Actions.Where(a => a.ActionType == PlanActionType.CreateNode).ToList();
        Assert.Equal(2, bootstrapActions.Count);

        // WireGuard mesh comes next (priority 5)
        var meshAction = plan.Actions.FirstOrDefault(a => a.ActionType == PlanActionType.ConfigureWireGuardMesh);
        Assert.NotNull(meshAction);
        Assert.Equal(2, meshAction.Dependencies.Count);

        // Nftables comes next (priority 10)
        var nftAction = plan.Actions.FirstOrDefault(a => a.ActionType == PlanActionType.ConfigureNftables);
        Assert.NotNull(nftAction);

        // DB (priority 10 + 50 = 60) must come before Web (priority 80 + 50 = 130)
        var dbAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "db", StringComparison.Ordinal));
        var webAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "web", StringComparison.Ordinal));

        Assert.NotNull(dbAction);
        Assert.NotNull(webAction);
        Assert.True(dbAction.Priority < webAction.Priority);
    }

    [Fact]
    public void BuildPlan_GeneratesBlueGreenMigration_WhenServiceNodeChanges()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.api.image", JsonValue.Create("my-api:v2"));
        tree.SetNode("services.api.node", JsonValue.Create("nijika")); // Desired node: nijika

        // Current live placement: bocchi
        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["api"] = "bocchi"
        };

        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);

        var migrationAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "api", StringComparison.Ordinal));
        Assert.NotNull(migrationAction);
        Assert.Equal(PlanActionType.MigrateServiceBlueGreen, migrationAction.ActionType);
        Assert.Contains("Blue-Green migration", migrationAction.Description, StringComparison.Ordinal);
        Assert.Contains("bocchi", migrationAction.Description, StringComparison.Ordinal);
        Assert.Contains("nijika", migrationAction.Description, StringComparison.Ordinal);
    }
}
