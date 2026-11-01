using Starry.Core.Containers;
using Starry.Infrastructure.Containers;
using Xunit;

namespace Starry.UnitTests;

public class ContainerPrioritySchedulerTests
{
    [Fact]
    public void OrderServicesByPriority_OrdersAscendingCorrectly()
    {
        var services = new List<ServiceSpec>
        {
            new() { Name = "proxy", Image = "nginx:alpine", Priority = 90 },
            new() { Name = "database", Image = "postgres:16", Priority = 10 },
            new() { Name = "registry", Image = "registry:2", Priority = 0 },
            new() { Name = "backend", Image = "my-api:v1", Priority = 50 }
        };

        var ordered = ContainerPriorityScheduler.OrderServicesByPriority(services);

        Assert.Equal("registry", ordered[0].Name);
        Assert.Equal("database", ordered[1].Name);
        Assert.Equal("backend", ordered[2].Name);
        Assert.Equal("proxy", ordered[3].Name);
    }

    [Fact]
    public void GroupServicesByPriorityTier_GroupsCorrectly()
    {
        var services = new List<ServiceSpec>
        {
            new() { Name = "db1", Image = "postgres:16", Priority = 10 },
            new() { Name = "db2", Image = "redis:alpine", Priority = 10 },
            new() { Name = "web", Image = "caddy:latest", Priority = 90 }
        };

        var tiers = ContainerPriorityScheduler.GroupServicesByPriorityTier(services);

        Assert.Equal(2, tiers.Count);
        Assert.Equal(10, tiers[0].Key);
        Assert.Equal(2, tiers[0].Count());
        Assert.Equal(90, tiers[1].Key);
        Assert.Single(tiers[1]);
    }
}
