namespace Starry.Infrastructure.Orchestration;

public enum PlanActionType
{
    CreateNode,
    ConfigureWireGuardMesh,
    ConfigureNftables,
    DeployService,
    MigrateServiceBlueGreen,
    UpdateDns
}
