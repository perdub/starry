using System.Formats.Tar;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Configuration;

namespace Starry.Infrastructure.Backups;

/// <summary>
/// Disaster recovery capsule exporter and importer packing entire fleet configuration
/// and state into a single portable archive for quick bootstrap on new IPs.
/// </summary>
public static class FleetBundleService
{
    private static readonly JsonSerializerOptions IndentedJsonOptions = new()
    {
        WriteIndented = true
    };

    public static void ExportBundle(
        ConfigTree tree,
        string bundleOutputPath,
        VirtualConfigEngine? engine = null)
    {
        ExportBundleAsync(tree, bundleOutputPath, nodes: null, engine).GetAwaiter().GetResult();
    }

    public static async Task ExportBundleAsync(
        ConfigTree tree,
        string bundleOutputPath,
        IReadOnlyList<INodeConnection>? nodes = null,
        VirtualConfigEngine? engine = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(bundleOutputPath);

        var configEngine = engine ?? new VirtualConfigEngine();
        var tempStagingDir = Path.Combine(Path.GetTempPath(), $"starry-bundle-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempStagingDir);

        try
        {
            configEngine.Split(tree, tempStagingDir, "yaml");
            WriteBundleManifest(tree, tempStagingDir);

            if (nodes != null && nodes.Count > 0 && tree.Root["volumes"] is JsonObject volumesObj)
            {
                var volDir = Path.Combine(tempStagingDir, "volumes");
                Directory.CreateDirectory(volDir);
                foreach (var (volName, _) in volumesObj)
                {
                    var targetNode = nodes[0];
                    var volTarPath = Path.Combine(volDir, $"{volName}.tar.gz");
                    using var fs = File.Create(volTarPath);
                    await targetNode.ExecuteStreamingCommandAsync(new ProcessSpec
                    {
                        Executable = "sh",
                        Arguments = ["-c", $"tar -C '/var/lib/docker/volumes/{volName}/_data' -czf - . 2>/dev/null || true"]
                    }, null, fs, ct).ConfigureAwait(false);
                }
            }

            var outDir = Path.GetDirectoryName(bundleOutputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using var fileStream = File.Create(bundleOutputPath);
            using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            await TarFile.CreateFromDirectoryAsync(tempStagingDir, gzipStream, includeBaseDirectory: false, ct).ConfigureAwait(false);
        }
        finally
        {
            if (Directory.Exists(tempStagingDir))
            {
                Directory.Delete(tempStagingDir, recursive: true);
            }
        }
    }

    private static void WriteBundleManifest(ConfigTree tree, string tempStagingDir)
    {
        var clusterName = tree.Root["cluster_name"]?.ToString() ?? "starry-fleet";
        var nodesList = new List<string>();
        if (tree.Root["nodes"] is JsonObject nodesObj)
        {
            nodesList.AddRange(nodesObj.Select(kv => kv.Key));
        }

        var manifestObj = new JsonObject
        {
            ["cluster_name"] = clusterName,
            ["created_at"] = DateTime.UtcNow.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
            ["starry_version"] = "0.1.0"
        };

        var nodesArr = new JsonArray();
        foreach (var node in nodesList)
        {
            nodesArr.Add(JsonValue.Create(node));
        }
        manifestObj["nodes"] = nodesArr;

        var manifestJson = manifestObj.ToJsonString(IndentedJsonOptions);
        File.WriteAllText(Path.Combine(tempStagingDir, "manifest.json"), manifestJson);
    }

    public static ConfigTree ImportBundle(
        string bundleArchivePath,
        string targetConfigDir,
        IReadOnlyDictionary<string, string>? ipOverrides = null,
        VirtualConfigEngine? engine = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bundleArchivePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetConfigDir);

        if (!File.Exists(bundleArchivePath))
        {
            throw new FileNotFoundException($"Bundle archive not found: {bundleArchivePath}", bundleArchivePath);
        }

        var configEngine = engine ?? new VirtualConfigEngine();

        if (!Directory.Exists(targetConfigDir))
        {
            Directory.CreateDirectory(targetConfigDir);
        }

        // Extract tar.gz into target directory
        using (var fileStream = File.OpenRead(bundleArchivePath))
        using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        {
            TarFile.ExtractToDirectory(gzipStream, targetConfigDir, overwriteFiles: true);
        }

        // Load extracted configuration
        var tree = configEngine.Load(targetConfigDir);

        // Apply IP overrides if provided (for disaster recovery on new VPS instances)
        if (ipOverrides != null && ipOverrides.Count > 0 && tree.Root["nodes"] is JsonObject nodesObj)
        {
            foreach (var (nodeName, newIp) in ipOverrides)
            {
                if (nodesObj.TryGetPropertyValue(nodeName, out var nodeVal) && nodeVal is JsonObject nodeObj)
                {
                    nodeObj["host"] = newIp;
                }
            }

            // Persist updated IPs back to disk
            configEngine.Split(tree, targetConfigDir, "yaml");
        }

        return tree;
    }
}
