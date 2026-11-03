using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Containers;

/// <summary>
/// Base class for container engines communicating over Unix Domain Sockets using pure HttpClient.
/// </summary>
public abstract class BaseSocketContainerRuntime : IContainerRuntime
{
    private static readonly Uri BaseDockerUri = new("http://127.0.0.1/");
    private const string ApiVersionPrefix = "/v1.40";

    public abstract string RuntimeName { get; }
    protected abstract IReadOnlyList<string> CandidateSocketPaths { get; }

    public async Task<string> ResolveSocketPathAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        foreach (var candidate in CandidateSocketPaths)
        {
            var checkRes = await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-S", candidate]
            }, ct).ConfigureAwait(false);

            if (checkRes.IsSuccess)
            {
                return candidate;
            }
        }

        // Return first default if none exists yet
        return CandidateSocketPaths[0];
    }

    public async Task<RuntimeInfo> ProbeAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/version"), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var bodyStr = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var json = JsonNode.Parse(bodyStr) as JsonObject;
        var version = json?["Version"]?.ToString() ?? "unknown";
        var os = json?["Os"]?.ToString() ?? "linux";
        var arch = json?["Arch"]?.ToString() ?? "amd64";

        var isRootless = socketPath.Contains("/run/user/", StringComparison.OrdinalIgnoreCase);

        return new RuntimeInfo(
            Engine: RuntimeName,
            Version: version,
            OperatingSystem: os,
            Architecture: arch,
            IsRootless: isRootless,
            SocketPath: socketPath);
    }

    public async Task<IReadOnlyList<ContainerState>> ListContainersAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/json?all=true"), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var bodyStr = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var jsonArray = JsonNode.Parse(bodyStr) as JsonArray;
        var result = new List<ContainerState>();

        if (jsonArray == null)
        {
            return result;
        }

        foreach (var item in jsonArray)
        {
            if (item is not JsonObject obj)
            {
                continue;
            }

            var id = obj["Id"]?.ToString() ?? string.Empty;
            var namesArr = obj["Names"] as JsonArray;
            var name = namesArr?.Count > 0 ? namesArr[0]?.ToString().TrimStart('/') ?? string.Empty : id;
            var image = obj["Image"]?.ToString() ?? string.Empty;
            var status = obj["Status"]?.ToString() ?? string.Empty;
            var state = obj["State"]?.ToString() ?? string.Empty;
            var isRunning = string.Equals(state, "running", StringComparison.OrdinalIgnoreCase);

            var labelsDict = new Dictionary<string, string>(StringComparer.Ordinal);
            if (obj["Labels"] is JsonObject labelsObj)
            {
                foreach (var (k, v) in labelsObj)
                {
                    labelsDict[k] = v?.ToString() ?? string.Empty;
                }
            }

            result.Add(new ContainerState(
                Id: id,
                Name: name,
                Image: image,
                Status: status,
                IsRunning: isRunning,
                ExitCode: 0,
                CreatedAt: DateTime.UtcNow,
                Labels: labelsDict));
        }

        return result;
    }

    public async Task DeployContainerAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(spec);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        // Check if dynamic files changed
        var filesChanged = await SyncDynamicFilesAsync(node, spec, ct).ConfigureAwait(false);

        // Check if container already exists
        var existingContainers = await ListContainersAsync(node, ct).ConfigureAwait(false);
        var existing = existingContainers.FirstOrDefault(c => string.Equals(c.Name, spec.Name, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
        {
            var shouldRecreate = await CheckContainerDriftAsync(client, existing, spec, ct).ConfigureAwait(false);
            if (shouldRecreate)
            {
                await client.DeleteAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{existing.Id}?force=true"), ct).ConfigureAwait(false);
            }
            else
            {
                await ReconcileExistingContainerAsync(client, existing, filesChanged, ct).ConfigureAwait(false);
                return;
            }
        }

        // If VIP address is specified, ensure it is assigned to starry0 interface on the node
        if (!string.IsNullOrEmpty(spec.VipAddress) && System.Net.IPAddress.TryParse(spec.VipAddress, out _))
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"ip addr add {spec.VipAddress}/32 dev starry0 2>/dev/null || true"]
            }, ct).ConfigureAwait(false);
        }

        // Ensure image is pulled locally before creating container
        await EnsureImageAvailableAsync(client, spec.Image, ct).ConfigureAwait(false);

        // Create and start container
        var createPayload = BuildCreateContainerPayload(spec);
        using var createContent = new StringContent(createPayload.ToJsonString(), Encoding.UTF8, "application/json");

        using var createRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/create?name={Uri.EscapeDataString(spec.Name)}"),
            createContent,
            ct).ConfigureAwait(false);

        createRes.EnsureSuccessStatusCode();
        var createStr = await createRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var createJson = JsonNode.Parse(createStr) as JsonObject;
        var containerId = createJson?["Id"]?.ToString() ?? throw new InvalidOperationException("Failed to get created container Id.");

        await StartContainerAsync(client, containerId, ct).ConfigureAwait(false);
    }

    private static async Task ReconcileExistingContainerAsync(HttpClient client, ContainerState existing, bool filesChanged, CancellationToken ct)
    {
        if (existing.IsRunning && filesChanged)
        {
            await SendSignalAsync(client, existing.Id, "SIGHUP", ct).ConfigureAwait(false);
            return;
        }

        if (!existing.IsRunning)
        {
            await StartContainerAsync(client, existing.Id, ct).ConfigureAwait(false);
        }
    }

    private static async Task<bool> CheckContainerDriftAsync(HttpClient client, ContainerState existing, ServiceSpec spec, CancellationToken ct)
    {
        try
        {
            var inspectRes = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{existing.Id}/json"), ct).ConfigureAwait(false);
            if (!inspectRes.IsSuccessStatusCode)
            {
                return false;
            }

            var inspectStr = await inspectRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var inspectJson = JsonNode.Parse(inspectStr) as JsonObject;
            var runningSpecHash = inspectJson?["Config"]?["Labels"]?["starry.spec.hash"]?.ToString();
            var runningImageId = inspectJson?["Image"]?.ToString();

            string? latestImageId = null;
            var imgRes = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/images/{Uri.EscapeDataString(spec.Image)}/json"), ct).ConfigureAwait(false);
            if (imgRes.IsSuccessStatusCode)
            {
                var imgStr = await imgRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                var imgJson = JsonNode.Parse(imgStr) as JsonObject;
                latestImageId = imgJson?["Id"]?.ToString();
            }

            var desiredSpecHash = ComputeSpecHash(spec);
            var specHashChanged = !string.IsNullOrEmpty(runningSpecHash) && !string.Equals(runningSpecHash, desiredSpecHash, StringComparison.Ordinal);
            var imageDigestChanged = !string.IsNullOrEmpty(latestImageId) && !string.IsNullOrEmpty(runningImageId) && !string.Equals(runningImageId, latestImageId, StringComparison.Ordinal);

            return specHashChanged || imageDigestChanged;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return false;
        }
    }

    public static string ComputeSpecHash(ServiceSpec spec)
    {
        ArgumentNullException.ThrowIfNull(spec);
        var sb = new StringBuilder();
        sb.Append(spec.Image).Append('|')
          .Append(spec.Command).Append('|')
          .Append(spec.Entrypoint).Append('|')
          .Append(spec.WorkingDir).Append('|')
          .Append(spec.User).Append('|')
          .Append(spec.Privileged).Append('|')
          .Append(spec.NetworkMode).Append('|')
          .Append(spec.ShmSize).Append('|')
          .Append(spec.MemLimit).Append('|')
          .Append(spec.Cpus).Append('|')
          .Append(spec.Egress).Append('|')
          .Append(string.Join(",", spec.Ports.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.Volumes.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.Networks.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.CapAdd.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.CapDrop.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.Devices.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.ExtraHosts.OrderBy(x => x, StringComparer.Ordinal))).Append('|')
          .Append(string.Join(",", spec.Environment.OrderBy(x => x.Key, StringComparer.Ordinal).Select(x => $"{x.Key}={x.Value}"))).Append('|')
          .Append(string.Join(",", spec.Files.OrderBy(x => x.Key, StringComparer.Ordinal).Select(x => $"{x.Key}={x.Value}")));

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
        return Convert.ToHexStringLower(hash);
    }

    public async Task StreamImagePeerToPeerAsync(
        INodeConnection source,
        INodeConnection target,
        string imageTag,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(imageTag);

        // Stream docker/podman save | zstd | ssh load through binary pipe
        using var pipeStream = new MemoryStream();

        var saveCmd = $"{RuntimeName} save {imageTag} | zstd -3";
        var saveSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", saveCmd],
            Timeout = TimeSpan.FromMinutes(30)
        };

        var sourceResult = await source.ExecuteStreamingCommandAsync(saveSpec, stdinStream: null, stdoutStream: pipeStream, ct).ConfigureAwait(false);
        if (!sourceResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to save image '{imageTag}' on source node '{source.NodeName}': {sourceResult.StandardError}");
        }

        pipeStream.Position = 0;
        var loadCmd = $"zstd -d | {RuntimeName} load";
        var targetSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", loadCmd],
            Timeout = TimeSpan.FromMinutes(30)
        };

        var targetResult = await target.ExecuteStreamingCommandAsync(targetSpec, stdinStream: pipeStream, stdoutStream: null, ct).ConfigureAwait(false);
        if (!targetResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to load image '{imageTag}' on target node '{target.NodeName}': {targetResult.StandardError}");
        }
    }

    private static async Task<bool> SyncDynamicFilesAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct)
    {
        if (spec.Files.Count == 0)
        {
            return false;
        }

        var anyFileChanged = false;
        var baseDir = $"/var/lib/starry/files/{spec.Name}";

        foreach (var (destPath, content) in spec.Files)
        {
            var hostFilePath = $"{baseDir}/{destPath.TrimStart('/')}";
            var hostDir = Path.GetDirectoryName(hostFilePath)?.Replace('\\', '/') ?? baseDir;
            var newHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));

            var checkHashSpec = new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"sha256sum {hostFilePath} 2>/dev/null | awk '{{print $1}}'"]
            };
            var hashRes = await node.ExecuteCommandAsync(checkHashSpec, ct).ConfigureAwait(false);
            var currentHash = hashRes.StandardOutput.Trim();

            if (!string.Equals(currentHash, newHash, StringComparison.OrdinalIgnoreCase))
            {
                anyFileChanged = true;
                var writeSpec = new ProcessSpec
                {
                    Executable = "sh",
                    Arguments = ["-c", $"mkdir -p {hostDir} && cat > {hostFilePath} && chmod 644 {hostFilePath}"],
                    StandardInputBytes = Encoding.UTF8.GetBytes(content)
                };
                await node.ExecuteCommandAsync(writeSpec, ct).ConfigureAwait(false);
            }
        }

        return anyFileChanged;
    }

    private static JsonObject BuildCreateContainerPayload(ServiceSpec spec)
    {
        var binds = new JsonArray();
        foreach (var vol in spec.Volumes)
        {
            binds.Add(JsonValue.Create(vol));
        }

        if (spec.Files.Count > 0)
        {
            var baseDir = $"/var/lib/starry/files/{spec.Name}";
            foreach (var destPath in spec.Files.Keys)
            {
                binds.Add(JsonValue.Create($"{baseDir}/{destPath.TrimStart('/')}:{destPath}:ro"));
            }
        }

        var env = new JsonArray();
        foreach (var (k, v) in spec.Environment)
        {
            env.Add(JsonValue.Create($"{k}={v}"));
        }

        var labels = new JsonObject();
        foreach (var (k, v) in spec.Labels)
        {
            labels[k] = v;
        }
        labels["managed-by"] = "starry";
        labels["starry.priority"] = spec.Priority.ToString(CultureInfo.InvariantCulture);
        labels["starry.spec.hash"] = ComputeSpecHash(spec);

        var (exposedPorts, portBindings) = BuildPortConfigurations(spec.Ports);
        var hostConfig = BuildHostConfig(spec, binds, portBindings);

        var payload = new JsonObject
        {
            ["Image"] = spec.Image,
            ["Env"] = env,
            ["Labels"] = labels,
            ["ExposedPorts"] = exposedPorts,
            ["HostConfig"] = hostConfig
        };

        ApplyContainerProcessSpecs(spec, payload);

        return payload;
    }

    private static void ApplyContainerProcessSpecs(ServiceSpec spec, JsonObject payload)
    {
        if (!string.IsNullOrEmpty(spec.Command))
        {
            payload["Cmd"] = new JsonArray(JsonValue.Create("sh"), JsonValue.Create("-c"), JsonValue.Create(spec.Command));
        }
        if (!string.IsNullOrEmpty(spec.WorkingDir)) payload["WorkingDir"] = spec.WorkingDir;
        if (!string.IsNullOrEmpty(spec.User)) payload["User"] = spec.User;
        if (!string.IsNullOrEmpty(spec.Hostname)) payload["Hostname"] = spec.Hostname;
        if (!string.IsNullOrEmpty(spec.Domainname)) payload["Domainname"] = spec.Domainname;
        if (!string.IsNullOrEmpty(spec.StopSignal)) payload["StopSignal"] = spec.StopSignal;
        if (spec.StopGracePeriod.HasValue) payload["StopTimeout"] = spec.StopGracePeriod.Value;
    }

    private static JsonObject BuildHostConfig(ServiceSpec spec, JsonArray binds, JsonObject portBindings)
    {
        var hostConfig = new JsonObject
        {
            ["Binds"] = binds,
            ["PortBindings"] = portBindings,
            ["RestartPolicy"] = new JsonObject { ["Name"] = spec.Restart },
            ["Privileged"] = spec.Privileged
        };

        ApplyCapAddDrop(spec, hostConfig);
        ApplyResourceLimits(spec, hostConfig);

        if (!string.IsNullOrEmpty(spec.NetworkMode)) hostConfig["NetworkMode"] = spec.NetworkMode;
        if (spec.ShmSize.HasValue) hostConfig["ShmSize"] = spec.ShmSize.Value;

        if (spec.Sysctls.Count > 0)
        {
            var sysctls = new JsonObject();
            foreach (var (k, v) in spec.Sysctls) sysctls[k] = v;
            hostConfig["Sysctls"] = sysctls;
        }

        return hostConfig;
    }

    private static void ApplyCapAddDrop(ServiceSpec spec, JsonObject hostConfig)
    {
        if (spec.CapAdd.Count > 0)
        {
            var capAdd = new JsonArray();
            foreach (var c in spec.CapAdd) capAdd.Add(JsonValue.Create(c));
            hostConfig["CapAdd"] = capAdd;
        }

        if (spec.CapDrop.Count > 0)
        {
            var capDrop = new JsonArray();
            foreach (var c in spec.CapDrop) capDrop.Add(JsonValue.Create(c));
            hostConfig["CapDrop"] = capDrop;
        }
    }

    private static void ApplyResourceLimits(ServiceSpec spec, JsonObject hostConfig)
    {
        var memBytes = ParseMemoryLimitBytes(spec.MemLimit);
        if (memBytes.HasValue) hostConfig["Memory"] = memBytes.Value;

        if (spec.Cpus.HasValue)
        {
            hostConfig["NanoCpus"] = (long)(spec.Cpus.Value * 1_000_000_000);
        }
    }

    private static long? ParseMemoryLimitBytes(string? memLimit)
    {
        if (string.IsNullOrWhiteSpace(memLimit)) return null;
        var trimmed = memLimit.Trim();
        if ((trimmed.EndsWith('g') || trimmed.EndsWith('G')) && long.TryParse(trimmed[..^1], CultureInfo.InvariantCulture, out var g))
            return g * 1024 * 1024 * 1024;
        if ((trimmed.EndsWith('m') || trimmed.EndsWith('M')) && long.TryParse(trimmed[..^1], CultureInfo.InvariantCulture, out var m))
            return m * 1024 * 1024;
        if ((trimmed.EndsWith('k') || trimmed.EndsWith('K')) && long.TryParse(trimmed[..^1], CultureInfo.InvariantCulture, out var k))
            return k * 1024;
        if (long.TryParse(trimmed, CultureInfo.InvariantCulture, out var b))
            return b;
        return null;
    }

    private static (JsonObject ExposedPorts, JsonObject PortBindings) BuildPortConfigurations(IReadOnlyList<string> ports)
    {
        var exposedPorts = new JsonObject();
        var portBindings = new JsonObject();

        foreach (var portStr in ports)
        {
            var parts = portStr.Split(':');
            if (parts.Length == 2)
            {
                var hostPort = parts[0].Trim();
                var containerPart = parts[1].Trim();
                var proto = "tcp";
                var containerPort = containerPart;
                if (containerPart.Contains('/', StringComparison.Ordinal))
                {
                    var sub = containerPart.Split('/');
                    containerPort = sub[0];
                    proto = string.Equals(sub[1], "udp", StringComparison.OrdinalIgnoreCase) ? "udp" : "tcp";
                }

                var containerKey = $"{containerPort}/{proto}";
                exposedPorts[containerKey] = new JsonObject();

                var bindingArr = new JsonArray
                {
                    new JsonObject
                    {
                        ["HostIp"] = "0.0.0.0",
                        ["HostPort"] = hostPort
                    }
                };
                portBindings[containerKey] = bindingArr;
            }
        }

        return (exposedPorts, portBindings);
    }

    private static async Task EnsureImageAvailableAsync(HttpClient client, string image, CancellationToken ct)
    {
        var inspectUri = new Uri(BaseDockerUri, $"{ApiVersionPrefix}/images/{Uri.EscapeDataString(image)}/json");
        using var inspectRes = await client.GetAsync(inspectUri, ct).ConfigureAwait(false);
        if (inspectRes.IsSuccessStatusCode)
        {
            return;
        }

        var pullUri = new Uri(BaseDockerUri, $"{ApiVersionPrefix}/images/create?fromImage={Uri.EscapeDataString(image)}");
        using var pullRes = await client.PostAsync(pullUri, content: null, ct).ConfigureAwait(false);
        if (pullRes.IsSuccessStatusCode)
        {
            await pullRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        }
    }

    private static async Task StartContainerAsync(HttpClient client, string containerId, CancellationToken ct)
    {
        using var response = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{containerId}/start"),
            content: null,
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotModified)
        {
            var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new InvalidOperationException($"Failed to start container '{containerId}': {err}");
        }
    }

    private static async Task SendSignalAsync(HttpClient client, string containerId, string signal, CancellationToken ct)
    {
        using var response = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{containerId}/kill?signal={signal}"),
            content: null,
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new InvalidOperationException($"Failed to send signal '{signal}' to container '{containerId}': {err}");
        }
    }
}
