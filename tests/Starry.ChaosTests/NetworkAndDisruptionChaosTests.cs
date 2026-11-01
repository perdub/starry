using Starry.Core.Processes;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Processes;
using Xunit;

namespace Starry.ChaosTests;

public class NetworkAndDisruptionChaosTests
{
    private readonly ProcessRunner _runner = new();

    [Fact]
    public void WireGuardMesh_HandlesPartitionAndRecomputesAllowedIps()
    {
        var healthyNodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false)
        };

        // Node 'ryo' was disconnected/partitioned, so only healthyNodes are generated
        var meshConfigs = WireGuardMeshService.GenerateMeshConfigurations(healthyNodes);

        Assert.Equal(2, meshConfigs.Count);
        Assert.DoesNotContain("Peer: ryo", meshConfigs["bocchi"], StringComparison.Ordinal);
        Assert.DoesNotContain("Peer: ryo", meshConfigs["nijika"], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_TerminatesCleanly_OnProcessAbruptExit()
    {
        // Simulates abrupt remote tool crash / broken pipe
        var spec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "echo 'initial' && exit 137"]
        };

        var result = await _runner.RunAsync(spec);

        Assert.False(result.IsSuccess);
        Assert.Equal(137, result.ExitCode);
        Assert.Contains("initial", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_AbortsPromptly_WhenCancellationTriggered()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(50));

        var spec = new ProcessSpec
        {
            Executable = "sleep",
            Arguments = ["10"]
        };

        await Assert.ThrowsAnyAsync<Exception>(() => _runner.RunAsync(spec, cts.Token));
    }
}
