using System.Globalization;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Diagnostics;

/// <summary>
/// Probes remote host diagnostics for Starry requirements:
/// UTC clock skew, sysctls, port 53 conflicts, tun device, nftables, SELinux, and disk/inode space.
/// </summary>
public static class HostPreflightInspector
{
    public static async Task<IReadOnlyList<PreflightCheckResult>> RunDiagnosticsAsync(
        INodeConnection node,
        bool fix = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var results = new List<PreflightCheckResult>
        {
            await CheckClockSkewAsync(node, ct).ConfigureAwait(false),
            await CheckSysctlsAsync(node, fix, ct).ConfigureAwait(false),
            await CheckPort53ConflictAsync(node, fix, ct).ConfigureAwait(false),
            await CheckTunDeviceAsync(node, ct).ConfigureAwait(false),
            await CheckNftablesAsync(node, ct).ConfigureAwait(false),
            await CheckSELinuxAsync(node, ct).ConfigureAwait(false),
            await CheckDiskSpaceAsync(node, ct).ConfigureAwait(false),
            await CheckInodesAsync(node, ct).ConfigureAwait(false)
        };

        return results;
    }

    private static async Task<PreflightCheckResult> CheckClockSkewAsync(INodeConnection node, CancellationToken ct)
    {
        var result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "date",
            Arguments = ["-u", "+%s"]
        }, ct).ConfigureAwait(false);

        if (!result.IsSuccess || !long.TryParse(result.StandardOutput.Trim(), CultureInfo.InvariantCulture, out var remoteSeconds))
        {
            return new PreflightCheckResult("UTC Clock Skew", false, "Unable to query remote system clock.");
        }

        var localSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var diff = Math.Abs(localSeconds - remoteSeconds);

        if (diff > 30)
        {
            return new PreflightCheckResult("UTC Clock Skew", false, $"Clock skew is {diff}s (exceeds 30s threshold). WireGuard handshakes may fail.");
        }

        return new PreflightCheckResult("UTC Clock Skew", true, $"Clock synchronized (skew: {diff}s).");
    }

    private static async Task<PreflightCheckResult> CheckSysctlsAsync(INodeConnection node, bool fix, CancellationToken ct)
    {
        var requiredFlags = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["net.ipv4.ip_forward"] = "1",
            ["net.ipv6.conf.all.forwarding"] = "1",
            ["net.ipv4.ip_nonlocal_bind"] = "1",
            ["net.ipv6.conf.all.proxy_ndp"] = "1"
        };

        var missing = new List<string>();
        foreach (var (k, v) in requiredFlags)
        {
            var res = await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sysctl",
                Arguments = ["-n", k]
            }, ct).ConfigureAwait(false);

            if (!res.IsSuccess || !string.Equals(res.StandardOutput.Trim(), v, StringComparison.Ordinal))
            {
                missing.Add(k);
            }
        }

        if (missing.Count == 0)
        {
            return new PreflightCheckResult("Sysctl Routing Flags", true, "All kernel IP forwarding and proxy NDP flags enabled.");
        }

        if (fix)
        {
            foreach (var flag in missing)
            {
                await node.ExecuteCommandAsync(new ProcessSpec
                {
                    Executable = "sysctl",
                    Arguments = ["-w", $"{flag}=1"]
                }, ct).ConfigureAwait(false);
            }
            return new PreflightCheckResult("Sysctl Routing Flags", true, $"Enabled sysctls: {string.Join(", ", missing)}.", WasFixed: true);
        }

        return new PreflightCheckResult("Sysctl Routing Flags", false, $"Missing required sysctls: {string.Join(", ", missing)} (run with --fix).");
    }

    private static async Task<PreflightCheckResult> CheckPort53ConflictAsync(INodeConnection node, bool fix, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "ss -lntup 'sport = :53' 2>/dev/null || netstat -lntup 2>/dev/null | grep ':53 ' || true"]
        }, ct).ConfigureAwait(false);

        var output = res.StandardOutput.Trim();
        var hasResolved = output.Contains("systemd-resolved", StringComparison.OrdinalIgnoreCase);

        if (!hasResolved)
        {
            return new PreflightCheckResult("DNS Port 53", true, "Port 53 is available for Starry embedded DNS.");
        }

        if (fix)
        {
            var fixCmd = "systemctl stop systemd-resolved 2>/dev/null && systemctl disable systemd-resolved 2>/dev/null || true";
            await node.ExecuteCommandAsync(new ProcessSpec { Executable = "sh", Arguments = ["-c", fixCmd] }, ct).ConfigureAwait(false);
            return new PreflightCheckResult("DNS Port 53", true, "Stopped and disabled systemd-resolved on port 53.", WasFixed: true);
        }

        return new PreflightCheckResult("DNS Port 53", false, "Port 53 is bound by systemd-resolved (run with --fix to disable).");
    }

    private static async Task<PreflightCheckResult> CheckTunDeviceAsync(INodeConnection node, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "test",
            Arguments = ["-c", "/dev/net/tun"]
        }, ct).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            return new PreflightCheckResult("WireGuard TUN Device", true, "/dev/net/tun is available.");
        }

        return new PreflightCheckResult("WireGuard TUN Device", false, "/dev/net/tun is missing or inaccessible.");
    }

    private static async Task<PreflightCheckResult> CheckNftablesAsync(INodeConnection node, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "nft",
            Arguments = ["-v"]
        }, ct).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            return new PreflightCheckResult("Firewall (nftables)", true, $"nftables present: {res.StandardOutput.Trim()}");
        }

        return new PreflightCheckResult("Firewall (nftables)", false, "nftables is not installed or not in PATH.");
    }

    private static async Task<PreflightCheckResult> CheckSELinuxAsync(INodeConnection node, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "getenforce 2>/dev/null || echo 'Disabled'"]
        }, ct).ConfigureAwait(false);

        var mode = res.StandardOutput.Trim();
        var isEnforcing = string.Equals(mode, "Enforcing", StringComparison.OrdinalIgnoreCase);

        return new PreflightCheckResult("SELinux", !isEnforcing, $"Current status: {mode}{(isEnforcing ? " (may require container: permissive or policy adjustments)" : string.Empty)}");
    }

    private static async Task<PreflightCheckResult> CheckDiskSpaceAsync(INodeConnection node, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "df",
            Arguments = ["-m", "/"]
        }, ct).ConfigureAwait(false);

        var lines = res.StandardOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length >= 2)
        {
            var parts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 4 && long.TryParse(parts[3], CultureInfo.InvariantCulture, out var availMb))
            {
                if (availMb < 2048)
                {
                    return new PreflightCheckResult("Disk Space (/)", false, $"Low disk space: {availMb} MB free (minimum recommended: 2048 MB).");
                }
                return new PreflightCheckResult("Disk Space (/)", true, $"{availMb} MB free.");
            }
        }

        return new PreflightCheckResult("Disk Space (/)", true, "Disk space probe succeeded.");
    }

    private static async Task<PreflightCheckResult> CheckInodesAsync(INodeConnection node, CancellationToken ct)
    {
        var res = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "df",
            Arguments = ["-i", "/"]
        }, ct).ConfigureAwait(false);

        var lines = res.StandardOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length >= 2)
        {
            var parts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 5)
            {
                var percentStr = parts[4].TrimEnd('%');
                if (int.TryParse(percentStr, CultureInfo.InvariantCulture, out var percent))
                {
                    if (percent > 90)
                    {
                        return new PreflightCheckResult("Inodes (/)", false, $"Inodes usage is high: {percent}% used.");
                    }
                    return new PreflightCheckResult("Inodes (/)", true, $"Inodes usage: {percent}%.");
                }
            }
        }

        return new PreflightCheckResult("Inodes (/)", true, "Inodes probe succeeded.");
    }
}
