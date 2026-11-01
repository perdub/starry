namespace Starry.Core.Containers;

public sealed record RuntimeInfo(
    string Engine,
    string Version,
    string OperatingSystem,
    string Architecture,
    bool IsRootless,
    string SocketPath);
