using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;

namespace Starry.Infrastructure.Security;

/// <summary>
/// Manages secret discovery, on-the-fly encryption of PLAIN[...] secrets,
/// atomic multi-file updates, and global master key rotation.
/// </summary>
public sealed class SecretManager
{
    private readonly CryptoService _cryptoService = new();

    public int EncryptOpenSecrets(ConfigTree tree, byte[] masterKey, string rootPath, VirtualConfigEngine engine)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentNullException.ThrowIfNull(masterKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        ArgumentNullException.ThrowIfNull(engine);

        var count = 0;
        var modified = EncryptOpenSecretsRecursive(tree.Root, string.Empty, masterKey, ref count);

        if (modified)
        {
            SaveTreeAtomically(tree, rootPath, engine);
        }

        return count;
    }

    public int ReencryptAll(ConfigTree tree, byte[] oldKey, byte[] newKey, string rootPath, VirtualConfigEngine engine)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentNullException.ThrowIfNull(oldKey);
        ArgumentNullException.ThrowIfNull(newKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        ArgumentNullException.ThrowIfNull(engine);

        var count = 0;
        var modified = ReencryptSecretsRecursive(tree.Root, string.Empty, oldKey, newKey, ref count);

        if (modified)
        {
            SaveTreeAtomically(tree, rootPath, engine);
        }

        return count;
    }

    public static void AtomicWriteFile(string targetPath, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetPath);
        ArgumentNullException.ThrowIfNull(content);

        var dir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var tempPath = Path.Combine(dir ?? ".", $".{Path.GetFileName(targetPath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
            }

