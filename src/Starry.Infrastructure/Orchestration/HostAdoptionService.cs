#pragma warning disable MA0048 // File name must match type name
#pragma warning disable CA1819 // Properties should not return arrays

using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Core.Security;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Options controlling the adoption and configuration generation process for an existing host.
/// </summary>
public sealed record AdoptionOptions
{
    /// <summary>
    /// Target directory where modular configuration files will be written (e.g. "./config").
    /// </summary>
    public string OutDir { get; init; } = "./config";

    /// <summary>
    /// Whether to pull single-file bind mounts from the host into local files directory.
    /// </summary>
    public bool PullFiles { get; init; }

    /// <summary>
    /// Configuration format for generated files ("yaml", "json", "toml").
    /// </summary>
    public string Format { get; init; } = "yaml";

    /// <summary>
    /// 32-byte master encryption key. If omitted, uses STARRY_MASTER_KEY or generates a new key.
    /// </summary>
    public byte[]? MasterKey { get; init; }

    /// <summary>
    /// Local directory where pulled host files will be stored.
    /// </summary>
    public string FilesDir { get; init; } = "./files";

    /// <summary>
    /// Default cluster name for the generated root configuration.
    /// </summary>
    public string ClusterName { get; init; } = "starry-cluster";
}

/// <summary>
/// Summary descriptor of a container running on a node that is not managed by Starry configuration.
/// </summary>
public sealed record UnmanagedContainerSummary(
    string NodeName,
    string ContainerId,
    string ContainerName,
    string Image,
    string State,
    IReadOnlyDictionary<string, string> Labels);

/// <summary>
/// Service contract for brownfield host adoption, live mesh network attachment,
/// and unmanaged container discovery.
/// </summary>
public interface IHostAdoptionService
{
    /// <summary>
    /// Probes an existing node, discovers running containers and compose projects,
    /// encrypts sensitive environment secrets, pulls configuration files, and outputs
    /// modular configuration files into the specified directory.
    /// </summary>
    Task<ConfigTree> AdoptHostAsync(INodeConnection node, AdoptionOptions options, CancellationToken ct = default);

    /// <summary>
    /// Connects a live, running container to the Starry WireGuard mesh network without restarting it.
    /// </summary>
    Task AttachLiveContainerToMeshAsync(INodeConnection node, string containerId, string vip, CancellationToken ct = default);

    /// <summary>
    /// Discovers containers running across nodes that are not defined in the current configuration tree.
    /// </summary>
    Task<IReadOnlyList<UnmanagedContainerSummary>> ListUnmanagedContainersAsync(
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree currentConfig,
        CancellationToken ct = default);
}

/// <summary>
/// Implements brownfield host adoption, live container mesh attachment, and unmanaged container inspection.
/// </summary>
public sealed class HostAdoptionService : IHostAdoptionService
{
    private static readonly Uri BaseDockerUri = new("http://127.0.0.1/");
    private const string ApiVersionPrefix = "/v1.40";

    private static readonly string[] SecretKeyIndicators =
    [
        "PASS",
        "PASSWORD",
        "SECRET",
        "KEY",
        "TOKEN",
        "PRIVATE",
        "DSN"
    ];

    private static readonly string[] DbKeywords = ["POSTGRES", "MYSQL", "MARIADB", "REDIS", "MONGO"];
    private static readonly string[] ProxyKeywords = ["NGINX", "TRAEFIK", "CADDY", "NPM"];

    private readonly CryptoService _cryptoService;
    private readonly VirtualConfigEngine _configEngine;
    private readonly IReadOnlyList<IContainerRuntime> _runtimes;

    public HostAdoptionService(
        CryptoService? cryptoService = null,
        VirtualConfigEngine? configEngine = null,
        IEnumerable<IContainerRuntime>? runtimes = null)
    {
        _cryptoService = cryptoService ?? new CryptoService();
        _configEngine = configEngine ?? new VirtualConfigEngine();
        _runtimes = runtimes?.ToList() ?? [new DockerRuntime(), new PodmanRuntime()];
    }

