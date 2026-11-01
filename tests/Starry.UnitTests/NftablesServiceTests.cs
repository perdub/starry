using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class NftablesServiceTests
{
    [Fact]
    public void GenerateRuleset_ProducesCanonicalTableInetStarry()
    {
        var forwards = new List<PortForwardRule>
        {
            new(80, "tcp", "10.99.10.1", 80),
            new(443, "tcp", "10.99.10.1", 443),
            new(53, "udp", "10.99.0.1", 53)
        };
        var publicPorts = new List<int> { 80, 443 };

        var ruleset = NftablesService.GenerateRuleset(forwards, publicPorts, sshPort: 2222, wireGuardPort: 51820);

        Assert.Contains("table inet starry", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 2222 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("udp dport 51820 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 80 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 443 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 80 dnat to 10.99.10.1:80", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 443 dnat to 10.99.10.1:443", ruleset, StringComparison.Ordinal);
        Assert.Contains("udp dport 53 dnat to 10.99.0.1:53", ruleset, StringComparison.Ordinal);
        Assert.Contains("oifname \"starry0\" masquerade", ruleset, StringComparison.Ordinal);
    }
}