            File.Move(tempPath, targetPath, overwrite: true);
        }
        catch
        {
            if (File.Exists(tempPath))
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch (IOException)
                {
                    // Ignore secondary cleanup exception
                }
            }
            throw;
        }
    }

    private bool EncryptOpenSecretsRecursive(JsonNode? current, string currentPath, byte[] masterKey, ref int count)
    {
        if (current is null)
        {
            return false;
        }

        if (current is JsonObject obj)
        {
            return EncryptObjectProperties(obj, currentPath, masterKey, ref count);
        }

        if (current is JsonArray arr)
        {
            return EncryptArrayItems(arr, currentPath, masterKey, ref count);
        }

        return false;
    }

    private bool EncryptObjectProperties(JsonObject obj, string currentPath, byte[] masterKey, ref int count)
    {
        var modified = false;
        var keys = obj.Select(kv => kv.Key).ToList();

        foreach (var key in keys)
        {
            var val = obj[key];
            var childPath = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";

            if (val is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                var rawSecret = strVal["PLAIN[".Length..^1];
                var plaintextBytes = Encoding.UTF8.GetBytes(rawSecret);
                var envelope = _cryptoService.Encrypt(plaintextBytes, masterKey, childPath);
                obj[key] = envelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((val is JsonObject or JsonArray) && EncryptOpenSecretsRecursive(val, childPath, masterKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool EncryptArrayItems(JsonArray arr, string currentPath, byte[] masterKey, ref int count)
    {
        var modified = false;

        for (var i = 0; i < arr.Count; i++)
        {
            var childPath = $"{currentPath}[{i}]";
            var item = arr[i];

            if (item is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                var rawSecret = strVal["PLAIN[".Length..^1];
                var plaintextBytes = Encoding.UTF8.GetBytes(rawSecret);
                var envelope = _cryptoService.Encrypt(plaintextBytes, masterKey, childPath);
                arr[i] = envelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((item is JsonObject or JsonArray) && EncryptOpenSecretsRecursive(item, childPath, masterKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool ReencryptSecretsRecursive(JsonNode? current, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        if (current is null)
        {
            return false;
        }

        if (current is JsonObject obj)
        {
            return ReencryptObjectProperties(obj, currentPath, oldKey, newKey, ref count);
        }

        if (current is JsonArray arr)
        {
            return ReencryptArrayItems(arr, currentPath, oldKey, newKey, ref count);
        }

        return false;
    }

    private bool ReencryptObjectProperties(JsonObject obj, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        var modified = false;
        var keys = obj.Select(kv => kv.Key).ToList();

        foreach (var key in keys)
        {
            var val = obj[key];
            var childPath = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";

            if (val is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                _cryptoService.TryParseEncrypted(strVal, out var envelope) && envelope != null)
            {
                var plaintextBytes = _cryptoService.Decrypt(envelope, oldKey, childPath);
                var newEnvelope = _cryptoService.Encrypt(plaintextBytes, newKey, childPath);
                obj[key] = newEnvelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((val is JsonObject or JsonArray) && ReencryptSecretsRecursive(val, childPath, oldKey, newKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool ReencryptArrayItems(JsonArray arr, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        var modified = false;

        for (var i = 0; i < arr.Count; i++)
        {
            var childPath = $"{currentPath}[{i}]";
            var item = arr[i];

            if (item is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                _cryptoService.TryParseEncrypted(strVal, out var envelope) && envelope != null)
            {
                var plaintextBytes = _cryptoService.Decrypt(envelope, oldKey, childPath);
                var newEnvelope = _cryptoService.Encrypt(plaintextBytes, newKey, childPath);
                arr[i] = newEnvelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((item is JsonObject or JsonArray) && ReencryptSecretsRecursive(item, childPath, oldKey, newKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private static void SaveTreeAtomically(ConfigTree tree, string rootPath, VirtualConfigEngine engine)
    {
        if (File.Exists(rootPath))
        {
            var ext = Path.GetExtension(rootPath);
            var provider = engine.Providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                           ?? engine.Providers[0];
            var serialized = provider.Serialize(tree.Root);
            AtomicWriteFile(rootPath, serialized);
            return;
        }

        if (Directory.Exists(rootPath))
        {
            var filesByPath = tree.SourceMap.Values
                .Select(v => v.FilePath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (filesByPath.Count > 0)
            {
                foreach (var relativeFile in filesByPath)
                {
                    SavePhysicalConfigFile(tree, rootPath, relativeFile, engine);
                }
                return;
            }

            engine.Split(tree, rootPath, "yaml");
        }
    }

    private static void SavePhysicalConfigFile(ConfigTree tree, string rootDir, string relativeFile, VirtualConfigEngine engine)
    {
        var fullPath = Path.IsPathRooted(relativeFile)
            ? relativeFile
            : Path.Combine(rootDir, relativeFile);

        var ext = Path.GetExtension(fullPath);
        var provider = engine.Providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase));
        if (provider == null)
        {
            return;
        }

        var pathWithoutExt = Path.Combine(
            Path.GetDirectoryName(relativeFile) ?? string.Empty,
            Path.GetFileNameWithoutExtension(relativeFile)).Replace('\\', '/');

        var fileNameNoExt = Path.GetFileNameWithoutExtension(relativeFile);
        JsonNode? subtree = null;
        if (string.Equals(fileNameNoExt, "starry", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(fileNameNoExt, "config", StringComparison.OrdinalIgnoreCase))
        {
            // Root properties that belong to this file
            var rootObj = new JsonObject();
            foreach (var (k, v) in tree.Root)
            {
                if (tree.SourceMap.TryGetValue(k, out var sm) &&
                    string.Equals(Path.GetFileName(sm.FilePath), Path.GetFileName(relativeFile), StringComparison.OrdinalIgnoreCase))
                {
                    rootObj[k] = v != null ? JsonNode.Parse(v.ToJsonString()) : null;
                }
            }
            subtree = rootObj.Count > 0 ? rootObj : tree.Root;
        }
        else
        {
            var canonicalPrefix = pathWithoutExt.Replace('/', '.');
            subtree = tree.FindNode(canonicalPrefix);
        }

        if (subtree != null)
        {
            var content = provider.Serialize(subtree);
            AtomicWriteFile(fullPath, content);
        }
    }
}
