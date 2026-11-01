using Starry.Infrastructure.Nodes;
using Xunit;

namespace Starry.UnitTests;

public class SshNodeConnectionTests
{
    [Fact]
    public async Task BuildSshArguments_IncludesPortAndBatchMode()
    {
        var options = new SshNodeOptions
        {
            NodeName = "bocchi",
            Host = "192.168.1.100",
            Port = 2222,
            User = "deploy"
        };

        await using var conn = new SshNodeConnection(options);
        var args = conn.BuildSshArguments();

        Assert.Contains("-p", args, StringComparer.Ordinal);
        Assert.Contains("2222", args, StringComparer.Ordinal);
        Assert.Contains("BatchMode=yes", args, StringComparer.Ordinal);
    }

    [Fact]
    public async Task BuildSshArguments_IncludesJumpHost_WhenConfigured()
    {
        var options = new SshNodeOptions
        {
            NodeName = "nat-node",
            Host = "10.0.0.50",
            Port = 22,
            JumpHost = "bastion.example.com"
        };

        await using var conn = new SshNodeConnection(options);
        var args = conn.BuildSshArguments();

        Assert.Contains("-J", args, StringComparer.Ordinal);
        Assert.Contains("bastion.example.com", args, StringComparer.Ordinal);
    }

    [Fact]
    public void LocalNodeConnection_ParsesDistroIdFromOsRelease()
    {
        const string osRelease = """
            NAME="Ubuntu"
            VERSION="24.04 LTS (Noble Numbat)"
            ID=ubuntu
            ID_LIKE=debian
            """;

        var distro = LocalNodeConnection.ParseDistroId(osRelease);
        Assert.Equal("UBUNTU", distro);
    }
}
