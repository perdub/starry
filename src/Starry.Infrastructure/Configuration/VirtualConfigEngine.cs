using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Virtual Configuration Engine for loading, merging, packing, splitting,
/// formatting, and validating multi-file configuration trees with Source Map tracking.
/// </summary>
public sealed class VirtualConfigEngine
{
    private readonly List<IConfigFormatProvider> _providers = [];

    public IReadOnlyList<IConfigFormatProvider> Providers => _providers;

    public VirtualConfigEngine(IEnumerable<IConfigFormatProvider>? providers = null)
    {
        if (providers != null)
        {
            _providers.AddRange(providers);
        }
        else
        {
            _providers.Add(new YamlConfigFormatProvider());
            _providers.Add(new JsonConfigFormatProvider());
            _providers.Add(new TomlConfigFormatProvider());
        }
    }

    public ConfigTree Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (File.Exists(path))
        {
            return LoadFile(path);
        }

        if (Directory.Exists(path))
        {
            return LoadDirectory(path);
        }

        throw new FileNotFoundException($"Configuration path does not exist: {path}", path);
    }

    private ConfigTree LoadFile(string filePath)
    {
        var provider = FindProviderForFile(filePath);
        var bytes = File.ReadAllBytes(filePath);
        var parsed = provider.Parse(bytes, filePath);

        var tree = new ConfigTree();
        if (parsed is JsonObject obj)
        {
            PopulateObjectIntoTree(tree, obj, string.Empty, filePath);
        }
        else
        {
            tree.SetNode("root", parsed, new SourceMapEntry("root", filePath));
        }

        return tree;
    }

    private ConfigTree LoadDirectory(string dirPath)
    {
        var tree = new ConfigTree();
        var supportedExtensions = _providers.SelectMany(p => p.SupportedExtensions).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f)))
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var file in files)
        {
            var relative = Path.GetRelativePath(dirPath, file).Replace('\\', '/');
            var provider = FindProviderForFile(file);
            var bytes = File.ReadAllBytes(file);
            var parsed = provider.Parse(bytes, relative);

            var pathWithoutExt = Path.Combine(
                Path.GetDirectoryName(relative) ?? string.Empty,
                Path.GetFileNameWithoutExtension(relative)).Replace('\\', '/');

            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(relative);

            if (string.Equals(fileNameWithoutExt, "starry", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(fileNameWithoutExt, "config", StringComparison.OrdinalIgnoreCase))
            {
                // Root-level configuration file merges directly into root
                if (parsed is JsonObject rootObj)
                {
                    PopulateObjectIntoTree(tree, rootObj, string.Empty, relative);
                }
            }
            else
            {
                // Hierarchy mapping: nodes/bocchi.yaml -> root.nodes.bocchi
                var canonicalPrefix = pathWithoutExt.Replace('/', '.');
                if (parsed is JsonObject obj)
                {
                    PopulateObjectIntoTree(tree, obj, canonicalPrefix, relative);
                }
                else
                {
                    tree.SetNode(canonicalPrefix, parsed, new SourceMapEntry(canonicalPrefix, relative));
                }
            }
        }

        return tree;
    }

    private static void PopulateObjectIntoTree(ConfigTree tree, JsonObject obj, string prefix, string filePath)
    {
        foreach (var (propName, propVal) in obj)
        {
            var currentPath = string.IsNullOrEmpty(prefix) ? propName : $"{prefix}.{propName}";
            if (propVal is JsonObject childObj)
            {
                PopulateObjectIntoTree(tree, childObj, currentPath, filePath);
            }
            else
            {
                // Clone the node so it can be re-parented into the tree
                var clonedVal = propVal != null ? JsonNode.Parse(propVal.ToJsonString()) : null;
                tree.SetNode(currentPath, clonedVal, new SourceMapEntry(currentPath, filePath));
            }
        }
    }

    public void Pack(ConfigTree tree, string targetFilePath, string format = "yaml")
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFilePath);

        var provider = FindProviderByName(format);
        var serialized = provider.Serialize(tree.Root);

        var dir = Path.GetDirectoryName(targetFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(targetFilePath, serialized);
    }

    public void Split(ConfigTree tree, string targetDirectoryPath, string format = "yaml")
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetDirectoryPath);

        if (!Directory.Exists(targetDirectoryPath))
        {
            Directory.CreateDirectory(targetDirectoryPath);
        }

        var provider = FindProviderByName(format);
        var ext = provider.SupportedExtensions[0];

        var rootCopy = new JsonObject();

        foreach (var (key, val) in tree.Root)
        {
            if (string.Equals(key, "nodes", StringComparison.OrdinalIgnoreCase) && val is JsonObject nodesObj)
            {
                var nodesDir = Path.Combine(targetDirectoryPath, "nodes");
                Directory.CreateDirectory(nodesDir);
                foreach (var (nodeName, nodeVal) in nodesObj)
                {
                    if (nodeVal != null)
                    {
                        var filePath = Path.Combine(nodesDir, $"{nodeName}{ext}");
                        File.WriteAllText(filePath, provider.Serialize(nodeVal));
                    }
                }
            }
            else if (string.Equals(key, "services", StringComparison.OrdinalIgnoreCase) && val is JsonObject servicesObj)
            {
                var servicesDir = Path.Combine(targetDirectoryPath, "services");
                Directory.CreateDirectory(servicesDir);
                foreach (var (svcName, svcVal) in servicesObj)
                {
                    if (svcVal != null)
                    {
                        var filePath = Path.Combine(servicesDir, $"{svcName}{ext}");
                        File.WriteAllText(filePath, provider.Serialize(svcVal));
                    }
                }
            }
            else
            {
                if (val != null)
                {
                    rootCopy[key] = JsonNode.Parse(val.ToJsonString());
                }
            }
        }

        var mainConfigPath = Path.Combine(targetDirectoryPath, $"starry{ext}");
        File.WriteAllText(mainConfigPath, provider.Serialize(rootCopy));
    }

    public string Format(string content, string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var provider = FindProviderByName(format);
        var parsed = provider.Parse(System.Text.Encoding.UTF8.GetBytes(content), "inline");
        return provider.Serialize(parsed);
    }

    public static string GenerateJsonSchema()
    {
        var schemaObject = new JsonObject
        {
            ["$schema"] = "https://json-schema.org/draft/2020-12/schema",
            ["title"] = "StarryClusterConfiguration",
            ["type"] = "object",
            ["properties"] = new JsonObject
            {
                ["cluster_name"] = new JsonObject { ["type"] = "string" },
                ["version"] = new JsonObject { ["type"] = "string" },
                ["nodes"] = new JsonObject
                {
                    ["type"] = "object",
                    ["additionalProperties"] = new JsonObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JsonObject
                        {
                            ["host"] = new JsonObject { ["type"] = "string" },
                            ["port"] = new JsonObject { ["type"] = "integer", ["default"] = 22 },
                            ["user"] = new JsonObject { ["type"] = "string", ["default"] = "root" },
                            ["ssh_key"] = new JsonObject { ["type"] = "string" },
                            ["vpn_ip"] = new JsonObject { ["type"] = "string" },
                            ["is_public"] = new JsonObject { ["type"] = "boolean", ["default"] = false }
                        },
                        ["required"] = new JsonArray { "host" }
                    }
                },
                ["services"] = new JsonObject
                {
                    ["type"] = "object",
                    ["additionalProperties"] = new JsonObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JsonObject
                        {
                            ["image"] = new JsonObject { ["type"] = "string" },
                            ["priority"] = new JsonObject { ["type"] = "integer", ["default"] = 50 },
                            ["vip"] = new JsonObject { ["type"] = "string" },
                            ["node"] = new JsonObject { ["type"] = "string" },
                            ["ports"] = new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string" } },
                            ["volumes"] = new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string" } },
                            ["env"] = new JsonObject { ["type"] = "object" }
                        },
                        ["required"] = new JsonArray { "image" }
                    }
                }
            }
        };

        return schemaObject.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    private IConfigFormatProvider FindProviderForFile(string filePath)
    {
        var ext = Path.GetExtension(filePath);
        var provider = _providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase));
        if (provider == null)
        {
            throw new NotSupportedException($"No configuration provider registered for extension '{ext}' of file '{filePath}'.");
        }

        return provider;
    }

    private IConfigFormatProvider FindProviderByName(string format)
    {
        var provider = _providers.FirstOrDefault(p => string.Equals(p.FormatName, format, StringComparison.OrdinalIgnoreCase));
        if (provider == null)
        {
            throw new NotSupportedException($"Unsupported configuration format '{format}'. Supported formats: yaml, json, toml.");
        }

        return provider;
    }
}
