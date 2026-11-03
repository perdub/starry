using System.Net;
using System.Text;
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class InternalDnsServerScopedTests
{
    [Fact]
    public async Task InternalDnsServer_SharedNetwork_ReturnsAaaaRecord()
    {
        await using var server = new InternalDnsServer(port: 5355);

        // Service DB on subnet fd67:b00b:0001:0001::/64 in network "backend_net"
        var dbIp = IPAddress.Parse("fd67:b00b:0001:0001::1");
        server.RegisterService(
            serviceName: "db",
            endpoints: [new ReplicaEndpoint("db-1", dbIp)],
            networks: ["backend_net"]);

        // Service API on subnet fd67:b00b:0001:0002::/64 in network "backend_net"
        var apiIp = IPAddress.Parse("fd67:b00b:0001:0002::1");
        server.RegisterService(
            serviceName: "api",
            endpoints: [new ReplicaEndpoint("api-1", apiIp)],
            networks: ["backend_net"]);

        // Query for db.internal from API container (caller IP = apiIp) for AAAA (qtype: 28)
        var queryPacket = BuildQueryPacket("db.internal", qtype: 28);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: apiIp);

        Assert.NotNull(response);
        // Header flags: 0x8180 = NOERROR
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
        // ANCOUNT = 1
        Assert.Equal(0x01, response[7]);

        // Response contains dbIp bytes
        var answerBytes = response[^16..];
        Assert.Equal(dbIp.GetAddressBytes(), answerBytes);
    }

    [Fact]
    public async Task InternalDnsServer_IsolatedNetwork_ReturnsNxDomain()
    {
        await using var server = new InternalDnsServer(port: 5356);

        // Service DB in "db_private_net"
        var dbIp = IPAddress.Parse("fd67:b00b:0001:0001::1");
        server.RegisterService(
            serviceName: "secret-db",
            endpoints: [new ReplicaEndpoint("secret-db-1", dbIp)],
            networks: ["db_private_net"]);

        // Service Frontend in "public_net" (NO shared network with secret-db!)
        var frontIp = IPAddress.Parse("fd67:b00b:0001:0003::1");
        server.RegisterService(
            serviceName: "frontend",
            endpoints: [new ReplicaEndpoint("frontend-1", frontIp)],
            networks: ["public_net"]);

        var queryPacket = BuildQueryPacket("secret-db.internal", qtype: 28);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: frontIp);

        Assert.NotNull(response);
        // Header flags: 0x8183 = NXDOMAIN (0x83)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x83, response[3]);
        // ANCOUNT = 0
        Assert.Equal(0x00, response[7]);
    }

    [Fact]
    public async Task InternalDnsServer_EgressFalse_ReturnsRefusedForExternalDomains()
    {
        await using var server = new InternalDnsServer(port: 5357);

        // Container with egress: false
        var isolatedIp = IPAddress.Parse("fd67:b00b:0001:0004::1");
        server.RegisterService(
            serviceName: "isolated-worker",
            endpoints: [new ReplicaEndpoint("isolated-worker-1", isolatedIp)],
            networks: ["worker_net"],
            egressAllowed: false);

        var queryPacket = BuildQueryPacket("github.com", qtype: 1);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: isolatedIp);

        Assert.NotNull(response);
        // Header flags: 0x8185 = REFUSED (0x85)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x85, response[3]);
    }

    [Fact]
    public async Task InternalDnsServer_DualStackNoIpv4_ReturnsEmptyNoError()
    {
        await using var server = new InternalDnsServer(port: 5358);

        var ipv6Only = IPAddress.Parse("fd67:b00b:0001:0005::1");
        server.RegisterService(
            serviceName: "v6-service",
            endpoints: [new ReplicaEndpoint("v6-service-1", ipv6Only, Ipv4Address: null)],
            networks: ["net1"]);

        var callerIp = IPAddress.Parse("fd67:b00b:0001:0006::1");
        server.RegisterService(
            serviceName: "caller",
            endpoints: [new ReplicaEndpoint("caller-1", callerIp)],
            networks: ["net1"]);

        // Query for A record (IPv4) when only IPv6 exists
        var queryPacket = BuildQueryPacket("v6-service.internal", qtype: 1);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: callerIp);

        Assert.NotNull(response);
        // NOERROR (0x8180) with ANCOUNT = 0
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
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
