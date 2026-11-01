namespace Starry.Infrastructure.Containers;

/// <summary>
/// Docker Engine runtime implementation communicating over Docker Unix domain socket.
/// </summary>
public sealed class DockerRuntime : BaseSocketContainerRuntime
{
    public override string RuntimeName => "docker";

    protected override IReadOnlyList<string> CandidateSocketPaths { get; } =
    [
        "/var/run/docker.sock",
        "/run/docker.sock"
    ];
}
