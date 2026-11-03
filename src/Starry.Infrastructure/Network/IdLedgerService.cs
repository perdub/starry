using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Starry.Infrastructure.Network;

public sealed record IdLedgerState
{
    public int NextHostId { get; set; } = 1;
    public Dictionary<string, int> AllocatedHosts { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public List<int> TombstonedHostIds { get; init; } = [];

    public int NextContainerId { get; set; } = 1;
    public Dictionary<string, int> AllocatedContainers { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public List<int> TombstonedContainerIds { get; init; } = [];
}

/// <summary>
/// Persistent ledger for monotonic, non-reusable host and container IDs with tombstoning,
/// generating canonical fd67:b00b::/32 cluster IPv6 addresses.
/// </summary>
public sealed class IdLedgerService
{
    public const string ClusterPrefix = "fd67:b00b";
    private readonly string _ledgerFilePath;
    private readonly object _syncLock = new();
    private IdLedgerState _state;

    public IdLedgerService(string? baseDir = null)
    {
        var resolvedDir = string.IsNullOrWhiteSpace(baseDir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(baseDir);
        var starryDir = Path.Combine(resolvedDir, ".starry");
        _ledgerFilePath = Path.Combine(starryDir, "id-ledger.json");
        _state = LoadState();
    }

    public int AllocateHostId(string hostName, int? explicitId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostName);

        lock (_syncLock)
        {
            if (_state.AllocatedHosts.TryGetValue(hostName, out var existing))
            {
                return existing;
            }

            int allocated;
            if (explicitId.HasValue)
            {
                allocated = explicitId.Value;
                if (_state.TombstonedHostIds.Contains(allocated))
                {
                    throw new InvalidOperationException($"Host ID {allocated} is tombstoned and cannot be reused.");
                }
            }
            else
            {
                while (_state.TombstonedHostIds.Contains(_state.NextHostId) ||
                       _state.AllocatedHosts.ContainsValue(_state.NextHostId))
                {
                    _state.NextHostId++;
                }

                allocated = _state.NextHostId++;
            }

            _state.AllocatedHosts[hostName] = allocated;
            SaveState();
            return allocated;
        }
    }

    public int AllocateContainerId(string containerKey, int? explicitId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerKey);

        lock (_syncLock)
        {
            if (_state.AllocatedContainers.TryGetValue(containerKey, out var existing))
            {
                return existing;
            }

            int allocated;
            if (explicitId.HasValue)
            {
                allocated = explicitId.Value;
                if (_state.TombstonedContainerIds.Contains(allocated))
                {
                    throw new InvalidOperationException($"Container ID {allocated} is tombstoned and cannot be reused.");
                }
            }
            else
            {
                while (_state.TombstonedContainerIds.Contains(_state.NextContainerId) ||
                       _state.AllocatedContainers.ContainsValue(_state.NextContainerId))
                {
                    _state.NextContainerId++;
                }

                allocated = _state.NextContainerId++;
            }

            _state.AllocatedContainers[containerKey] = allocated;
            SaveState();
            return allocated;
        }
    }

    public void ReleaseHost(string hostName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostName);

        lock (_syncLock)
        {
            if (_state.AllocatedHosts.TryGetValue(hostName, out var id))
            {
                _state.AllocatedHosts.Remove(hostName);
                if (!_state.TombstonedHostIds.Contains(id))
                {
                    _state.TombstonedHostIds.Add(id);
                }
                SaveState();
            }
        }
    }

    public void ReleaseContainer(string containerKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerKey);

        lock (_syncLock)
        {
            if (_state.AllocatedContainers.TryGetValue(containerKey, out var id))
            {
                _state.AllocatedContainers.Remove(containerKey);
                if (!_state.TombstonedContainerIds.Contains(id))
                {
                    _state.TombstonedContainerIds.Add(id);
                }
                SaveState();
            }
        }
    }

    public static string FormatHostWireGuardIp(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}::1";

    public static string FormatHostSubnet(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}::/48";

    public static string FormatContainerSubnet(int hostId, int containerId) =>
        $"{ClusterPrefix}:{hostId:x4}:{containerId:x4}::/64";

    public static string FormatContainerIp(int hostId, int containerId) =>
        $"{ClusterPrefix}:{hostId:x4}:{containerId:x4}::1";

    public static string FormatIngressIp(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}:ffff::1";

    public IdLedgerState GetStateSnapshot()
    {
        lock (_syncLock)
        {
            return new IdLedgerState
            {
                NextHostId = _state.NextHostId,
                AllocatedHosts = new Dictionary<string, int>(_state.AllocatedHosts, StringComparer.OrdinalIgnoreCase),
                TombstonedHostIds = [.. _state.TombstonedHostIds],
                NextContainerId = _state.NextContainerId,
                AllocatedContainers = new Dictionary<string, int>(_state.AllocatedContainers, StringComparer.OrdinalIgnoreCase),
                TombstonedContainerIds = [.. _state.TombstonedContainerIds]
            };
        }
    }

    private IdLedgerState LoadState()
    {
        if (!File.Exists(_ledgerFilePath))
        {
            return new IdLedgerState();
        }

        try
        {
            var json = File.ReadAllText(_ledgerFilePath);
            var loaded = JsonSerializer.Deserialize<IdLedgerState>(json);
            return loaded ?? new IdLedgerState();
        }
        catch
        {
            return new IdLedgerState();
        }
    }

    private void SaveState()
    {
        var dir = Path.GetDirectoryName(_ledgerFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(_state, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_ledgerFilePath, json);
    }
}
