using System.Formats.Tar;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
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
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(bundleOutputPath);

        var configEngine = engine ?? new VirtualConfigEngine();
        var tempStagingDir = Path.Combine(Path.GetTempPath(), $"starry-bundle-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempStagingDir);

        try
        {
            // Split configuration into modular folder hierarchy inside staging dir
            configEngine.Split(tree, tempStagingDir, "yaml");

            // Write manifest metadata
            var clusterName = tree.Root["cluster_name"]?.ToString() ?? "starry-fleet";
            var nodesList = new List<string>();
            if (tree.Root["nodes"] is JsonObject nodesObj)
            {
                nodesList.AddRange(nodesObj.Select(kv => kv.Key));
            }

            var manifest = new BundleManifest(
                ClusterName: clusterName,
                CreatedAt: DateTime.UtcNow,
                StarryVersion: "0.1.0",
                Nodes: nodesList);

            var manifestJson = JsonSerializer.Serialize(manifest, IndentedJsonOptions);
            File.WriteAllText(Path.Combine(tempStagingDir, "manifest.json"), manifestJson);

            // Pack directory into tar.gz
            var outDir = Path.GetDirectoryName(bundleOutputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using var fileStream = File.Create(bundleOutputPath);
            using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            TarFile.CreateFromDirectory(tempStagingDir, gzipStream, includeBaseDirectory: false);
        }
        finally
        {
            if (Directory.Exists(tempStagingDir))
            {
                Directory.Delete(tempStagingDir, recursive: true);
            }
        }
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
