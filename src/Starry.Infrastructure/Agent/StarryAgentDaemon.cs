using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Starry.Core.Backups;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Agent;

/// <summary>
/// Autonomous host agent daemon managing local WireGuard health, embedded DNS VIP resolution,
/// continuous container drift reconciliation, and volume backup scheduling.
/// </summary>
public sealed class StarryAgentDaemon : IAsyncDisposable
{
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Standard fixed systemd service path on Linux")]
    public const string SystemdServicePath = "/etc/systemd/system/starry-agent.service";

    public const string SystemdServiceContent =
        """
        [Unit]
        Description=Starry Cluster Agent Daemon
        After=network.target docker.service podman.service

        [Service]
        Type=simple
        ExecStart=/usr/local/bin/starry agent run
        Restart=always
        RestartSec=5
        LimitNOFILE=65536

        [Install]
        WantedBy=multi-user.target
        """;

    private readonly IContainerRuntime _runtime;
    private readonly INodeConnection _nodeConnection;
    private readonly bool _ownsNodeConnection;
    private readonly IProcessRunner _processRunner;
    private readonly IBackupStorageProvider? _backupStorageProvider;
    private readonly Action<string>? _logger;
    private readonly ConcurrentQueue<DriftEvent> _driftHistory = new();
    private InternalDnsServer? _activeDnsServer;

    public event EventHandler<DriftEventArgs>? DriftDetected;

    public IReadOnlyList<DriftEvent> RecentDriftEvents => _driftHistory.ToArray();

    public InternalDnsServer? ActiveDnsServer => _activeDnsServer;

    public StarryAgentDaemon(
        IContainerRuntime? runtime = null,
        INodeConnection? nodeConnection = null,
        IProcessRunner? processRunner = null,
        IBackupStorageProvider? backupStorageProvider = null,
        Action<string>? logger = null)
    {
        _logger = logger;
        _processRunner = processRunner ?? new ProcessRunner();

        if (nodeConnection != null)
        {
            _nodeConnection = nodeConnection;
            _ownsNodeConnection = false;
        }
        else
        {
            _nodeConnection = new LocalNodeConnection("local", _processRunner);
            _ownsNodeConnection = true;
        }

        _runtime = runtime ?? DetectDefaultRuntime();
        _backupStorageProvider = backupStorageProvider;
    }

    /// <summary>
    /// Starts the agent daemon lifecycle:
    /// - Starts embedded InternalDnsServer on port 53 (or 5353) serving *.internal VIP records.
    /// - Starts background health monitoring &amp; drift detection loop (every 30 seconds).
    /// - Starts periodic volume streaming backup scheduler.
    /// </summary>
    public async Task RunAsync(AgentOptions options, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        _logger?.Invoke("Starting StarryAgentDaemon...");
        await StartDnsServerAsync(options, ct).ConfigureAwait(false);

        try
        {
            var healthTask = RunHealthMonitoringLoopAsync(options, ct);
            var backupTask = RunBackupSchedulerAsync(options.BackupPolicies, ct);

            await Task.WhenAll(healthTask, backupTask).ConfigureAwait(false);
        }
        finally
        {
            _logger?.Invoke("Stopping StarryAgentDaemon...");
            if (_activeDnsServer != null)
            {
                await _activeDnsServer.DisposeAsync().ConfigureAwait(false);
                _activeDnsServer = null;
            }
        }
    }

