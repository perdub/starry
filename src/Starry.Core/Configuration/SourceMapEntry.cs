namespace Starry.Core.Configuration;

/// <summary>
/// Source mapping entry linking a canonical JSON path to its physical source file and line.
/// </summary>
public sealed record SourceMapEntry(
    string CanonicalPath,
    string FilePath,
    int Line = 1,
    int Column = 1);
