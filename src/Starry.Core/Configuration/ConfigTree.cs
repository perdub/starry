using System.Text.Json.Nodes;

namespace Starry.Core.Configuration;

/// <summary>
/// Virtual aggregated configuration tree with source map tracking across multiple files.
/// </summary>
public sealed class ConfigTree
{
    private readonly Dictionary<string, SourceMapEntry> _sourceMap = new(StringComparer.Ordinal);

    public JsonObject Root { get; }

    public IReadOnlyDictionary<string, SourceMapEntry> SourceMap => _sourceMap;

    public ConfigTree(JsonObject? root = null)
    {
        Root = root ?? new JsonObject();
    }

    public void RegisterSource(string canonicalPath, SourceMapEntry entry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalPath);
        ArgumentNullException.ThrowIfNull(entry);

        _sourceMap[canonicalPath] = entry;
    }

    public JsonNode? FindNode(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        JsonNode? current = Root;

        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];
            if (current is JsonObject obj)
            {
                if (obj.TryGetPropertyValue(segment, out var next))
                {
                    current = next;
                }
                else
                {
                    var remaining = string.Join(".", segments[i..]);
                    if (obj.TryGetPropertyValue(remaining, out var remainingVal))
                    {
                        return remainingVal;
                    }
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        return current;
    }

    public void SetNode(string path, JsonNode? value, SourceMapEntry? sourceMapEntry = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var current = Root;

        for (var i = 0; i < segments.Length - 1; i++)
        {
            var segment = segments[i];
            if (!current.TryGetPropertyValue(segment, out var next) || next is not JsonObject nextObj)
            {
                nextObj = new JsonObject();
                current[segment] = nextObj;
            }

            current = nextObj;
        }

        var lastSegment = segments[^1];
        current[lastSegment] = value;

        if (sourceMapEntry != null)
        {
            _sourceMap[path] = sourceMapEntry;
        }
    }
}
