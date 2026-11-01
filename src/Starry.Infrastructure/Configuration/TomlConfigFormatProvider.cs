using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Tomlyn;
using Tomlyn.Model;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// TOML configuration provider using Tomlyn.
/// </summary>
public sealed class TomlConfigFormatProvider : IConfigFormatProvider
{
    public IReadOnlyList<string> SupportedExtensions { get; } = [".toml"];

    public string FormatName => "toml";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var text = Encoding.UTF8.GetString(content);
            var model = Toml.ToModel(text);
            return ConvertTomlTableToJsonNode(model) ?? new JsonObject();
        }
        catch (Exception ex) when (ex is not FormatException)
        {
            throw new FormatException($"Failed to parse TOML file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        var tomlTable = ConvertJsonNodeToTomlObject(node) as TomlTable ?? new TomlTable();
        return Toml.FromModel(tomlTable);
    }

    private static JsonNode? ConvertTomlTableToJsonNode(object? tomlObj)
    {
        if (tomlObj is null)
        {
            return null;
        }

        if (tomlObj is TomlTable table)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in table)
            {
                jsonObject[key] = ConvertTomlTableToJsonNode(value);
            }
            return jsonObject;
        }

        if (tomlObj is TomlArray array)
        {
            var jsonArray = new JsonArray();
            foreach (var item in array)
            {
                jsonArray.Add(ConvertTomlTableToJsonNode(item));
            }
            return jsonArray;
        }

        if (tomlObj is TomlTableArray tableArray)
        {
            var jsonArray = new JsonArray();
            foreach (var item in tableArray)
            {
                jsonArray.Add(ConvertTomlTableToJsonNode(item));
            }
            return jsonArray;
        }

        return tomlObj switch
        {
            string s => JsonValue.Create(s),
            bool b => JsonValue.Create(b),
            int i => JsonValue.Create(i),
            long l => JsonValue.Create(l),
            double d => JsonValue.Create(d),
            float f => JsonValue.Create(f),
            _ => JsonValue.Create(tomlObj.ToString())
        };
    }

    private static object? ConvertJsonNodeToTomlObject(JsonNode? node)
    {
        if (node is null)
        {
            return null;
        }

        if (node is JsonObject obj)
        {
            var table = new TomlTable();
            foreach (var (propName, propValue) in obj)
            {
                var converted = ConvertJsonNodeToTomlObject(propValue);
                if (converted != null)
                {
                    table[propName] = converted;
                }
            }
            return table;
        }

        if (node is JsonArray arr)
        {
            var list = new TomlArray();
            foreach (var item in arr)
            {
                var converted = ConvertJsonNodeToTomlObject(item);
                if (converted != null)
                {
                    list.Add(converted);
                }
            }
            return list;
        }

        if (node is JsonValue jsonVal)
        {
            if (jsonVal.TryGetValue<bool>(out var boolVal))
            {
                return boolVal;
            }

            if (jsonVal.TryGetValue<long>(out var longVal))
            {
                return longVal;
            }

            if (jsonVal.TryGetValue<double>(out var doubleVal))
            {
                return doubleVal;
            }

            if (jsonVal.TryGetValue<string>(out var strVal))
            {
                return strVal;
            }

            return jsonVal.ToString();
        }

        return node.ToString();
    }
}