    /// <summary>
    /// Installs the starry-agent systemd unit file and enables the service.
    /// </summary>
    public async Task InstallSystemdServiceAsync(CancellationToken ct = default)
    {
        await InstallSystemdServiceAsync(SystemdServicePath, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Installs the starry-agent systemd unit file to a target destination and enables the service.
    /// </summary>
    public async Task InstallSystemdServiceAsync(string? targetServicePath, CancellationToken ct = default)
    {
        var servicePath = string.IsNullOrWhiteSpace(targetServicePath) ? SystemdServicePath : targetServicePath;
        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Generating systemd unit file at '{servicePath}'..."));

        var unitText = SystemdServiceContent.ReplaceLineEndings("\n").TrimEnd() + "\n";
        var writeSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"mkdir -p $(dirname '{servicePath}') && cat > '{servicePath}' && chmod 644 '{servicePath}'"],
            StandardInputBytes = Encoding.UTF8.GetBytes(unitText),
            Timeout = TimeSpan.FromSeconds(30)
        };

        var writeResult = await _nodeConnection.ExecuteCommandAsync(writeSpec, ct).ConfigureAwait(false);
        if (!writeResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to write systemd service unit to '{servicePath}': {writeResult.StandardError}");
        }

        _logger?.Invoke("Reloading systemd daemon and enabling starry-agent.service...");

        var reloadSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "systemctl daemon-reload && systemctl enable --now starry-agent.service"],
            Timeout = TimeSpan.FromSeconds(60)
        };

        var reloadResult = await _nodeConnection.ExecuteCommandAsync(reloadSpec, ct).ConfigureAwait(false);
        if (!reloadResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to enable starry-agent.service: {reloadResult.StandardError}");
        }

        _logger?.Invoke("starry-agent.service successfully installed and enabled.");
    }

    /// <summary>
    /// Gathers local node agent status including WireGuard interface state, wg dump peers, and running containers.
    /// </summary>
    public async Task<AgentStatusReport> GetStatusAsync(CancellationToken ct = default)
    {
        _logger?.Invoke("Querying agent status...");

        var isInterfaceActive = await CheckStarry0ActiveAsync(ct).ConfigureAwait(false);
        var wgDump = await QueryWireGuardDumpAsync(ct).ConfigureAwait(false);
        var peers = ParseWireGuardDump(wgDump);
        var allContainers = await QueryContainersAsync(ct).ConfigureAwait(false);
        var runningContainers = allContainers.Where(static c => c.IsRunning).ToList();

        return new AgentStatusReport(
            IsStarry0Active: isInterfaceActive,
            WireGuardDump: wgDump,
            Peers: peers,
            Containers: allContainers,
            RunningContainers: runningContainers,
            RecentDriftEvents: _driftHistory.ToArray(),
            TimestampUtc: DateTime.UtcNow);
    }

    /// <summary>
    /// Executes a single health monitoring and reconciliation cycle.
    /// Detects stopped/exited managed containers and restarts them via Docker/Podman socket API.
    /// </summary>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Loop must continue if container listing fails")]
    public async Task<int> PerformHealthCheckAndReconcileAsync(CancellationToken ct = default)
    {
        _logger?.Invoke("Performing container drift detection...");

        IReadOnlyList<ContainerState> containers;
        try
        {
            containers = await _runtime.ListContainersAsync(_nodeConnection, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Warning: Unable to list containers during drift check: {ex.Message}"));
            return 0;
        }

        var restartedCount = 0;
        foreach (var container in containers)
        {
            if (await ReconcileSingleContainerAsync(container, ct).ConfigureAwait(false))
            {
                restartedCount++;
            }
        }

        return restartedCount;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Daemon must record failed restarts without throwing")]
    private async Task<bool> ReconcileSingleContainerAsync(ContainerState container, CancellationToken ct)
    {
        if (!IsManagedContainer(container))
        {
            return false;
        }

        var isStoppedOrExited = !container.IsRunning ||
            container.Status.Contains("exited", StringComparison.OrdinalIgnoreCase) ||
            container.Status.Contains("stopped", StringComparison.OrdinalIgnoreCase) ||
            container.Status.Contains("dead", StringComparison.OrdinalIgnoreCase);

        if (!isStoppedOrExited)
        {
            return false;
        }

        var reason = string.Create(CultureInfo.InvariantCulture, $"Managed container '{container.Name}' ({container.Id}) is not running. Status='{container.Status}', ExitCode={container.ExitCode}.");
        _logger?.Invoke(reason);

        var restartSuccess = false;
        string? details = null;

        try
        {
            restartSuccess = await RestartContainerAsync(container.Id, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            details = ex.Message;
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Failed to restart container '{container.Name}': {ex.Message}"));
        }

        RecordDriftEvent(new DriftEvent(
            ContainerId: container.Id,
            ContainerName: container.Name,
            Reason: reason,
            TimestampUtc: DateTime.UtcNow,
            RestartTriggered: true,
            RestartSucceeded: restartSuccess,
            Details: details));

        return restartSuccess;
    }

    /// <summary>
    /// Restarts a container via socket HTTP POST /v1.40/containers/{id}/start.
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Standard fixed endpoint for Docker/Podman Unix domain socket")]
    public async Task<bool> RestartContainerAsync(string containerId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerId);

        var socketPath = await ResolveSocketPathAsync(ct).ConfigureAwait(false);
        using var client = await _nodeConnection.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.PostAsync(
            new Uri(new Uri("http://127.0.0.1/"), $"/v1.40/containers/{containerId}/start"),
            content: null,
            ct).ConfigureAwait(false);

        if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Successfully started container '{containerId}'."));
            return true;
        }

        var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        throw new InvalidOperationException($"Failed to restart container '{containerId}' via Docker API: {response.StatusCode} {err}");
    }

    /// <summary>
    /// Executes a streaming volume backup for the given policy.
    /// </summary>
    public async Task<string> ExecuteBackupAsync(BackupPolicy policy, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(policy);

        var prefix = string.IsNullOrWhiteSpace(policy.BackupPrefix) ? policy.ServiceName : policy.BackupPrefix;
        var backupId = string.Create(CultureInfo.InvariantCulture, $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}");

        var provider = _backupStorageProvider;
        if (provider == null && !string.IsNullOrWhiteSpace(policy.RemoteDestination))
        {
            provider = new RcloneBackupStorageProvider(policy.RemoteDestination);
        }

        if (provider == null)
        {
            throw new InvalidOperationException($"No backup storage provider configured for service '{policy.ServiceName}'");
        }

        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Starting streaming backup for volume '{policy.VolumePath}' (ID: {backupId})..."));
        await provider.StreamBackupAsync(_nodeConnection, policy.VolumePath, backupId, ct).ConfigureAwait(false);
        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Completed streaming backup for volume '{policy.VolumePath}' (ID: {backupId})."));

        return backupId;
    }

    public void RegisterDnsRecord(string domain, IPAddress address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentNullException.ThrowIfNull(address);

        _activeDnsServer?.RegisterRecord(domain, address);
    }

    public async ValueTask DisposeAsync()
    {
        if (_activeDnsServer != null)
        {
            await _activeDnsServer.DisposeAsync().ConfigureAwait(false);
            _activeDnsServer = null;
        }

        if (_ownsNodeConnection)
        {
            await _nodeConnection.DisposeAsync().ConfigureAwait(false);
        }

        GC.SuppressFinalize(this);
    }

    private async Task StartDnsServerAsync(AgentOptions options, CancellationToken ct)
    {
        var port = options.DnsPort > 0 ? options.DnsPort : 53;
        InternalDnsServer dnsServer;

        try
        {
            dnsServer = new InternalDnsServer(port);
            RegisterRecords(dnsServer, options.InternalDnsRecords);
            await dnsServer.StartAsync(options.DnsBindAddress, ct).ConfigureAwait(false);
            _activeDnsServer = dnsServer;
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Internal DNS server listening on port {port}."));
        }
        catch (SocketException) when (port == 53)
        {
            _logger?.Invoke("Binding port 53 failed. Falling back to port 5353 for embedded DNS server...");
            dnsServer = new InternalDnsServer(5353);
            RegisterRecords(dnsServer, options.InternalDnsRecords);
            await dnsServer.StartAsync(options.DnsBindAddress, ct).ConfigureAwait(false);
            _activeDnsServer = dnsServer;
            _logger?.Invoke("Internal DNS server listening on fallback port 5353.");
        }
    }

    private static void RegisterRecords(InternalDnsServer server, IReadOnlyDictionary<string, string>? records)
    {
        if (records == null)
        {
            return;
        }

        foreach (var (domain, ipStr) in records)
        {
            if (IPAddress.TryParse(ipStr, out var ip))
            {
                server.RegisterRecord(domain, ip);
                if (!domain.EndsWith(".internal", StringComparison.OrdinalIgnoreCase) && !domain.Contains('.', StringComparison.Ordinal))
                {
                    server.RegisterRecord($"{domain}.internal", ip);
                }
            }
        }
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Background loop must survive intermittent errors")]
    private async Task RunHealthMonitoringLoopAsync(AgentOptions options, CancellationToken ct)
    {
        var interval = options.HealthCheckInterval > TimeSpan.Zero
            ? options.HealthCheckInterval
            : TimeSpan.FromSeconds(30);

        using var timer = new PeriodicTimer(interval);

        try
        {
            await PerformHealthCheckAndReconcileAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return;
        }
        catch (Exception ex)
        {
            RecordDriftEvent(new DriftEvent(
                ContainerId: string.Empty,
                ContainerName: "system",
                Reason: $"Initial health check cycle failed: {ex.Message}",
                TimestampUtc: DateTime.UtcNow,
                RestartTriggered: false,
                RestartSucceeded: false,
                Details: ex.ToString()));
        }

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var hasTicked = await timer.WaitForNextTickAsync(ct).ConfigureAwait(false);
                if (!hasTicked)
                {
                    break;
                }

                await PerformHealthCheckAndReconcileAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                RecordDriftEvent(new DriftEvent(
                    ContainerId: string.Empty,
                    ContainerName: "system",
                    Reason: $"Health check cycle failed: {ex.Message}",
                    TimestampUtc: DateTime.UtcNow,
                    RestartTriggered: false,
                    RestartSucceeded: false,
                    Details: ex.ToString()));
            }
        }
    }

    private async Task RunBackupSchedulerAsync(IReadOnlyList<BackupPolicy>? policies, CancellationToken ct)
    {
        if (policies == null || policies.Count == 0)
        {
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Graceful cancellation on daemon shutdown
            }
            return;
        }

        var tasks = policies.Select(p => RunSinglePolicySchedulerAsync(p, ct)).ToList();
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Policy backup failure should be logged and not crash the scheduler")]
    private async Task RunSinglePolicySchedulerAsync(BackupPolicy policy, CancellationToken ct)
    {
        var interval = policy.Interval > TimeSpan.Zero
            ? policy.Interval
            : TimeSpan.FromHours(24);

        using var timer = new PeriodicTimer(interval);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var hasTicked = await timer.WaitForNextTickAsync(ct).ConfigureAwait(false);
                if (!hasTicked)
                {
                    break;
                }

                await ExecuteBackupAsync(policy, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Backup failed for policy '{policy.ServiceName}': {ex.Message}"));
            }
        }
    }

    private void RecordDriftEvent(DriftEvent drift)
    {
        _driftHistory.Enqueue(drift);
        while (_driftHistory.Count > 100)
        {
            _driftHistory.TryDequeue(out _);
        }

        DriftDetected?.Invoke(this, new DriftEventArgs(drift));
    }

    private static bool IsManagedContainer(ContainerState container)
    {
        ArgumentNullException.ThrowIfNull(container);

        if (container.Labels.TryGetValue("managed-by", out var managedBy) &&
            string.Equals(managedBy, "starry", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return container.Labels.ContainsKey("starry.priority") ||
               container.Labels.ContainsKey("starry.service");
    }

    private async Task<string> ResolveSocketPathAsync(CancellationToken ct)
    {
        if (_runtime is BaseSocketContainerRuntime socketRuntime)
        {
            return await socketRuntime.ResolveSocketPathAsync(_nodeConnection, ct).ConfigureAwait(false);
        }

        string[] candidateSockets = ["/var/run/docker.sock", "/run/docker.sock", "/run/podman/podman.sock", "/var/run/podman/podman.sock"];
        foreach (var candidate in candidateSockets)
        {
            var check = await _nodeConnection.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-S", candidate]
            }, ct).ConfigureAwait(false);

            if (check.IsSuccess)
            {
                return candidate;
            }
        }

        return candidateSockets[0];
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Fallback if network interface enumeration is restricted")]
    private async Task<bool> CheckStarry0ActiveAsync(CancellationToken ct)
    {
        var linkSpec = new ProcessSpec
        {
            Executable = "ip",
            Arguments = ["link", "show", "starry0"]
        };

        var linkResult = await _nodeConnection.ExecuteCommandAsync(linkSpec, ct).ConfigureAwait(false);
        if (linkResult.IsSuccess)
        {
            var output = linkResult.StandardOutput;
            if (output.Contains("state UP", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("<UP", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("state UNKNOWN", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        try
        {
            var iface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(static i => string.Equals(i.Name, "starry0", StringComparison.OrdinalIgnoreCase));
            if (iface != null && iface.OperationalStatus == OperationalStatus.Up)
            {
                return true;
            }
        }
        catch
        {
            // Ignore failure querying network interfaces
        }

        return false;
    }

    private async Task<string?> QueryWireGuardDumpAsync(CancellationToken ct)
    {
        var wgSpec = new ProcessSpec
        {
            Executable = "wg",
            Arguments = ["show", "starry0", "dump"]
        };

        var wgResult = await _nodeConnection.ExecuteCommandAsync(wgSpec, ct).ConfigureAwait(false);
        return wgResult.IsSuccess ? wgResult.StandardOutput.TrimEnd() : null;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Status query should remain resilient even if container runtime is unavailable")]
    private async Task<IReadOnlyList<ContainerState>> QueryContainersAsync(CancellationToken ct)
    {
        try
        {
            return await _runtime.ListContainersAsync(_nodeConnection, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Warning: Unable to list containers: {ex.Message}"));
            return [];
        }
    }

    public static IReadOnlyList<WireGuardPeerDump> ParseWireGuardDump(string? dump)
    {
        if (string.IsNullOrWhiteSpace(dump))
        {
            return [];
        }

        var lines = dump.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
        {
            return [];
        }

        var peers = new List<WireGuardPeerDump>();
        for (var i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split('\t');
            if (parts.Length >= 8)
            {
                peers.Add(new WireGuardPeerDump(
                    PublicKey: parts[0],
                    PresharedKey: parts[1],
                    Endpoint: parts[2],
                    AllowedIps: parts[3],
                    LatestHandshake: long.TryParse(parts[4], CultureInfo.InvariantCulture, out var hs) ? hs : 0,
                    TransferRx: long.TryParse(parts[5], CultureInfo.InvariantCulture, out var rx) ? rx : 0,
                    TransferTx: long.TryParse(parts[6], CultureInfo.InvariantCulture, out var tx) ? tx : 0,
                    PersistentKeepalive: int.TryParse(parts[7], CultureInfo.InvariantCulture, out var ka) ? ka : 0));
            }
        }

        return peers;
    }

    private static IContainerRuntime DetectDefaultRuntime()
    {
        if (File.Exists("/run/podman/podman.sock") || File.Exists("/var/run/podman/podman.sock"))
        {
            return new PodmanRuntime();
        }

        return new DockerRuntime();
    }
}
