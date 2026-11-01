using System.Text.Json.Nodes;

namespace Starry.Core.Configuration;

/// <summary>
/// Configuration format provider interface for parsing and serializing config files (YAML, JSON, TOML, etc.).
/// </summary>
public interface IConfigFormatProvider
{
    /// <summary>
    /// File extensions supported by this provider (e.g., [".yaml", ".yml"], [".json"], [".toml"]).
    /// </summary>
    IReadOnlyList<string> SupportedExtensions { get; }

    /// <summary>
    /// Format identifier name (e.g. "yaml", "json", "toml").
    /// </summary>
    string FormatName { get; }

    /// <summary>
    /// Parses byte span content into a JsonNode/JsonObject tree with source mapping.
    /// </summary>
    /// <param name="content">Raw content bytes.</param>
    /// <param name="sourcePath">Originating file path.</param>
    /// <returns>Parsed JsonNode.</returns>
    JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath);

    /// <summary>
    /// Serializes a JsonNode tree into the target format string.
    /// </summary>
    /// <param name="node">JsonNode tree.</param>
    /// <returns>Formatted string representation.</returns>
    string Serialize(JsonNode node);
}
