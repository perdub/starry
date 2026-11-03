using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Computes directed acyclic execution plan (DAG) comparing desired configuration against cluster state.
/// </summary>
public static class MigrationPlanner
{
    public static MigrationPlan BuildPlan(
        ConfigTree desiredConfig,
        IReadOnlyDictionary<string, string>? currentServiceNodePlacements = null,
        bool onlyChangedServices = false)
    {
        ArgumentNullException.ThrowIfNull(desiredConfig);

        var actions = new List<PlanAction>();
        var clusterName = desiredConfig.Root["cluster_name"]?.ToString() ?? "starry-cluster";

        var nodeCount = PlanNodeActions(desiredConfig, actions);
        PlanNetworkActions(nodeCount, actions);
        PlanServiceActions(desiredConfig, currentServiceNodePlacements, actions, onlyChangedServices);
        PlanDnsActions(desiredConfig, actions);

        var sortedActions = actions
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.Id, StringComparer.Ordinal)
            .ToList();

        return new MigrationPlan(clusterName, sortedActions);
    }

    private static int PlanNodeActions(ConfigTree desiredConfig, List<PlanAction> actions)
    {
        var count = 0;
        if (desiredConfig.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            foreach (var (nodeName, _) in nodesObj)
            {
                count++;
                actions.Add(new PlanAction(
                    Id: $"node:{nodeName}:bootstrap",
                    ActionType: PlanActionType.CreateNode,
                    Target: nodeName,
                    Description: $"Bootstrap node '{nodeName}' with baseline system packages",
                    Priority: 0));
            }
        }
        return count;
    }

    private static void PlanNetworkActions(int nodeCount, List<PlanAction> actions)
    {
        if (nodeCount == 0)
        {
            return;
        }

        var nodeDependencyIds = actions.Where(a => a.ActionType == PlanActionType.CreateNode).Select(a => a.Id).ToList();

        actions.Add(new PlanAction(
            Id: "network:wireguard:mesh",
            ActionType: PlanActionType.ConfigureWireGuardMesh,
            Target: "starry0",
            Description: $"Synchronize WireGuard P2P mesh across {nodeCount} node(s) with Keepalive=25",
            Priority: 5,
            Dependencies: nodeDependencyIds));

        actions.Add(new PlanAction(
            Id: "network:nftables:ruleset",
            ActionType: PlanActionType.ConfigureNftables,
            Target: "inet starry",
            Description: "Apply host firewall policies and DNAT rules in table inet starry",
            Priority: 10,
            Dependencies: ["network:wireguard:mesh"]));
    }

    private static void PlanServiceActions(
        ConfigTree desiredConfig,
        IReadOnlyDictionary<string, string>? currentServiceNodePlacements,
        List<PlanAction> actions,
        bool onlyChangedServices = false)
    {
        if (!desiredConfig.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            return;
        }

        var placements = currentServiceNodePlacements ?? new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var (svcName, svcVal) in servicesObj)
        {
            if (svcVal is not JsonObject sObj)
            {
                continue;
            }

            var image = sObj["image"]?.ToString() ?? "unknown";
            var priority = sObj.TryGetPropertyValue("priority", out var pVal) &&
                           int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
                ? p
                : 50;
            var targetNode = sObj["node"]?.ToString();

            var hasOldPlacement = targetNode != null &&
                                  placements.TryGetValue(svcName, out var currentNode) &&
                                  !string.Equals(currentNode, targetNode, StringComparison.OrdinalIgnoreCase);

            var isAlreadyConverged = targetNode != null &&
                                     placements.TryGetValue(svcName, out var runningNode) &&
                                     string.Equals(runningNode, targetNode, StringComparison.OrdinalIgnoreCase);

            if (onlyChangedServices && isAlreadyConverged)
            {
                continue;
            }

            if (hasOldPlacement)
            {
                var oldNode = placements[svcName];
                actions.Add(new PlanAction(
                    Id: $"service:{svcName}:blue-green",
                    ActionType: PlanActionType.MigrateServiceBlueGreen,
                    Target: svcName,
                    Description: $"Zero-downtime Blue-Green migration of '{svcName}' from '{oldNode}' to '{targetNode}'",
                    Priority: 50 + priority,
                    Dependencies: ["network:wireguard:mesh", "network:nftables:ruleset"]));
            }
            else
            {
                actions.Add(new PlanAction(
                    Id: $"service:{svcName}:deploy",
                    ActionType: PlanActionType.DeployService,
                    Target: svcName,
                    Description: $"Deploy container '{svcName}' [{image}] on '{targetNode ?? "auto"}' (priority: {priority})",
                    Priority: 50 + priority,
                    Dependencies: ["network:wireguard:mesh", "network:nftables:ruleset"]));
            }
        }
    }

    private static void PlanDnsActions(ConfigTree desiredConfig, List<PlanAction> actions)
    {
        if (desiredConfig.Root.TryGetPropertyValue("domains", out var domNode) && domNode is JsonObject)
        {
            actions.Add(new PlanAction(
                Id: "dns:domains:sync",
                ActionType: PlanActionType.UpdateDns,
                Target: "domains",
                Description: "Synchronize hierarchical domains, certificates, and Cloudflare origin rules",
                Priority: 95));
        }
        else if (desiredConfig.Root.TryGetPropertyValue("dns", out var dnsNode) && dnsNode is JsonObject)
        {
            actions.Add(new PlanAction(
                Id: "dns:cluster:sync",
                ActionType: PlanActionType.UpdateDns,
                Target: "dns",
                Description: "Synchronize DNS records with configured DNS provider",
                Priority: 95));
        }
    }
}
