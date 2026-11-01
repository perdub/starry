using Starry.Core.Processes;
using Starry.Infrastructure.Processes;
using Xunit;

namespace Starry.UnitTests;

public class ProcessRunnerTests
{
    private readonly ProcessRunner _runner = new();

    [Fact]
    public async Task ProcessRunner_ExecutesEcho_Successfully()
    {
        var spec = new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["hello", "starry"]
        };

        var result = await _runner.RunAsync(spec);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("hello starry", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_PassesEnvironmentVariables()
    {
        var spec = new ProcessSpec
        {
            Executable = "printenv",
            Arguments = ["TEST_VAR"],
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["TEST_VAR"] = "StarrySecret123"
            }
        };

        var result = await _runner.RunAsync(spec);

        Assert.True(result.IsSuccess);
        Assert.Contains("StarrySecret123", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_TimesOut_WhenExceedingLimit()
    {
        var spec = new ProcessSpec
        {
            Executable = "sleep",
            Arguments = ["5"],
            Timeout = TimeSpan.FromMilliseconds(100)
        };

        await Assert.ThrowsAsync<TimeoutException>(() => _runner.RunAsync(spec));
    }
}
