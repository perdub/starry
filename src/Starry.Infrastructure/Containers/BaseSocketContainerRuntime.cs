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
            if (existing.IsRunning && filesChanged)
            {
                // Send SIGHUP for hot configuration reload
                await SendSignalAsync(client, existing.Id, "SIGHUP", ct).ConfigureAwait(false);
                return;
            }

            if (existing.IsRunning)
            {
                return; // Already desired state
            }

            // Start existing stopped container
            await StartContainerAsync(client, existing.Id, ct).ConfigureAwait(false);
            return;
        }

        // If VIP address is specified, ensure it is assigned to starry0 interface on the node
        if (!string.IsNullOrEmpty(spec.VipAddress))
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"ip addr add {spec.VipAddress}/32 dev starry0 2>/dev/null || true"]
            }, ct).ConfigureAwait(false);
        }

        // Pull / verify image, then create and start container
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

        // Mount dynamic files directory
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

        var (exposedPorts, portBindings) = BuildPortConfigurations(spec.Ports);

        var payload = new JsonObject
        {
            ["Image"] = spec.Image,
            ["Env"] = env,
            ["Labels"] = labels,
            ["ExposedPorts"] = exposedPorts,
            ["HostConfig"] = new JsonObject
            {
                ["Binds"] = binds,
                ["PortBindings"] = portBindings,
                ["RestartPolicy"] = new JsonObject { ["Name"] = "unless-stopped" }
            }
        };

        return payload;
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