    public async Task<ConfigTree> AdoptHostAsync(INodeConnection node, AdoptionOptions options, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(options);

        var (runtime, socketPath) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
        var discoveredContainers = await DiscoverContainersAsync(node, runtime, socketPath, ct).ConfigureAwait(false);

        var masterKey = ResolveMasterKey(options);
        var format = string.IsNullOrWhiteSpace(options.Format) ? "yaml" : options.Format.Trim();

        var tree = InitializeConfigTree(options, format);
        EnsureClusterMetadata(tree, options);
        RegisterNodeInConfig(tree, node, runtime);

        if (!tree.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            servicesObj = [];
            tree.Root["services"] = servicesObj;
        }

        await AdoptDiscoveredContainersAsync(node, discoveredContainers, servicesObj, masterKey, options, ct).ConfigureAwait(false);

        _configEngine.Split(tree, options.OutDir, format);
        return tree;
    }

    public async Task AttachLiveContainerToMeshAsync(INodeConnection node, string containerId, string vip, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(vip);

        var (_, socketPath) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        var targetNetId = await FindOrCreateMeshNetworkAsync(client, ct).ConfigureAwait(false);

        var connectPayload = new JsonObject
        {
            ["Container"] = containerId,
            ["EndpointConfig"] = new JsonObject
            {
                ["IPAMConfig"] = new JsonObject
                {
                    ["IPv4Address"] = vip
                }
            }
        };

        using var connectContent = new StringContent(connectPayload.ToJsonString(), Encoding.UTF8, "application/json");
        using var connectRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks/{Uri.EscapeDataString(targetNetId)}/connect"),
            connectContent,
            ct).ConfigureAwait(false);

        if (!connectRes.IsSuccessStatusCode)
        {
            var err = await connectRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (connectRes.StatusCode != HttpStatusCode.Conflict &&
                !err.Contains("already exists in network", StringComparison.OrdinalIgnoreCase) &&
                !err.Contains("is already attached", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Failed to connect container '{containerId}' to network '{targetNetId}': {err}");
            }
        }

        await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr add {vip}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<UnmanagedContainerSummary>> ListUnmanagedContainersAsync(
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree currentConfig,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNull(currentConfig);

        var managedServiceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (currentConfig.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (svcName, _) in servicesObj)
            {
                managedServiceNames.Add(svcName);
            }
        }

        var unmanagedList = new List<UnmanagedContainerSummary>();

        foreach (var node in nodes)
        {
            var (runtime, _) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
            var containers = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);

            foreach (var container in containers)
            {
                var cleanName = container.Name.TrimStart('/');
                var composeService = container.Labels.GetValueOrDefault("com.docker.compose.service");
                var composeProject = container.Labels.GetValueOrDefault("com.docker.compose.project");
                var isManagedByStarry = string.Equals(container.Labels.GetValueOrDefault("managed-by"), "starry", StringComparison.OrdinalIgnoreCase);

                var isManaged = isManagedByStarry ||
                                managedServiceNames.Contains(cleanName) ||
                                (!string.IsNullOrEmpty(composeService) && managedServiceNames.Contains(composeService)) ||
                                (!string.IsNullOrEmpty(composeProject) && managedServiceNames.Contains(composeProject)) ||
                                (!string.IsNullOrEmpty(composeProject) && !string.IsNullOrEmpty(composeService) && managedServiceNames.Contains($"{composeProject}-{composeService}"));

                if (!isManaged)
                {
                    unmanagedList.Add(new UnmanagedContainerSummary(
                        NodeName: node.NodeName,
                        ContainerId: container.Id,
                        ContainerName: cleanName,
                        Image: container.Image,
                        State: container.Status,
                        Labels: container.Labels));
                }
            }
        }

