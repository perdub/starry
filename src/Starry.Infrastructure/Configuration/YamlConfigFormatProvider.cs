using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using YamlDotNet.Serialization;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// YAML configuration provider using YamlDotNet.
/// </summary>
public sealed class YamlConfigFormatProvider : IConfigFormatProvider
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder().Build();
    private readonly ISerializer _serializer = new SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull).Build();

    public IReadOnlyList<string> SupportedExtensions { get; } = [".yaml", ".yml"];

    public string FormatName => "yaml";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var text = Encoding.UTF8.GetString(content);
            using var reader = new StringReader(text);
            var yamlObject = _deserializer.Deserialize(reader);
            return ConvertYamlObjectToJsonNode(yamlObject) ?? new JsonObject();
        }
        catch (Exception ex) when (ex is not FormatException)
        {
            throw new FormatException($"Failed to parse YAML file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        var rawObject = ConvertJsonNodeToRawObject(node);
        return _serializer.Serialize(rawObject);
    }

    private static JsonNode? ConvertYamlObjectToJsonNode(object? yamlObj)
    {
        if (yamlObj is null)
        {
            return null;
        }

        if (yamlObj is IDictionary<object, object> dictObj)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in dictObj)
            {
                var keyStr = key?.ToString() ?? string.Empty;
                jsonObject[keyStr] = ConvertYamlObjectToJsonNode(value);
            }
            return jsonObject;
        }

        if (yamlObj is IDictionary<string, object> dictStr)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in dictStr)
            {
                jsonObject[key] = ConvertYamlObjectToJsonNode(value);
            }
            return jsonObject;
        }

        if (yamlObj is IEnumerable<object> list)
        {
            var jsonArray = new JsonArray();
            foreach (var item in list)
            {
                jsonArray.Add(ConvertYamlObjectToJsonNode(item));
            }
            return jsonArray;
        }

        return yamlObj switch
        {
            string s => JsonValue.Create(s),
            bool b => JsonValue.Create(b),
            int i => JsonValue.Create(i),
            long l => JsonValue.Create(l),
            double d => JsonValue.Create(d),
            float f => JsonValue.Create(f),
            decimal m => JsonValue.Create(m),
            _ => JsonValue.Create(yamlObj.ToString())
        };
    }

    private static object? ConvertJsonNodeToRawObject(JsonNode? node)
    {
        if (node is null)
        {
            return null;
        }

        if (node is JsonObject obj)
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal);
            foreach (var (propName, propValue) in obj)
            {
                dict[propName] = ConvertJsonNodeToRawObject(propValue);
            }
            return dict;
        }

        if (node is JsonArray arr)
        {
            var list = new List<object?>(arr.Count);
            foreach (var item in arr)
            {
                list.Add(ConvertJsonNodeToRawObject(item));
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
