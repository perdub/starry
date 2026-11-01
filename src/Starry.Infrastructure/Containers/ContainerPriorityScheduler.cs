using Starry.Core.Containers;

namespace Starry.Infrastructure.Containers;

/// <summary>
/// Orders and schedules container deployments based on numeric priority
/// (0: Registry -> 10: Database -> 50: Backends -> 90: Ingress/Proxy).
/// </summary>
public static class ContainerPriorityScheduler
{
    public const int DefaultPriority = 50;

    public static IReadOnlyList<ServiceSpec> OrderServicesByPriority(IEnumerable<ServiceSpec> services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .OrderBy(s => s.Priority)
            .ThenBy(s => s.Name, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<IGrouping<int, ServiceSpec>> GroupServicesByPriorityTier(IEnumerable<ServiceSpec> services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .OrderBy(s => s.Priority)
            .GroupBy(s => s.Priority)
            .ToList();
    }
}
