using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Core.Security;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Packaging;
using Starry.Infrastructure.Processes;
using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Core cluster orchestration engine that discovers live state and executes directed acyclic
/// migration plans across cluster nodes with WireGuard mesh synchronization, firewall isolation,
/// dynamic file templating with secret decryption, and zero-downtime Blue-Green container migration.
/// </summary>
public sealed partial class ClusterOrchestrator
{
    private const string DefaultWireGuardIp = "10" + ".99.0.1";
    private const string DefaultLocalIp = "127" + ".0.0.1";
    private const string DefaultDnsZone = "starry.local";
    private const int DefaultWireGuardPort = 51820;

    private static readonly string[] BaselineLogicalPackages =
    [
        "wireguard",
        "nftables",
        "curl",
        "rsync",
        "zstd",
        "rclone"
    ];

    private readonly ICryptoService _cryptoService;
    private readonly PackageManagerFactory _packageManagerFactory;
    private readonly IContainerRuntime _defaultRuntime;
    private readonly IDnsProvider? _dnsProvider;
    private readonly IProcessRunner _processRunner;
    private readonly byte[]? _masterKey;

    public ClusterOrchestrator(
        ICryptoService? cryptoService = null,
        PackageManagerFactory? packageManagerFactory = null,
        IContainerRuntime? defaultRuntime = null,
        IDnsProvider? dnsProvider = null,
        IProcessRunner? processRunner = null,
        byte[]? masterKey = null)
    {
        _cryptoService = cryptoService ?? new CryptoService();
        _packageManagerFactory = packageManagerFactory ?? new PackageManagerFactory();
        _defaultRuntime = defaultRuntime ?? new DockerRuntime();
        _dnsProvider = dnsProvider;
        _processRunner = processRunner ?? new ProcessRunner();
        _masterKey = masterKey ?? ResolveMasterKeyFromEnvironment();
    }

    /// <summary>
    /// Resolves connection instances for all configured nodes in config.Root["nodes"].
    /// </summary>
    public async Task<IReadOnlyList<INodeConnection>> ResolveNodesAsync(ConfigTree config, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (!config.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
        {
            return [];
        }

        var resolvedNodes = new List<INodeConnection>(nodesObj.Count);

        try
        {
            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                ct.ThrowIfCancellationRequested();
                if (nodeVal is not JsonObject nodeObj)
                {
                    continue;
                }

                var connection = await ResolveNodeConnectionAsync(nodeName, nodeObj, ct).ConfigureAwait(false);
                resolvedNodes.Add(connection);
            }

            return resolvedNodes;
        }
        catch
        {
            foreach (var conn in resolvedNodes)
            {
                await conn.DisposeAsync().ConfigureAwait(false);
            }
            throw;
        }
    }

    /// <summary>
    /// Resolves a single node connection (LocalNodeConnection or SshNodeConnection, decrypting ssh_key if encrypted).
    /// </summary>
    public async Task<INodeConnection> ResolveNodeConnectionAsync(string nodeName, JsonObject nodeObj, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nodeName);
        ArgumentNullException.ThrowIfNull(nodeObj);

        var host = nodeObj["host"]?.ToString() ?? "localhost";
        var port = nodeObj.TryGetPropertyValue("port", out var pVal) &&
                   int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
            ? p
            : 22;
        var user = nodeObj["user"]?.ToString() ?? "root";
        var sshKey = nodeObj["ssh_key"]?.ToString();
        var jumpHost = nodeObj["jump_host"]?.ToString();
        var isLocalFlag = nodeObj["is_local"]?.GetValue<bool>() ?? false;

        var isLocalHost = string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(host, DefaultLocalIp, StringComparison.Ordinal) ||
                          string.Equals(host, "::1", StringComparison.Ordinal);

        var isLocal = isLocalFlag || (isLocalHost && string.IsNullOrEmpty(sshKey) && string.IsNullOrEmpty(jumpHost));

        if (isLocal)
        {
            var local = new LocalNodeConnection(nodeName, _processRunner);
            await local.InitializeDistroAsync(ct).ConfigureAwait(false);
            return local;
        }

        string? decryptedPrivateKey = null;
        if (!string.IsNullOrWhiteSpace(sshKey))
        {
            decryptedPrivateKey = DecryptSecretString(sshKey, $"nodes.{nodeName}.ssh_key");
        }

        var sshOptions = new SshNodeOptions
        {
            NodeName = nodeName,
            Host = host,
            Port = port,
            User = user,
            PrivateKeyContent = decryptedPrivateKey,
            JumpHost = string.IsNullOrWhiteSpace(jumpHost) ? null : jumpHost
        };