        return unmanagedList;
    }

    private static async Task<List<DiscoveredContainerInfo>> DiscoverContainersAsync(
        INodeConnection node,
        IContainerRuntime runtime,
        string socketPath,
        CancellationToken ct)
    {
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);
        var containerStates = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);
        var discovered = new List<DiscoveredContainerInfo>(containerStates.Count);

        foreach (var state in containerStates)
        {
            var details = await QueryContainerDetailsAsync(client, state.Id, ct).ConfigureAwait(false);
            if (details != null)
            {
                discovered.Add(new DiscoveredContainerInfo(state, details));
            }
        }

        return discovered;
    }

    private async Task AdoptDiscoveredContainersAsync(
        INodeConnection node,
        List<DiscoveredContainerInfo> discovered,
        JsonObject servicesObj,
        byte[] masterKey,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var projectGroups = discovered
            .GroupBy(c => !string.IsNullOrWhiteSpace(c.ComposeProject) ? c.ComposeProject : c.CleanName, StringComparer.OrdinalIgnoreCase);

        var adoptedServiceNames = new HashSet<string>(servicesObj.Select(s => s.Key), StringComparer.OrdinalIgnoreCase);

        foreach (var group in projectGroups)
        {
            var groupKey = group.Key;
            var groupContainers = group.ToList();

            foreach (var container in groupContainers)
            {
                var serviceName = ResolveServiceName(groupKey, groupContainers.Count, container, adoptedServiceNames);
                adoptedServiceNames.Add(serviceName);

                var serviceObj = await BuildServiceConfigAsync(
                    node,
                    container,
                    serviceName,
                    masterKey,
                    options,
                    ct).ConfigureAwait(false);

                servicesObj[serviceName] = serviceObj;
            }
        }
    }

    private async Task<(IContainerRuntime Runtime, string SocketPath)> ResolveActiveRuntimeAsync(INodeConnection node, CancellationToken ct)
    {
        foreach (var runtime in _runtimes)
        {
            try
            {
                var info = await runtime.ProbeAsync(node, ct).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(info.SocketPath))
                {
                    return (runtime, info.SocketPath);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Try next runtime
            }
        }

        throw new InvalidOperationException($"No active container runtime found on node '{node.NodeName}'.");
    }

    private static async Task<JsonObject?> QueryContainerDetailsAsync(HttpClient client, string containerId, CancellationToken ct)
    {
        using var response = await client.GetAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{Uri.EscapeDataString(containerId)}/json"),
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        return JsonNode.Parse(content) as JsonObject;
    }

    private ConfigTree InitializeConfigTree(AdoptionOptions options, string format)
    {
        var mainConfigFile = Path.Combine(options.OutDir, $"starry.{format}");
        if (File.Exists(mainConfigFile))
        {
            try
            {
                return _configEngine.Load(options.OutDir);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Fallback to fresh tree
            }
        }

        return new ConfigTree();
    }

    private static void EnsureClusterMetadata(ConfigTree tree, AdoptionOptions options)
    {
        if (!tree.Root.ContainsKey("version"))
        {
            tree.Root["version"] = "1.0";
        }

        if (!tree.Root.ContainsKey("cluster_name"))
        {
            tree.Root["cluster_name"] = options.ClusterName;
        }

        if (!tree.Root.ContainsKey("mesh"))
        {
            tree.Root["mesh"] = new JsonObject
            {
                ["cidr"] = "10.99.0.0/16",
                ["port"] = 51820,
                ["keepalive"] = 25
            };
        }
    }

    private static void RegisterNodeInConfig(ConfigTree tree, INodeConnection node, IContainerRuntime runtime)
    {
        if (!tree.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
        {
            nodesObj = [];
            tree.Root["nodes"] = nodesObj;
        }

        var nodeEntry = new JsonObject
        {
            ["host"] = node.Host,
            ["port"] = node.Port,
            ["engine"] = runtime.RuntimeName
        };

        if (!string.IsNullOrWhiteSpace(node.DistroId))
        {
            nodeEntry["distro"] = node.DistroId;
        }

        nodesObj[node.NodeName] = nodeEntry;
    }

    private static string ResolveServiceName(
        string groupKey,
        int groupCount,
        DiscoveredContainerInfo container,
        HashSet<string> existingNames)
    {
        string candidateName;

        if (groupCount == 1 && string.IsNullOrWhiteSpace(container.ComposeService))
        {
            candidateName = groupKey;
        }
        else if (!string.IsNullOrWhiteSpace(container.ComposeService))
        {
            var svc = container.ComposeService;
            if (string.Equals(svc, groupKey, StringComparison.OrdinalIgnoreCase) ||
                svc.StartsWith($"{groupKey}-", StringComparison.OrdinalIgnoreCase))
            {
                candidateName = svc;
            }
            else
            {
                candidateName = $"{groupKey}-{svc}";
            }
        }
        else
        {
            candidateName = container.CleanName;
        }

        var uniqueName = candidateName;
        var suffix = 2;
        while (existingNames.Contains(uniqueName))
        {
            uniqueName = $"{candidateName}-{suffix}";
            suffix++;
        }

        return uniqueName;
    }

    private async Task<JsonObject> BuildServiceConfigAsync(
        INodeConnection node,
        DiscoveredContainerInfo container,
        string serviceName,
        byte[] masterKey,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var priority = DetectPriority(container.State.Image, serviceName);
        var envObj = ExtractAndEncryptEnvironment(container, serviceName, masterKey);
        var (filesObj, volumesArr) = await ProcessMountsAsync(node, container, serviceName, options, ct).ConfigureAwait(false);
        var portsArr = ReconstructPortBindings(container);

        var serviceObj = new JsonObject
        {
            ["image"] = container.State.Image,
            ["priority"] = priority,
            ["node"] = node.NodeName
        };

        if (portsArr.Count > 0)
        {
            serviceObj["ports"] = portsArr;
        }

        if (volumesArr.Count > 0)
        {
            serviceObj["volumes"] = volumesArr;
        }

        if (envObj.Count > 0)
        {
            serviceObj["env"] = envObj;
        }

        if (filesObj.Count > 0)
        {
            serviceObj["files"] = filesObj;
        }

        var labelsObj = new JsonObject();
        foreach (var (k, v) in container.State.Labels)
        {
            labelsObj[k] = v;
        }

        labelsObj["managed-by"] = "starry";
        labelsObj["starry.priority"] = priority.ToString(CultureInfo.InvariantCulture);
        serviceObj["labels"] = labelsObj;

        return serviceObj;
    }

    private static int DetectPriority(string image, string serviceName)
    {
        var upperImage = image.ToUpperInvariant();
        var upperName = serviceName.ToUpperInvariant();

        if (upperImage.Contains("REGISTRY", StringComparison.Ordinal) || upperName.Contains("REGISTRY", StringComparison.Ordinal))
        {
            return 0;
        }

        if (DbKeywords.Any(kw => upperImage.Contains(kw, StringComparison.Ordinal) || upperName.Contains(kw, StringComparison.Ordinal)))
        {
            return 10;
        }

        if (ProxyKeywords.Any(kw => upperImage.Contains(kw, StringComparison.Ordinal) || upperName.Contains(kw, StringComparison.Ordinal)))
        {
            return 90;
        }

        return 50;
    }

    private JsonObject ExtractAndEncryptEnvironment(
        DiscoveredContainerInfo container,
        string serviceName,
        byte[] masterKey)
    {
        var envObj = new JsonObject();
        if (container.Details["Config"]?["Env"] is not JsonArray envArr)
        {
            return envObj;
        }

        foreach (var envNode in envArr)
        {
            if (envNode is null)
            {
                continue;
            }

            var envStr = envNode.ToString();
            var eqIdx = envStr.IndexOf('=', StringComparison.Ordinal);
            if (eqIdx <= 0)
            {
                continue;
            }

            var key = envStr[..eqIdx].Trim();
            var val = envStr[(eqIdx + 1)..];

            if (IsSecretKey(key))
            {
                if (_cryptoService.TryParseEncrypted(val, out _))
                {
                    envObj[key] = val;
                }
                else
                {
                    var canonicalPath = $"services.{serviceName}.env.{key}";
                    var envelope = _cryptoService.Encrypt(Encoding.UTF8.GetBytes(val), masterKey, canonicalPath);
                    envObj[key] = envelope.FormatEncrypted();
                }
            }
            else
            {
                envObj[key] = val;
            }
        }

        return envObj;
    }

    private static bool IsSecretKey(string key)
    {
        return SecretKeyIndicators.Any(indicator => key.Contains(indicator, StringComparison.OrdinalIgnoreCase));
    }

    private static (List<string> Binds, List<string> Volumes) ExtractRawMounts(DiscoveredContainerInfo container)
    {
        var rawBinds = new List<string>();
        var volumes = new List<string>();

        if (container.Details["HostConfig"]?["Binds"] is JsonArray bindsArr)
        {
            rawBinds.AddRange(bindsArr
                .Where(b => b != null && !string.IsNullOrWhiteSpace(b.ToString()))
                .Select(b => b!.ToString().Trim()));
        }

        if (container.Details["Mounts"] is JsonArray mountsArr)
        {
            PopulateMounts(mountsArr, rawBinds, volumes);
        }

        return (rawBinds, volumes);
    }

    private static void PopulateMounts(JsonArray mountsArr, List<string> rawBinds, List<string> volumes)
    {
        foreach (var m in mountsArr)
        {
            if (m is not JsonObject mObj)
            {
                continue;
            }

            var type = mObj["Type"]?.ToString();
            var source = mObj["Source"]?.ToString();
            var destination = mObj["Destination"]?.ToString();
            var mode = mObj["Mode"]?.ToString();

            if (string.Equals(type, "bind", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(source) &&
                !string.IsNullOrWhiteSpace(destination))
            {
                var entry = !string.IsNullOrWhiteSpace(mode)
                    ? $"{source}:{destination}:{mode}"
                    : $"{source}:{destination}";
                if (!rawBinds.Contains(entry))
                {
                    rawBinds.Add(entry);
                }
            }
            else if (string.Equals(type, "volume", StringComparison.OrdinalIgnoreCase) &&
                     !string.IsNullOrWhiteSpace(destination))
            {
                var volName = mObj["Name"]?.ToString() ?? source;
                if (!string.IsNullOrWhiteSpace(volName))
                {
                    volumes.Add($"{volName}:{destination}");
                }
            }
        }
    }

    private static async Task<(JsonObject Files, JsonArray Volumes)> ProcessMountsAsync(
        INodeConnection node,
        DiscoveredContainerInfo container,
        string serviceName,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var filesObj = new JsonObject();
        var volumesArr = new JsonArray();
        var processed = new HashSet<string>(StringComparer.Ordinal);

        var (rawBinds, namedVolumes) = ExtractRawMounts(container);

        foreach (var vol in namedVolumes.Where(processed.Add))
        {
            volumesArr.Add(vol);
        }

        foreach (var bindEntry in rawBinds)
        {
            await ProcessSingleBindAsync(
                node,
                bindEntry,
                serviceName,
                options,
                filesObj,
                volumesArr,
                processed,
                ct).ConfigureAwait(false);
        }

        return (filesObj, volumesArr);
    }

    private static async Task ProcessSingleBindAsync(
        INodeConnection node,
        string bindEntry,
        string serviceName,
        AdoptionOptions options,
        JsonObject filesObj,
        JsonArray volumesArr,
        HashSet<string> processed,
        CancellationToken ct)
    {
        var parts = bindEntry.Split(':');
        if (parts.Length < 2)
        {
            if (processed.Add(bindEntry))
            {
                volumesArr.Add(bindEntry);
            }
            return;
        }

        var hostSource = parts[0];
        var containerDest = parts[1];
        var mode = parts.Length > 2 ? parts[2] : string.Empty;

        if (!processed.Add($"{hostSource}:{containerDest}"))
        {
            return;
        }

        var isFile = await CheckIsRemoteFileAsync(node, hostSource, ct).ConfigureAwait(false);
        if (isFile && options.PullFiles)
        {
            var pulled = await TryPullRemoteFileAsync(node, hostSource, containerDest, serviceName, options.FilesDir, ct).ConfigureAwait(false);
            if (pulled != null)
            {
                filesObj[containerDest] = pulled;
                return;
            }
        }

        var volEntry = !string.IsNullOrEmpty(mode) ? $"{hostSource}:{containerDest}:{mode}" : $"{hostSource}:{containerDest}";
        volumesArr.Add(volEntry);
    }

    private static async Task<bool> CheckIsRemoteFileAsync(INodeConnection node, string hostSource, CancellationToken ct)
    {
        try
        {
            var testResult = await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-f", hostSource]
            }, ct).ConfigureAwait(false);

            return testResult.IsSuccess;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Path.HasExtension(hostSource);
        }
    }

    private static async Task<string?> TryPullRemoteFileAsync(
        INodeConnection node,
        string hostSource,
        string containerDest,
        string serviceName,
        string filesDir,
        CancellationToken ct)
    {
        var catResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "cat",
            Arguments = [hostSource]
        }, ct).ConfigureAwait(false);

        if (!catResult.IsSuccess)
        {
            return null;
        }

        var fileName = Path.GetFileName(hostSource);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Path.GetFileName(containerDest);
        }

        var serviceFilesDir = Path.Combine(filesDir, serviceName);
        Directory.CreateDirectory(serviceFilesDir);
        var localFilePath = Path.Combine(serviceFilesDir, fileName);

        await File.WriteAllTextAsync(localFilePath, catResult.StandardOutput, ct).ConfigureAwait(false);
        return $"files/{serviceName}/{fileName}";
    }

    private static JsonArray ReconstructPortBindings(DiscoveredContainerInfo container)
    {
        var portsArr = new JsonArray();
        var portRoutes = new HashSet<string>(StringComparer.Ordinal);

        JsonObject? portBindings = null;
        if (container.Details["HostConfig"]?["PortBindings"] is JsonObject hb)
        {
            portBindings = hb;
        }
        else if (container.Details["NetworkSettings"]?["Ports"] is JsonObject np)
        {
            portBindings = np;
        }

        if (portBindings is null)
        {
            return portsArr;
        }

        foreach (var (containerPortProto, bindingsVal) in portBindings)
        {
            if (bindingsVal is not JsonArray bArr || bArr.Count == 0)
            {
                continue;
            }

            var slashIdx = containerPortProto.IndexOf('/', StringComparison.Ordinal);
            var containerPort = slashIdx > 0 ? containerPortProto[..slashIdx] : containerPortProto;
            var proto = slashIdx > 0 ? containerPortProto[(slashIdx + 1)..] : "tcp";

            var validBindings = bArr
                .OfType<JsonObject>()
                .Where(b => b.TryGetPropertyValue("HostPort", out var hp) && hp != null && !string.IsNullOrWhiteSpace(hp.ToString()));

            foreach (var bObj in validBindings)
            {
                var hostPort = bObj["HostPort"]!.ToString().Trim();
                var route = string.Equals(proto, "udp", StringComparison.OrdinalIgnoreCase)
                    ? $"{hostPort}:{containerPort}/udp"
                    : $"{hostPort}:{containerPort}";

                if (portRoutes.Add(route))
                {
                    portsArr.Add(route);
                }
            }
        }

        return portsArr;
    }

    private static async Task<string> FindOrCreateMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        var existingNetId = await FindExistingMeshNetworkAsync(client, ct).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(existingNetId))
        {
            return existingNetId;
        }

        return await CreateMeshNetworkAsync(client, ct).ConfigureAwait(false);
    }

    private static async Task<string?> FindExistingMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        using var netListRes = await client.GetAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks"),
            ct).ConfigureAwait(false);

        if (!netListRes.IsSuccessStatusCode)
        {
            return null;
        }

        var netListJson = await netListRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        if (JsonNode.Parse(netListJson) is not JsonArray networks)
        {
            return null;
        }

        foreach (var netNode in networks)
        {
            if (netNode is JsonObject netObj &&
                netObj.TryGetPropertyValue("Name", out var nameVal) &&
                nameVal != null)
            {
                var name = nameVal.ToString();
                if (string.Equals(name, "starry", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "starry0", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "starry-mesh", StringComparison.OrdinalIgnoreCase))
                {
                    return netObj["Id"]?.ToString();
                }
            }
        }

        return null;
    }

    private static async Task<string> CreateMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        var createNetPayload = new JsonObject
        {
            ["Name"] = "starry",
            ["CheckDuplicate"] = true,
            ["Driver"] = "bridge",
            ["IPAM"] = new JsonObject
            {
                ["Config"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["Subnet"] = "10.99.0.0/16"
                    }
                }
            }
        };

        using var createContent = new StringContent(createNetPayload.ToJsonString(), Encoding.UTF8, "application/json");
        using var createRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks/create"),
            createContent,
            ct).ConfigureAwait(false);

        if (createRes.IsSuccessStatusCode)
        {
            var createJson = await createRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (JsonNode.Parse(createJson) is JsonObject createdObj)
            {
                var createdId = createdObj["Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(createdId))
                {
                    return createdId;
                }
            }
        }

        return "starry";
    }

    private static byte[] ResolveMasterKey(AdoptionOptions options)
    {
        if (options.MasterKey is { Length: CryptoService.KeySizeBytes } key)
        {
            return key;
        }

        var envKey = Environment.GetEnvironmentVariable("STARRY_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(envKey))
        {
            try
            {
                return CryptoService.ParseMasterKey(envKey);
            }
            catch (FormatException)
            {
                // Fallback to generating a fresh key
            }
        }

        return CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
    }

    private sealed record DiscoveredContainerInfo(ContainerState State, JsonObject Details)
    {
        public string CleanName => State.Name.TrimStart('/');

        public string? ComposeProject
        {
            get
            {
                if (Details["Config"]?["Labels"] is JsonObject labelsObj &&
                    labelsObj.TryGetPropertyValue("com.docker.compose.project", out var projVal) &&
                    projVal != null)
                {
                    return projVal.ToString().Trim();
                }

                return State.Labels.GetValueOrDefault("com.docker.compose.project")?.Trim();
            }
        }

        public string? ComposeService
        {
            get
            {
                if (Details["Config"]?["Labels"] is JsonObject labelsObj &&
                    labelsObj.TryGetPropertyValue("com.docker.compose.service", out var svcVal) &&
                    svcVal != null)
                {
                    return svcVal.ToString().Trim();
                }

                return State.Labels.GetValueOrDefault("com.docker.compose.service")?.Trim();
            }
        }
    }
}
