using System.Net;
using System.Text;
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class InternalDnsServerTests
{
    [Fact]
    public async Task HandleDnsQuery_ResolvesInternalDomainsCorrectly()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));
        server.RegisterRecord("api.internal", IPAddress.Parse("10.99.10.5"));

        // Construct a standard DNS query packet for "api.internal" (Type A, Class IN)
        var query = BuildQueryPacket("api.internal");

        var response = server.HandleDnsQuery(query);
        Assert.NotNull(response);

        // Check header flags in response (0x8180 = standard response, no error)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);

        // Check answer count = 1
        Assert.Equal(0x01, response[7]);

        // Last 4 bytes should be IPv4: 10.99.10.5
        var ipBytes = response[^4..];
        Assert.Equal(new byte[] { 10, 99, 10, 5 }, ipBytes);
    }

    [Fact]
    public async Task HandleDnsQuery_ReturnsNxDomainForUnknownDomain()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));

        var query = BuildQueryPacket("unknown.internal");
        var response = server.HandleDnsQuery(query);

        Assert.NotNull(response);
        // Flags: 0x8183 = NXDOMAIN
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x83, response[3]);
        // Answer count = 0
        Assert.Equal(0x00, response[7]);
    }

    [Fact]
    public async Task HandleDnsQuery_ReturnsEmptyNoError_ForAaaaQuery()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));

        var query = BuildQueryPacket("bocchi.internal", qtype: 28); // 28 = AAAA
        var response = server.HandleDnsQuery(query);

        Assert.NotNull(response);
        // Flags: 0x8180 = NOERROR
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
        // Answer count = 0
        Assert.Equal(0x00, response[7]);
    }

    private static byte[] BuildQueryPacket(string domain, ushort qtype = 1)
    {
        var buffer = new List<byte>
        {
            0x12, 0x34, // Transaction ID
            0x01, 0x00, // Flags: Standard query
            0x00, 0x01, // Questions: 1
            0x00, 0x00, // Answer RRs: 0
            0x00, 0x00, // Authority RRs: 0
            0x00, 0x00  // Additional RRs: 0
        };

        var labels = domain.Split('.');
        foreach (var label in labels)
        {
            buffer.Add((byte)label.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(label));
        }
        buffer.Add(0x00); // End of name

        buffer.Add((byte)(qtype >> 8));
        buffer.Add((byte)(qtype & 0xFF)); // QTYPE

        buffer.Add(0x00);
        buffer.Add(0x01); // QCLASS: IN (0x0001)

        return [.. buffer];
    }
}
