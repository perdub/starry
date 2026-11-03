using System.Security.Cryptography;
using Starry.Core.Processes;
using Starry.Infrastructure.Nodes;
using Xunit;

namespace Starry.E2ETests;

public class LiveMultiNodeSshClusterE2ETests
{
    [Fact]
    public async Task LiveCluster_RealSshConnection_DetectsAlpineDistro()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null)
        {
            // Skip gracefully when Docker is not accessible in execution environment
            return;
        }

        Assert.Equal("ALPINE", fixture.Node1.DistroId);

        var result = await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "uname",
            Arguments = ["-a"]
        });

        Assert.True(result.IsSuccess);
        Assert.Contains("Linux", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LiveCluster_VolumeBinaryStream_BitForBitIntegrityOverRealSsh()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null || fixture.Node2 == null)
        {
            return;
        }

        // 1. Generate 64 KB of non-UTF8 binary data on Node 1
        var binaryPayload = new byte[65536];
        RandomNumberGenerator.Fill(binaryPayload);
        binaryPayload[0] = 0xFF;
        binaryPayload[1] = 0x00;
        binaryPayload[2] = 0xFE;

        var expectedSha = Convert.ToHexString(SHA256.HashData(binaryPayload));

        // Create source volume directory on Node 1
        await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "mkdir",
            Arguments = ["-p", "/var/data/vol-test"]
        });

        // Write binary payload directly via stdin stream
        await fixture.Node1.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "cat > /var/data/vol-test/payload.bin"] },
            stdinStream: new MemoryStream(binaryPayload),
            stdoutStream: null);

        // Verify SHA256 on Node 1 over SSH
        var sha1Res = await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sha256sum",
            Arguments = ["/var/data/vol-test/payload.bin"]
        });
        Assert.Contains(expectedSha, sha1Res.StandardOutput, StringComparison.OrdinalIgnoreCase);

        // 2. Stream volume binary archive (tar --zstd) from Node 1 into orchestrator MemoryStream, then restore to Node 2
        using var pipeStream = new MemoryStream();

        var exportRes = await fixture.Node1.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "tar -cf - --zstd -C /var/data/vol-test ."] },
            stdinStream: null,
            stdoutStream: pipeStream);
        Assert.True(exportRes.IsSuccess, $"Export failed: {exportRes.ExitCode}, {exportRes.StandardError}");

        pipeStream.Position = 0;

        await fixture.Node2.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "mkdir",
            Arguments = ["-p", "/var/data/vol-test"]
        });

        var importRes = await fixture.Node2.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "tar -xf - --zstd -C /var/data/vol-test"] },
            stdinStream: pipeStream,
            stdoutStream: null);
        Assert.True(importRes.IsSuccess, $"ExitCode: {importRes.ExitCode}, Stderr: {importRes.StandardError}");

        // 3. Verify SHA256 on Node 2 matches expected hash bit-for-bit
        var sha2Res = await fixture.Node2.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sha256sum",
            Arguments = ["/var/data/vol-test/payload.bin"]
        });

        Assert.True(sha2Res.IsSuccess);
        Assert.Contains(expectedSha, sha2Res.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LiveCluster_SshKeyRotation_OnLiveContainer()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null)
        {
            return;
        }

        // Rotate SSH key on live Node 1
        var newPrivateKey = await NodeOperationsService.RotateSshKeysAsync(fixture.Node1);
        Assert.NotNull(newPrivateKey);
        Assert.Contains("BEGIN OPENSSH PRIVATE KEY", newPrivateKey, StringComparison.Ordinal);

        var host = fixture.Node1.Host;
        var port = fixture.Node1.Port;
        await fixture.Node1.DisposeAsync();

        // Verify connection with the new rotated key
        await using var newConn = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "rotated-node-1",
            Host = host,
            Port = port,
            User = "root",
            PrivateKeyContent = newPrivateKey
        });

        await newConn.InitializeAsync();
        var testRes = await newConn.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["ROTATION_VERIFIED"]
        });

        Assert.True(testRes.IsSuccess, $"testRes ExitCode: {testRes.ExitCode}, Stderr: '{testRes.StandardError}', Stdout: '{testRes.StandardOutput}'");
        Assert.Contains("ROTATION_VERIFIED", testRes.StandardOutput, StringComparison.Ordinal);
    }
}
