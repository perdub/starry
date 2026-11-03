using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class IdLedgerServiceTests
{
    [Fact]
    public void AllocateHostId_IncrementsMonotonically()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_ledger_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            var id1 = ledger.AllocateHostId("germany");
            var id2 = ledger.AllocateHostId("finland");
            var id3 = ledger.AllocateHostId("japan");

            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);

            // Re-querying same host returns existing ID
            Assert.Equal(1, ledger.AllocateHostId("germany"));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ReleaseHost_TombstonesId_AndNeverReusesIt()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_tombstone_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            ledger.AllocateHostId("germany"); // 1
            ledger.AllocateHostId("finland"); // 2

            // Release germany (ID 1 should be tombstoned)
            ledger.ReleaseHost("germany");

            // Allocate brand new host -> must receive 3, NOT tombstoned 1!
            var id3 = ledger.AllocateHostId("singapore");
            Assert.Equal(3, id3);

            var snapshot = ledger.StateSnapshot;
            Assert.Contains(1, snapshot.TombstonedHostIds);
            Assert.DoesNotContain(1, snapshot.AllocatedHosts.Values);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ReleaseContainer_TombstonesContainerId_AndNeverReusesIt()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_cid_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            ledger.AllocateContainerId("shop#1"); // 1
            ledger.AllocateContainerId("shop#2"); // 2

            ledger.ReleaseContainer("shop#1");

            var c3 = ledger.AllocateContainerId("shop#3");
            Assert.Equal(3, c3);

            var snapshot = ledger.StateSnapshot;
            Assert.Contains(1, snapshot.TombstonedContainerIds);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void AddressFormatting_FormatsCanonicalIPv6Subnets()
    {
        Assert.Equal("fd67:b00b:0001::1", IdLedgerService.FormatHostWireGuardIp(1));
        Assert.Equal("fd67:b00b:0001::/48", IdLedgerService.FormatHostSubnet(1));
        Assert.Equal("fd67:b00b:0001:000a::/64", IdLedgerService.FormatContainerSubnet(1, 10));
        Assert.Equal("fd67:b00b:0001:000a::1", IdLedgerService.FormatContainerIp(1, 10));
        Assert.Equal("fd67:b00b:0001:ffff::1", IdLedgerService.FormatIngressIp(1));
    }
}
