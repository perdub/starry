using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Xunit;

namespace Starry.UnitTests;

public class ConfigValidatorTests
{
    [Fact]
    public void ConfigValidator_AcceptsValidConfiguration()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.bocchi.port", JsonValue.Create(22));
        tree.SetNode("nodes.bocchi.vpn_ip", JsonValue.Create("10.99.0.2"));
        tree.SetNode("services.frontend.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.frontend.priority", JsonValue.Create(80));
        tree.SetNode("services.frontend.node", JsonValue.Create("bocchi"));

        var result = ConfigValidator.Validate(tree);
        Assert.True(result.IsValid);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public void ConfigValidator_DetectsMissingHost()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.kita.port", JsonValue.Create(22));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => string.Equals(i.Path, "nodes.kita.host", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_DetectsInvalidVpnIp()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.vpn_ip", JsonValue.Create("not-an-ip"));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => string.Equals(i.Path, "nodes.bocchi.vpn_ip", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_FlagsOpenSecretsAsWarnings()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[my-secret-key]"));

        var result = ConfigValidator.Validate(tree);
        Assert.True(result.IsValid); // Warnings do not make configuration invalid
        Assert.Contains(result.Issues, i => i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_FailsOnMalformedEncryptedSecret()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("ENC[INVALID_TAG:secret]"));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => !i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));
    }
}
