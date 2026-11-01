using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// JSON format provider based on System.Text.Json with native Native AOT support.
/// </summary>
public sealed class JsonConfigFormatProvider : IConfigFormatProvider
{
    private static readonly JsonSerializerOptions IndentedOptions = new()
    {
        WriteIndented = true
    };

    public IReadOnlyList<string> SupportedExtensions { get; } = [".json"];

    public string FormatName => "json";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var parsed = JsonNode.Parse(content);
            return parsed ?? new JsonObject();
        }
        catch (JsonException ex)
        {
            throw new FormatException($"Failed to parse JSON file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.ToJsonString(IndentedOptions);
    }
}