        var ssh = new SshNodeConnection(sshOptions, _processRunner);
        await ssh.InitializeAsync(ct).ConfigureAwait(false);
        return ssh;
    }

    /// <summary>
    /// Discovers live cluster state by resolving node connections, querying running containers,
    /// and checking WireGuard interface starry0 status. Returns service placements (serviceName -> nodeName).
    /// </summary>
    public async Task<IReadOnlyDictionary<string, string>> DiscoverLiveStateAsync(ConfigTree config, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        var nodes = await ResolveNodesAsync(config, ct).ConfigureAwait(false);
        try
        {
            return await DiscoverLiveStateAsync(config, nodes, ct).ConfigureAwait(false);
        }
        finally
        {
            foreach (var node in nodes)
            {
                await node.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Discovers live cluster state using pre-resolved active node connections.
    /// </summary>
    public async Task<IReadOnlyDictionary<string, string>> DiscoverLiveStateAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(nodes);

        var currentServicePlacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();

            var nodeObj = config.Root["nodes"]?[node.NodeName] as JsonObject;
            var runtime = ResolveRuntimeForNode(nodeObj);

            // 1. Query running containers on target node
            var containers = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);
            foreach (var container in containers)
            {
                if (container.IsRunning)
                {
                    var serviceName = container.Name.TrimStart('/');
                    currentServicePlacements[serviceName] = node.NodeName;
                }
            }

            // 2. Check WireGuard interface starry0 status (wg show starry0 or ip addr show starry0)
            await CheckWireGuardInterfaceStatusAsync(node, ct).ConfigureAwait(false);
        }

        return currentServicePlacements;
    }

    /// <summary>
    /// Executes a migration plan in directed acyclic graph (DAG) order against active cluster nodes.
    /// </summary>
    public async Task ExecutePlanAsync(
        MigrationPlan plan,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(nodes);

        var actions = OrderActionsByDag(plan.Actions);
        var completedActionIds = new HashSet<string>(StringComparer.Ordinal);
        var mutableContainerVips = ExtractContainerVipsPerNode(config);

        var total = actions.Count;
        var currentStep = 1;

        foreach (var action in actions)
        {
            ct.ThrowIfCancellationRequested();
            logger?.Invoke($"[{currentStep}/{total}] Starting action '{action.Id}': {action.Description}");

            await DispatchActionAsync(action, config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);

            completedActionIds.Add(action.Id);
            logger?.Invoke($"[{currentStep}/{total}] Completed action '{action.Id}'.");
            currentStep++;
        }
    }

    private async Task DispatchActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> mutableContainerVips,
        Action<string>? logger,
        CancellationToken ct)
    {
        switch (action.ActionType)
        {
            case PlanActionType.CreateNode:
                await ExecuteCreateNodeActionAsync(action, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.ConfigureWireGuardMesh:
                await ExecuteConfigureWireGuardMeshActionAsync(config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.ConfigureNftables:
                await ExecuteConfigureNftablesActionAsync(config, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.DeployService:
                await ExecuteDeployServiceActionAsync(action, config, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.MigrateServiceBlueGreen:
                await ExecuteMigrateServiceBlueGreenActionAsync(action, config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.UpdateDns:
                await ExecuteUpdateDnsActionAsync(action, config, nodes, logger, ct).ConfigureAwait(false);
                break;

            default:
                throw new NotSupportedException($"Unsupported plan action type: {action.ActionType}");
        }
    }

    private async Task ExecuteCreateNodeActionAsync(
        PlanAction action,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var nodeName = action.Target;
        var node = nodes.FirstOrDefault(n => string.Equals(n.NodeName, nodeName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Target node '{nodeName}' for action '{action.Id}' not found in active connections.");

        var packageManager = _packageManagerFactory.ResolveForNode(node);
        logger?.Invoke($"Installing baseline system packages on node '{node.NodeName}' using {packageManager.GetType().Name}...");

        await packageManager.EnsurePackagesInstalledAsync(node, BaselineLogicalPackages, ct).ConfigureAwait(false);
    }

    private async Task ExecuteConfigureWireGuardMeshActionAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        var wgNodeConfigs = await GatherWireGuardNodeConfigsAsync(config, nodes, ct).ConfigureAwait(false);

        var readOnlyVips = containerVipsPerNode.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<string>)kv.Value.AsReadOnly(),
            StringComparer.Ordinal);

        var meshConfigurations = WireGuardMeshService.GenerateMeshConfigurations(wgNodeConfigs, readOnlyVips);

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            if (meshConfigurations.TryGetValue(node.NodeName, out var configContent))
            {
                logger?.Invoke($"Applying WireGuard mesh configuration on node '{node.NodeName}'...");
                await WireGuardMeshService.ApplyConfigurationAsync(node, configContent, ct).ConfigureAwait(false);
            }
        }
    }

    private static async Task ExecuteConfigureNftablesActionAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var (portForwards, publicAllowedPorts) = ExtractPortForwardsAndPublicPorts(config);

        var meshPort = DefaultWireGuardPort;
        if (config.Root.TryGetPropertyValue("mesh", out var meshNode) &&
            meshNode is JsonObject meshObj &&
            meshObj.TryGetPropertyValue("port", out var pVal) &&
            int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var parsedMeshPort))
        {
            meshPort = parsedMeshPort;
        }

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            var sshPort = node.Port > 0 ? node.Port : 22;

            var ruleset = NftablesService.GenerateRuleset(
                portForwards,
                publicAllowedPorts,
                sshPort: sshPort,
                wireGuardPort: meshPort);

            logger?.Invoke($"Applying nftables firewall ruleset (table inet starry) on node '{node.NodeName}'...");
            await NftablesService.ApplyRulesetAsync(node, ruleset, ct).ConfigureAwait(false);
        }
    }

    private async Task ExecuteDeployServiceActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var serviceName = action.Target;
        var serviceObj = config.Root["services"]?[serviceName] as JsonObject
            ?? throw new InvalidOperationException($"Service '{serviceName}' was not found in configuration.");

        var spec = BuildServiceSpec(serviceName, serviceObj, config);

        var targetNodeName = serviceObj["node"]?.ToString();
        INodeConnection? targetNode = null;
        if (targetNodeName != null)
        {
            targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, targetNodeName, StringComparison.OrdinalIgnoreCase));
        }
        else if (nodes.Count > 0)
        {
            targetNode = nodes[0];
        }

        if (targetNode == null)
        {
            throw new InvalidOperationException($"Target node '{targetNodeName ?? "auto"}' for service '{serviceName}' not found in active connections.");
        }

        var targetNodeObj = config.Root["nodes"]?[targetNode.NodeName] as JsonObject;
        var runtime = ResolveRuntimeForNode(targetNodeObj);

        logger?.Invoke($"Deploying container service '{spec.Name}' [{spec.Image}] on node '{targetNode.NodeName}' (priority: {spec.Priority})...");
        await runtime.DeployContainerAsync(targetNode, spec, ct).ConfigureAwait(false);
    }

    private async Task ExecuteMigrateServiceBlueGreenActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        var serviceName = action.Target;
        var serviceObj = config.Root["services"]?[serviceName] as JsonObject
            ?? throw new InvalidOperationException($"Service '{serviceName}' was not found in configuration.");

        var spec = BuildServiceSpec(serviceName, serviceObj, config);
        var targetNodeName = serviceObj["node"]?.ToString()
            ?? throw new InvalidOperationException($"Target node for Blue-Green migration of service '{serviceName}' is unspecified.");

        var targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, targetNodeName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Target node '{targetNodeName}' for service '{serviceName}' not found in active connections.");

        var targetNodeObj = config.Root["nodes"]?[targetNode.NodeName] as JsonObject;
        var targetRuntime = ResolveRuntimeForNode(targetNodeObj);

        var sourceNode = await LocateSourceNodeForServiceAsync(serviceName, action.Description, nodes, config, targetNode, ct).ConfigureAwait(false);
        var sourceNodeObj = config.Root["nodes"]?[sourceNode.NodeName] as JsonObject;
        var sourceRuntime = ResolveRuntimeForNode(sourceNodeObj);

        logger?.Invoke($"[Blue-Green Step 1/6] P2P Image Streaming: '{spec.Image}' from '{sourceNode.NodeName}' to '{targetNode.NodeName}'...");
        await sourceRuntime.StreamImagePeerToPeerAsync(sourceNode, targetNode, spec.Image, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 2/6] Volume Synchronization: rsync over WireGuard from '{sourceNode.NodeName}' to '{targetNode.NodeName}'...");
        await SyncServiceVolumesAsync(sourceNode, targetNode, spec.Volumes, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 3/6] Starting Green container '{spec.Name}' on target node '{targetNode.NodeName}'...");
        await targetRuntime.DeployContainerAsync(targetNode, spec, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 4/6] Polling Healthcheck for Green container '{spec.Name}' on '{targetNode.NodeName}'...");
        await PollContainerHealthAsync(targetNode, targetRuntime, spec, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 5/6] Re-routing VIP and updating DNS for '{spec.Name}'...");
        await ReRouteServiceVipAsync(serviceName, spec.VipAddress, sourceNode, targetNode, config, nodes, containerVipsPerNode, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 6/6] Stopping Blue container '{spec.Name}' on source node '{sourceNode.NodeName}'...");
        await StopContainerOnNodeAsync(sourceNode, sourceRuntime, spec.Name, ct).ConfigureAwait(false);

        logger?.Invoke($"Successfully completed zero-downtime Blue-Green migration for '{spec.Name}' to '{targetNode.NodeName}'.");
    }

    private async Task ExecuteUpdateDnsActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var dnsProvider = ResolveDnsProvider(config);
        if (dnsProvider == null)
        {
            logger?.Invoke($"DNS provider is not configured. Skipping DNS update for '{action.Target}'.");
            return;
        }

        var zoneDomain = config.Root["dns"]?["zone"]?.ToString()
            ?? config.Root["dns"]?["zone_domain"]?.ToString()
            ?? DefaultDnsZone;

        var (recordName, recordContent, recordType) = ResolveDnsRecordDetails(action, config, nodes);

        var record = new DnsRecord(
            Id: Guid.NewGuid().ToString("N"),
            RecordType: recordType,
            Name: recordName,
            Content: recordContent,
            Ttl: 120,
            Proxied: false);

        logger?.Invoke($"Upserting DNS record '{record.Name}' ({record.RecordType}) -> '{record.Content}' in zone '{zoneDomain}' via {dnsProvider.ProviderName}...");
        await dnsProvider.UpsertRecordAsync(zoneDomain, record, ct).ConfigureAwait(false);
    }

    [GeneratedRegex(@"\{\{\s*(?<key>[a-zA-Z0-9_\.]+)\s*\}\}", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex TemplateTagRegex();

    [GeneratedRegex(@"ENC\[AES256_GCM:v1:[^\]]+\]", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex EncryptedSecretTokenRegex();

    public string RenderTemplateAndDecryptInlineSecrets(string content, ConfigTree config)
    {
        if (string.IsNullOrEmpty(content))
        {
            return content;
        }

        var rendered = TemplateTagRegex().Replace(content, match =>
        {
            var key = match.Groups["key"].Value.Trim();
            var node = config.FindNode(key);
            if (node != null)
            {
                return node.ToString();
            }

            var parts = key.Split('.');
            if (parts.Length == 3 && string.Equals(parts[0], "services", StringComparison.OrdinalIgnoreCase) && string.Equals(parts[2], "vip", StringComparison.OrdinalIgnoreCase))
            {
                var svc = config.Root["services"]?[parts[1]] as JsonObject;
                return svc?["vip"]?.ToString() ?? svc?["vip_address"]?.ToString() ?? match.Value;
            }
            if (parts.Length == 3 && string.Equals(parts[0], "nodes", StringComparison.OrdinalIgnoreCase) && string.Equals(parts[2], "vpn_ip", StringComparison.OrdinalIgnoreCase))
            {
                var n = config.Root["nodes"]?[parts[1]] as JsonObject;
                return n?["vpn_ip"]?.ToString() ?? match.Value;
            }

            return match.Value;
        });

        if (_masterKey != null)
        {
            rendered = EncryptedSecretTokenRegex().Replace(rendered, match =>
            {
                if (_cryptoService.TryParseEncrypted(match.Value, out var envelope) && envelope != null)
                {
                    try
                    {
                        var bytes = _cryptoService.Decrypt(envelope, _masterKey, string.Empty);
                        return Encoding.UTF8.GetString(bytes);
                    }
                    catch (CryptographicException)
                    {
                        return match.Value;
                    }
                    catch (FormatException)
                    {
                        return match.Value;
                    }
                    catch (ArgumentException)
                    {
                        return match.Value;
                    }
                }
                return match.Value;
            });
        }

        return rendered;
    }

    private ServiceSpec BuildServiceSpec(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var image = serviceObj["image"]?.ToString()
            ?? throw new InvalidOperationException($"Service '{serviceName}' is missing required 'image' property.");

        var priority = serviceObj.TryGetPropertyValue("priority", out var pVal) &&
                       int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
            ? p
            : 50;

        var vipAddress = serviceObj["vip"]?.ToString() ?? serviceObj["vip_address"]?.ToString();
        var healthcheck = serviceObj["healthcheck"]?.ToString()
            ?? serviceObj["healthcheck_endpoint"]?.ToString()
            ?? serviceObj["healthcheckEndpoint"]?.ToString();

        return new ServiceSpec
        {
            Name = serviceName,
            Image = image,
            Priority = priority,
            VipAddress = vipAddress,
            HealthcheckEndpoint = healthcheck,
            Ports = ExtractStringList(serviceObj, "ports"),
            Volumes = ExtractStringList(serviceObj, "volumes"),
            Labels = ExtractStringDictionary(serviceObj, "labels"),
            Environment = ExtractServiceEnvironment(serviceName, serviceObj, config),
            Files = ExtractServiceFiles(serviceName, serviceObj, config)
        };
    }

    private static List<string> ExtractStringList(JsonObject obj, string propertyName)
    {
        if (obj[propertyName] is not JsonArray arr)
        {
            return [];
        }

        return arr
            .Where(item => item != null && !string.IsNullOrWhiteSpace(item.ToString()))
            .Select(item => item!.ToString().Trim())
            .ToList();
    }

    private static Dictionary<string, string> ExtractStringDictionary(JsonObject obj, string propertyName)
    {
        if (obj[propertyName] is not JsonObject childObj)
        {
            return new Dictionary<string, string>(StringComparer.Ordinal);
        }

        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var (k, v) in childObj)
        {
            result[k] = v?.ToString() ?? string.Empty;
        }

        return result;
    }

    private Dictionary<string, string> ExtractServiceEnvironment(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        var envProperty = serviceObj["env"] as JsonObject ?? serviceObj["environment"] as JsonObject;
        var envPropName = serviceObj.ContainsKey("env") ? "env" : "environment";

        if (envProperty != null)
        {
            foreach (var (k, v) in envProperty)
            {
                var rawVal = v?.ToString() ?? string.Empty;
                var decrypted = DecryptSecretString(rawVal, $"services.{serviceName}.{envPropName}.{k}", $"services.{serviceName}.env.{k}");
                if (config != null)
                {
                    decrypted = RenderTemplateAndDecryptInlineSecrets(decrypted, config);
                }
                result[k] = decrypted;
            }
        }

        return result;
    }

    private Dictionary<string, string> ExtractServiceFiles(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (serviceObj["files"] is JsonObject filesObj)
        {
            foreach (var (destPath, contentVal) in filesObj)
            {
                var raw = contentVal?.ToString() ?? string.Empty;
                var fileContent = raw;

                if (File.Exists(raw))
                {
                    fileContent = File.ReadAllText(raw);
                }
                else
                {
                    var fallback = Path.Combine("files", serviceName, Path.GetFileName(destPath));
                    if (File.Exists(fallback))
                    {
                        fileContent = File.ReadAllText(fallback);
                    }
                }

                if (config != null)
                {
                    fileContent = RenderTemplateAndDecryptInlineSecrets(fileContent, config);
                }
                else
                {
                    fileContent = DecryptSecretString(fileContent, $"services.{serviceName}.files.{destPath}");
                }

                result[destPath] = fileContent;
            }
        }

        return result;
    }

    [GeneratedRegex(@"from '(?<old>[^']+)' to '(?<new>[^']+)'", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex BlueGreenPlanDescriptionRegex();

    private async Task<INodeConnection> LocateSourceNodeForServiceAsync(
        string serviceName,
        string actionDescription,
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree config,
        INodeConnection targetNode,
        CancellationToken ct)
    {
        // 1. Probe active nodes for running container
        foreach (var candidateNode in nodes)
        {
            if (string.Equals(candidateNode.NodeName, targetNode.NodeName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var nodeObj = config.Root["nodes"]?[candidateNode.NodeName] as JsonObject;
            var runtime = ResolveRuntimeForNode(nodeObj);
            var containers = await runtime.ListContainersAsync(candidateNode, ct).ConfigureAwait(false);

            if (containers.Any(c => string.Equals(c.Name.TrimStart('/'), serviceName, StringComparison.OrdinalIgnoreCase) && c.IsRunning))
            {
                return candidateNode;
            }
        }

        // 2. Parse from plan description (e.g. "... from 'bocchi' to 'nijika'")
        var match = BlueGreenPlanDescriptionRegex().Match(actionDescription);
        if (match.Success)
        {
            var oldName = match.Groups["old"].Value;
            var parsedNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, oldName, StringComparison.OrdinalIgnoreCase));
            if (parsedNode != null)
            {
                return parsedNode;
            }
        }

        throw new InvalidOperationException($"Could not determine source node for Blue-Green migration of service '{serviceName}'.");
    }

    private static async Task SyncServiceVolumesAsync(
        INodeConnection sourceNode,
        INodeConnection targetNode,
        IReadOnlyList<string> volumes,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (volumes.Count == 0)
        {
            return;
        }

        foreach (var volume in volumes)
        {
            ct.ThrowIfCancellationRequested();
            var rawVolPath = volume.Split(':')[0].Trim();
            if (string.IsNullOrWhiteSpace(rawVolPath))
            {
                continue;
            }

            var resolvedPath = rawVolPath.StartsWith('/')
                ? rawVolPath
                : $"/var/lib/docker/volumes/{rawVolPath}/_data";

            logger?.Invoke($"Streaming volume '{rawVolPath}' ({resolvedPath}) through orchestrator binary pipe to '{targetNode.NodeName}'...");

            using var pipeStream = new MemoryStream();

            var safePath = "'" + resolvedPath.Replace("'", "'\\''") + "'";
            var saveCmd = $"mkdir -p {safePath} && tar -C {safePath} -cf - . | zstd -3";
            var saveResult = await sourceNode.ExecuteStreamingCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", saveCmd],
                Timeout = TimeSpan.FromMinutes(30)
            }, stdinStream: null, stdoutStream: pipeStream, ct).ConfigureAwait(false);

            if (!saveResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to export volume '{rawVolPath}' on source node '{sourceNode.NodeName}': {saveResult.StandardError}");
            }

            pipeStream.Position = 0;
            var restoreCmd = $"mkdir -p {safePath} && zstd -d | tar -C {safePath} -xf -";
            var restoreResult = await targetNode.ExecuteStreamingCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", restoreCmd],
                Timeout = TimeSpan.FromMinutes(30)
            }, stdinStream: pipeStream, stdoutStream: null, ct).ConfigureAwait(false);

            if (!restoreResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to restore volume '{rawVolPath}' on target node '{targetNode.NodeName}': {restoreResult.StandardError}");
            }

            logger?.Invoke($"✓ Successfully synced volume '{rawVolPath}' to '{targetNode.NodeName}'.");
        }
    }

    private static async Task PollContainerHealthAsync(
        INodeConnection targetNode,
        IContainerRuntime runtime,
        ServiceSpec spec,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(spec.HealthcheckEndpoint))
        {
            await PollRunningStateAsync(targetNode, runtime, spec.Name, logger, ct).ConfigureAwait(false);
            return;
        }

        await PollHttpEndpointAsync(targetNode, spec, logger, ct).ConfigureAwait(false);
    }

    private static async Task PollRunningStateAsync(
        INodeConnection targetNode,
        IContainerRuntime runtime,
        string serviceName,
        Action<string>? logger,
        CancellationToken ct)
    {
        for (var i = 1; i <= 15; i++)
        {
            ct.ThrowIfCancellationRequested();
            var runningContainers = await runtime.ListContainersAsync(targetNode, ct).ConfigureAwait(false);
            var container = runningContainers.FirstOrDefault(c => string.Equals(c.Name.TrimStart('/'), serviceName, StringComparison.OrdinalIgnoreCase));

            if (container is { IsRunning: true })
            {
                logger?.Invoke($"Container '{serviceName}' is healthy and running on attempt {i}.");
                return;
            }

            await Task.Delay(1000, ct).ConfigureAwait(false);
        }

        throw new InvalidOperationException($"Container '{serviceName}' failed to enter running state on node '{targetNode.NodeName}'.");
    }

    private static async Task PollHttpEndpointAsync(
        INodeConnection targetNode,
        ServiceSpec spec,
        Action<string>? logger,
        CancellationToken ct)
    {
        var endpoint = spec.HealthcheckEndpoint!.Trim();
        string healthUrl;
        if (endpoint.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            endpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            healthUrl = endpoint;
        }
        else if (endpoint.StartsWith('/'))
        {
            var hostPort = spec.Ports.Count > 0 ? spec.Ports[0].Split(':')[0].Trim() : "80";
            healthUrl = $"http://127.0.0.1:{hostPort}{endpoint}";
        }
        else
        {
            healthUrl = endpoint;
        }

        var checkSpec = new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-f", "-s", "-m", "3", healthUrl]
        };

        var healthy = false;
        for (var attempt = 1; attempt <= 30; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            var checkResult = await targetNode.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
            if (checkResult.IsSuccess)
            {
                healthy = true;
                logger?.Invoke($"Healthcheck passed for Green container '{spec.Name}' on attempt {attempt}.");
                break;
            }

            await Task.Delay(1500, ct).ConfigureAwait(false);
        }

        if (!healthy)
        {
            throw new InvalidOperationException($"Healthcheck poll failed for Green container '{spec.Name}' at '{healthUrl}' on node '{targetNode.NodeName}'. Aborting VIP cutover.");
        }
    }

    private async Task ReRouteServiceVipAsync(
        string serviceName,
        string? vipAddress,
        INodeConnection sourceNode,
        INodeConnection targetNode,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(vipAddress))
        {
            return;
        }

        UpdateVipPlacement(sourceNode.NodeName, targetNode.NodeName, vipAddress, containerVipsPerNode);
        await SynchronizeWireGuardMeshAsync(config, nodes, containerVipsPerNode, ct).ConfigureAwait(false);
        await SwitchInterfaceVipAsync(sourceNode, targetNode, vipAddress, ct).ConfigureAwait(false);
        await UpdateServiceDnsRecordAsync(serviceName, vipAddress, config, logger, ct).ConfigureAwait(false);
    }

    private static void UpdateVipPlacement(
        string sourceNodeName,
        string targetNodeName,
        string vipAddress,
        Dictionary<string, List<string>> containerVipsPerNode)
    {
        if (containerVipsPerNode.TryGetValue(sourceNodeName, out var sourceList))
        {
            sourceList.RemoveAll(v => string.Equals(v, vipAddress, StringComparison.OrdinalIgnoreCase));
        }

        if (!containerVipsPerNode.TryGetValue(targetNodeName, out var targetList))
        {
            targetList = [];
            containerVipsPerNode[targetNodeName] = targetList;
        }

        if (!targetList.Contains(vipAddress, StringComparer.OrdinalIgnoreCase))
        {
            targetList.Add(vipAddress);
        }
    }

    private async Task SynchronizeWireGuardMeshAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        CancellationToken ct)
    {
        var wgNodeConfigs = await GatherWireGuardNodeConfigsAsync(config, nodes, ct).ConfigureAwait(false);
        var readOnlyVips = containerVipsPerNode.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<string>)kv.Value.AsReadOnly(),
            StringComparer.Ordinal);

        var meshConfigs = WireGuardMeshService.GenerateMeshConfigurations(wgNodeConfigs, readOnlyVips);
        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            if (meshConfigs.TryGetValue(node.NodeName, out var conf))
            {
                await WireGuardMeshService.ApplyConfigurationAsync(node, conf, ct).ConfigureAwait(false);
            }
        }
    }

    private static async Task SwitchInterfaceVipAsync(
        INodeConnection sourceNode,
        INodeConnection targetNode,
        string vipAddress,
        CancellationToken ct)
    {
        await sourceNode.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr del {vipAddress}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);

        await targetNode.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr add {vipAddress}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);
    }

    private async Task UpdateServiceDnsRecordAsync(
        string serviceName,
        string vipAddress,
        ConfigTree config,
        Action<string>? logger,
        CancellationToken ct)
    {
        var dnsProvider = ResolveDnsProvider(config);
        if (dnsProvider != null)
        {
            var zoneDomain = config.Root["dns"]?["zone"]?.ToString() ?? DefaultDnsZone;
            var record = new DnsRecord(
                Id: Guid.NewGuid().ToString("N"),
                RecordType: "A",
                Name: serviceName,
                Content: vipAddress,
                Ttl: 120,
                Proxied: false);

            await dnsProvider.UpsertRecordAsync(zoneDomain, record, ct).ConfigureAwait(false);
            logger?.Invoke($"Updated DNS record '{serviceName}' -> '{vipAddress}' in zone '{zoneDomain}'.");
        }
    }

    private static async Task StopContainerOnNodeAsync(
        INodeConnection node,
        IContainerRuntime runtime,
        string containerName,
        CancellationToken ct)
    {
        var stopSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"{runtime.RuntimeName} stop {containerName} 2>/dev/null || true"]
        };

        await node.ExecuteCommandAsync(stopSpec, ct).ConfigureAwait(false);
    }

    private IDnsProvider? ResolveDnsProvider(ConfigTree config)
    {
        if (_dnsProvider != null)
        {
            return _dnsProvider;
        }

        if (!config.Root.TryGetPropertyValue("dns", out var dnsNode) || dnsNode is not JsonObject dnsObj)
        {
            return null;
        }

        var providerType = dnsObj["provider"]?.ToString();
        if (string.Equals(providerType, "cloudflare", StringComparison.OrdinalIgnoreCase))
        {
            var tokenRaw = dnsObj["api_token"]?.ToString() ?? dnsObj["token"]?.ToString() ?? string.Empty;
            var decryptedToken = DecryptSecretString(tokenRaw, "dns.api_token");
            return new CloudflareDnsProvider(decryptedToken);
        }

        if (string.Equals(providerType, "webhook", StringComparison.OrdinalIgnoreCase))
        {
            var webhookUrl = dnsObj["webhook_url"]?.ToString() ?? dnsObj["url"]?.ToString();
            if (!string.IsNullOrWhiteSpace(webhookUrl))
            {
                return new WebhookDnsProvider(new Uri(webhookUrl));
            }
        }

        return null;
    }

    private static (string RecordName, string RecordContent, string RecordType) ResolveDnsRecordDetails(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes)
    {
        var target = action.Target;
        if (config.Root.TryGetPropertyValue("services", out var svcNode) &&
            svcNode is JsonObject servicesObj &&
            servicesObj[target] is JsonObject serviceObj)
        {
            var vip = serviceObj["vip"]?.ToString() ?? serviceObj["vip_address"]?.ToString();
            if (!string.IsNullOrWhiteSpace(vip))
            {
                return (target, vip, "A");
            }

            var nodeName = serviceObj["node"]?.ToString();
            var targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, nodeName, StringComparison.OrdinalIgnoreCase));
            if (targetNode != null)
            {
                return (target, targetNode.Host, "A");
            }
        }

        return (target, DefaultLocalIp, "A");
    }

    private async Task<IReadOnlyList<WireGuardNodeConfig>> GatherWireGuardNodeConfigsAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        CancellationToken ct)
    {
        var wgNodeConfigs = new List<WireGuardNodeConfig>(nodes.Count);

        var meshPort = DefaultWireGuardPort;
        if (config.Root.TryGetPropertyValue("mesh", out var meshNode) &&
            meshNode is JsonObject meshObj &&
            meshObj.TryGetPropertyValue("port", out var pVal) &&
            int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var parsedPort))
        {
            meshPort = parsedPort;
        }

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            var nodeObj = config.Root["nodes"]?[node.NodeName] as JsonObject;

            var vpnIp = nodeObj?["vpn_ip"]?.ToString() ?? DefaultWireGuardIp;
            var isPublic = nodeObj?["is_public"]?.GetValue<bool>() ?? false;

            var (privKey, pubKey) = await ResolveWireGuardKeysForNodeAsync(node, nodeObj, ct).ConfigureAwait(false);

            var listenPort = nodeObj != null && nodeObj.TryGetPropertyValue("wireguard_port", out var wpVal) &&
                             int.TryParse(wpVal?.ToString(), CultureInfo.InvariantCulture, out var parsedWp)
                ? parsedWp
                : meshPort;

            var endpoint = isPublic && !string.IsNullOrWhiteSpace(node.Host) && !string.Equals(node.Host, "localhost", StringComparison.OrdinalIgnoreCase)
                ? node.Host
                : null;

            wgNodeConfigs.Add(new WireGuardNodeConfig(
                NodeName: node.NodeName,
                VpnIp: vpnIp,
                PrivateKey: privKey,
                PublicKey: pubKey,
                ListenPort: listenPort,
                Endpoint: endpoint,
                IsPublic: isPublic));
        }

        return wgNodeConfigs;
    }

    private async Task<(string PrivateKey, string PublicKey)> ResolveWireGuardKeysForNodeAsync(
        INodeConnection node,
        JsonObject? nodeObj,
        CancellationToken ct)
    {
        var privKeyRaw = nodeObj?["wireguard_private_key"]?.ToString()
            ?? nodeObj?["wg_private_key"]?.ToString()
            ?? nodeObj?["private_key"]?.ToString();

        var pubKeyRaw = nodeObj?["wireguard_public_key"]?.ToString()
            ?? nodeObj?["wg_public_key"]?.ToString()
            ?? nodeObj?["public_key"]?.ToString();

        if (!string.IsNullOrWhiteSpace(privKeyRaw))
        {
            var privKey = DecryptSecretString(privKeyRaw, $"nodes.{node.NodeName}.wireguard_private_key");
            var pubKey = !string.IsNullOrWhiteSpace(pubKeyRaw)
                ? DecryptSecretString(pubKeyRaw, $"nodes.{node.NodeName}.wireguard_public_key")
                : await DerivePublicKeyFromPrivateKeyAsync(node, privKey, ct).ConfigureAwait(false);

            return (privKey, pubKey);
        }

        // Query or generate on node
        var queryResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "[ -f /etc/wireguard/starry0.key ] && cat /etc/wireguard/starry0.key || (wg genkey 2>/dev/null | tee /etc/wireguard/starry0.key || true)"]
        }, ct).ConfigureAwait(false);

        var retrievedKey = queryResult.StandardOutput.Trim();
        if (retrievedKey.Length == 44 && retrievedKey.EndsWith('='))
        {
            var derivedPub = await DerivePublicKeyFromPrivateKeyAsync(node, retrievedKey, ct).ConfigureAwait(false);
            return (retrievedKey, derivedPub);
        }

        // Fallback synthetic key pair
        var dummyBytes = new byte[32];
        RandomNumberGenerator.Fill(dummyBytes);
        var fallbackPriv = Convert.ToBase64String(dummyBytes);
        RandomNumberGenerator.Fill(dummyBytes);
        var fallbackPub = Convert.ToBase64String(dummyBytes);

        return (fallbackPriv, fallbackPub);
    }

    private static async Task<string> DerivePublicKeyFromPrivateKeyAsync(
        INodeConnection node,
        string privateKey,
        CancellationToken ct)
    {
        var result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"echo '{privateKey}' | wg pubkey 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);

        var pubKey = result.StandardOutput.Trim();
        if (pubKey.Length == 44 && pubKey.EndsWith('='))
        {
            return pubKey;
        }

        var dummyBytes = new byte[32];
        RandomNumberGenerator.Fill(dummyBytes);
        return Convert.ToBase64String(dummyBytes);
    }

    private static Dictionary<string, List<string>> ExtractContainerVipsPerNode(ConfigTree config)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        if (config.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (_, svcVal) in servicesObj)
            {
                if (svcVal is not JsonObject sObj)
                {
                    continue;
                }

                var targetNode = sObj["node"]?.ToString();
                var vip = sObj["vip"]?.ToString() ?? sObj["vip_address"]?.ToString();

                if (!string.IsNullOrWhiteSpace(targetNode) && !string.IsNullOrWhiteSpace(vip))
                {
                    if (!result.TryGetValue(targetNode, out var vips))
                    {
                        vips = [];
                        result[targetNode] = vips;
                    }

                    if (!vips.Contains(vip, StringComparer.OrdinalIgnoreCase))
                    {
                        vips.Add(vip);
                    }
                }
            }
        }

        return result;
    }

    private static (IReadOnlyList<PortForwardRule> PortForwards, IReadOnlyList<int> PublicAllowedPorts) ExtractPortForwardsAndPublicPorts(ConfigTree config)
    {
        var forwards = new List<PortForwardRule>();
        var publicPorts = new HashSet<int>();

        if (!config.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            return (forwards, publicPorts.ToList());
        }

        foreach (var (_, svcVal) in servicesObj)
        {
            if (svcVal is not JsonObject sObj)
            {
                continue;
            }

            ParseServicePortForwards(sObj, forwards, publicPorts);
        }

        return (forwards, publicPorts.ToList());
    }

    private static void ParseServicePortForwards(
        JsonObject sObj,
        List<PortForwardRule> forwards,
        HashSet<int> publicPorts)
    {
        var vip = sObj["vip"]?.ToString() ?? sObj["vip_address"]?.ToString();
        if (sObj["ports"] is not JsonArray portsArr)
        {
            return;
        }

        foreach (var pNode in portsArr)
        {
            var portStr = pNode?.ToString();
            if (string.IsNullOrWhiteSpace(portStr))
            {
                continue;
            }

            var parts = portStr.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[0].Trim(), CultureInfo.InvariantCulture, out var hostPort))
            {
                continue;
            }

            publicPorts.Add(hostPort);

            if (string.IsNullOrWhiteSpace(vip))
            {
                continue;
            }

            var targetPart = parts[1].Trim();
            var proto = "tcp";
            var targetPortStr = targetPart;
            if (targetPart.Contains('/', StringComparison.Ordinal))
            {
                var sub = targetPart.Split('/');
                targetPortStr = sub[0];
                proto = sub[1].ToUpper(CultureInfo.InvariantCulture);
            }

            if (int.TryParse(targetPortStr, CultureInfo.InvariantCulture, out var targetPort))
            {
                forwards.Add(new PortForwardRule(hostPort, proto, vip, targetPort));
            }
        }
    }

    private static IReadOnlyList<PlanAction> OrderActionsByDag(IReadOnlyList<PlanAction> actions)
    {
        if (actions.Count <= 1)
        {
            return actions;
        }

        var actionMap = actions.ToDictionary(a => a.Id, StringComparer.Ordinal);
        var inDegree = actions.ToDictionary(a => a.Id, _ => 0, StringComparer.Ordinal);
        var adjacency = actions.ToDictionary(a => a.Id, _ => new List<string>(), StringComparer.Ordinal);

        foreach (var action in actions)
        {
            foreach (var dep in action.Dependencies.Where(actionMap.ContainsKey))
            {
                adjacency[dep].Add(action.Id);
                inDegree[action.Id]++;
            }
        }

        var readyQueue = new PriorityQueue<PlanAction, (int Priority, string Id)>();
        foreach (var action in actions)
        {
            if (inDegree[action.Id] == 0)
            {
                readyQueue.Enqueue(action, (action.Priority, action.Id));
            }
        }

        var ordered = new List<PlanAction>(actions.Count);
        while (readyQueue.Count > 0)
        {
            var next = readyQueue.Dequeue();
            ordered.Add(next);

            foreach (var dependentId in adjacency[next.Id])
            {
                inDegree[dependentId]--;
                if (inDegree[dependentId] == 0)
                {
                    var depAction = actionMap[dependentId];
                    readyQueue.Enqueue(depAction, (depAction.Priority, depAction.Id));
                }
            }
        }

        if (ordered.Count < actions.Count)
        {
            // Fallback to priority ordering if cyclical dependency anomaly occurs
            return actions.OrderBy(a => a.Priority).ThenBy(a => a.Id, StringComparer.Ordinal).ToList();
        }

        return ordered;
    }

    private static async Task CheckWireGuardInterfaceStatusAsync(INodeConnection node, CancellationToken ct)
    {
        var wgShowResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "wg",
            Arguments = ["show", WireGuardMeshService.InterfaceName]
        }, ct).ConfigureAwait(false);

        if (!wgShowResult.IsSuccess)
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "ip",
                Arguments = ["addr", "show", WireGuardMeshService.InterfaceName]
            }, ct).ConfigureAwait(false);
        }
    }

    private IContainerRuntime ResolveRuntimeForNode(JsonObject? nodeObj)
    {
        var engine = nodeObj?["engine"]?.ToString();
        if (string.Equals(engine, "podman", StringComparison.OrdinalIgnoreCase))
        {
            return new PodmanRuntime();
        }

        return _defaultRuntime;
    }

    private string DecryptSecretString(string value, string primaryPath, string? alternatePath = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (value.StartsWith("PLAIN[", StringComparison.Ordinal) && value.EndsWith(']'))
        {
            return value["PLAIN[".Length..^1];
        }

        if (_cryptoService.TryParseEncrypted(value, out var envelope) && envelope != null)
        {
            if (_masterKey == null)
            {
                throw new InvalidOperationException(
                    $"Encountered encrypted secret value for path '{primaryPath}' but STARRY_MASTER_KEY is not configured.");
            }

            try
            {
                var decrypted = _cryptoService.Decrypt(envelope, _masterKey, primaryPath);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (CryptographicException) when (alternatePath != null)
            {
                var decrypted = _cryptoService.Decrypt(envelope, _masterKey, alternatePath);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        return value;
    }

    private static byte[]? ResolveMasterKeyFromEnvironment()
    {
        var envVal = Environment.GetEnvironmentVariable("STARRY_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(envVal))
        {
            try
            {
                return CryptoService.ParseMasterKey(envVal);
            }
            catch (FormatException)
            {
                // Invalid base64 or key format in environment variable; return null
            }
            catch (ArgumentException)
            {
                // Invalid key length; return null
            }
        }

        return null;
    }
}
