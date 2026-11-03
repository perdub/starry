using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Dsl;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Virtual Configuration Engine for loading, merging, packing, splitting,
/// formatting, resolving AST graph references via Starry.Dsl, and validating configuration trees.
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

    public ConfigTree Load(string path, bool resolveGraph = true, Func<string, string>? secretDecryptor = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        ConfigTree tree;
        if (File.Exists(path))
        {
            tree = LoadFile(path);
        }
        else if (Directory.Exists(path))
        {
            tree = LoadDirectory(path);
        }
        else
        {
            throw new FileNotFoundException($"Configuration path does not exist: {path}", path);
        }

        if (resolveGraph)
        {
            ResolveGraph(tree, secretDecryptor);
        }

        return tree;
    }

    public static void ResolveGraph(ConfigTree tree, Func<string, string>? secretDecryptor = null)
    {
        ArgumentNullException.ThrowIfNull(tree);

        var optDecryptor = secretDecryptor != null
            ? Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.Some(
                Microsoft.FSharp.Core.FuncConvert.FromFunc<string, string>(secretDecryptor))
            : Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None;

        var configNode = ConfigAst.fromJsonNode(Microsoft.FSharp.Core.FSharpOption<string>.None, tree.Root);
        var resolvedResult = GraphResolver.resolve(configNode, optDecryptor);

        if (resolvedResult.IsError)
        {
            var err = resolvedResult.ErrorValue;
            if (err.IsCircularReference)
            {
                var chain = string.Join(" -> ", ((GraphResolver.ResolutionError.CircularReference)err).cycleChain);
                throw new InvalidOperationException($"Circular dependency detected in configuration graph: {chain}");
            }

            throw new InvalidOperationException($"Failed to resolve configuration graph references: {err}");
        }

        if (ConfigAst.toJsonNode(resolvedResult.ResultValue) is JsonObject resolvedJson)
        {
            var keys = tree.Root.Select(kv => kv.Key).ToList();
            foreach (var key in keys)
            {
                tree.Root.Remove(key);
            }

            foreach (var (k, v) in resolvedJson)
            {
                tree.Root[k] = v != null ? JsonNode.Parse(v.ToJsonString()) : null;
            }
        }
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
            .Where(f => !IsExcludedConfigPath(dirPath, f))
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var file in files)
        {
            LoadSingleConfigFile(tree, dirPath, file);
        }

        LoadRawFilesDirectory(tree, dirPath);

        return tree;
    }

    private void LoadSingleConfigFile(ConfigTree tree, string dirPath, string file)
    {
        var relative = Path.GetRelativePath(dirPath, file).Replace('\\', '/');
        var provider = FindProviderForFile(file);
        var bytes = File.ReadAllBytes(file);
        var parsed = provider.Parse(bytes, relative);

        var dirName = Path.GetDirectoryName(relative);
        var fileNameNoExt = Path.GetFileNameWithoutExtension(relative);

        if (string.IsNullOrEmpty(dirName) &&
            (string.Equals(fileNameNoExt, "starry", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(fileNameNoExt, "config", StringComparison.OrdinalIgnoreCase)))
        {
            if (parsed is JsonObject rootObj)
            {
                PopulateObjectIntoTree(tree, rootObj, string.Empty, relative);
            }
        }
        else
        {
            var pathWithoutExt = Path.Combine(dirName ?? string.Empty, fileNameNoExt).Replace('\\', '/');
            var canonicalPrefix = pathWithoutExt.Replace('/', '.');
            if (parsed is JsonObject obj)
            {
                if (string.Equals(dirName, "nodes", StringComparison.OrdinalIgnoreCase) &&
                    obj.TryGetPropertyValue("ssh", out var sshVal) && sshVal != null &&
                    (!obj.ContainsKey("host") || string.IsNullOrWhiteSpace(obj["host"]?.ToString())))
                {
                    var (u, h, p) = ParseSsh(sshVal.ToString());
                    obj["host"] = h;
                    obj["port"] = p;
                    obj["user"] = u;
                }

                PopulateObjectIntoTree(tree, obj, canonicalPrefix, relative);
            }
            else
            {
                tree.SetNode(canonicalPrefix, parsed, new SourceMapEntry(canonicalPrefix, relative));
            }
        }
    }

    private static (string User, string Host, int Port) ParseSsh(string input)
    {
        var user = "root";
        var host = input.Trim();
        var port = 22;

        if (host.Contains('@', StringComparison.Ordinal))
        {
            var atParts = host.Split('@', 2);
            user = atParts[0];
            host = atParts[1];
        }

        if (host.Contains(':', StringComparison.Ordinal))
        {
            var colonParts = host.Split(':', 2);
            host = colonParts[0];
            if (int.TryParse(colonParts[1], System.Globalization.CultureInfo.InvariantCulture, out var p))
            {
                port = p;
            }
        }

        return (user, host, port);
    }

    private static void LoadRawFilesDirectory(ConfigTree tree, string dirPath)
    {
        var filesDir = Path.Combine(dirPath, "files");
        if (!Directory.Exists(filesDir))
        {
            return;
        }

        if (tree.Root["files"] is not JsonObject filesObj)
        {
            filesObj = new JsonObject();
            tree.Root["files"] = filesObj;
        }

        var rawFiles = Directory.GetFiles(filesDir, "*.*", SearchOption.AllDirectories);
        foreach (var rf in rawFiles)
        {
            var rel = Path.GetRelativePath(dirPath, rf).Replace('\\', '/');
            var relInsideFiles = Path.GetRelativePath(filesDir, rf).Replace('\\', '/');
            var content = File.ReadAllText(rf);
            filesObj[relInsideFiles] = JsonValue.Create(content);
            tree.RegisterSource($"files.{relInsideFiles}", new SourceMapEntry($"files.{relInsideFiles}", rel));
        }
    }

    private static bool IsExcludedConfigPath(string rootDir, string fullPath)
    {
        var rel = Path.GetRelativePath(rootDir, fullPath).Replace('\\', '/');
        var fileName = Path.GetFileName(rel);

        if (string.Equals(fileName, "manifest.json", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var segments = rel.Split('/');
        if (segments.Length > 0 && string.Equals(segments[0], "files", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return segments.Any(s =>
            string.Equals(s, ".starry", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, ".github", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, ".git", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, "node_modules", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, "dist", StringComparison.OrdinalIgnoreCase));
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
            SplitSubtreeProperty(targetDirectoryPath, key, val, ext, provider, rootCopy);
        }

        var mainConfigPath = Path.Combine(targetDirectoryPath, $"starry{ext}");
        File.WriteAllText(mainConfigPath, provider.Serialize(rootCopy));
    }

    private static void SplitSubtreeProperty(
        string targetDirectoryPath,
        string key,
        JsonNode? val,
        string ext,
        IConfigFormatProvider provider,
        JsonObject rootCopy)
    {
        if (string.Equals(key, "nodes", StringComparison.OrdinalIgnoreCase) && val is JsonObject nodesObj)
        {
            WriteDirectorySubtree(Path.Combine(targetDirectoryPath, "nodes"), nodesObj, ext, provider);
        }
        else if (string.Equals(key, "services", StringComparison.OrdinalIgnoreCase) && val is JsonObject servicesObj)
        {
            WriteDirectorySubtree(Path.Combine(targetDirectoryPath, "services"), servicesObj, ext, provider);
        }
        else if (string.Equals(key, "domains", StringComparison.OrdinalIgnoreCase) && val != null)
        {
            var domDir = Path.Combine(targetDirectoryPath, "domains");
            Directory.CreateDirectory(domDir);
            File.WriteAllText(Path.Combine(domDir, $"domains{ext}"), provider.Serialize(val));
        }
        else if (string.Equals(key, "ports", StringComparison.OrdinalIgnoreCase) && val != null)
        {
            File.WriteAllText(Path.Combine(targetDirectoryPath, $"ports{ext}"), provider.Serialize(val));
        }
        else if (string.Equals(key, "volumes", StringComparison.OrdinalIgnoreCase) && val != null)
        {
            File.WriteAllText(Path.Combine(targetDirectoryPath, $"volumes{ext}"), provider.Serialize(val));
        }
        else if (string.Equals(key, "files", StringComparison.OrdinalIgnoreCase) && val is JsonObject filesObj)
        {
            WriteRawFilesSubtree(Path.Combine(targetDirectoryPath, "files"), filesObj);
        }
        else if (val != null)
        {
            rootCopy[key] = JsonNode.Parse(val.ToJsonString());
        }
    }

    private static void WriteDirectorySubtree(string dir, JsonObject obj, string ext, IConfigFormatProvider provider)
    {
        Directory.CreateDirectory(dir);
        foreach (var (name, nodeVal) in obj)
        {
            if (nodeVal != null)
            {
                File.WriteAllText(Path.Combine(dir, $"{name}{ext}"), provider.Serialize(nodeVal));
            }
        }
    }

    private static void WriteRawFilesSubtree(string filesDir, JsonObject filesObj)
    {
        Directory.CreateDirectory(filesDir);
        foreach (var (name, contentVal) in filesObj)
        {
            if (contentVal is JsonValue jv)
            {
                var filePath = Path.Combine(filesDir, name);
                var pDir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(pDir))
                {
                    Directory.CreateDirectory(pDir);
                }
                File.WriteAllText(filePath, jv.ToString());
            }
        }
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
                        }
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
                        ["required"] = new JsonArray(JsonValue.Create("image"))
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
