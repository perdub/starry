namespace Starry.Infrastructure.Containers;

/// <summary>
/// Podman runtime implementation communicating over Libpod/Compat Unix domain socket (rootful and rootless).
/// </summary>
public sealed class PodmanRuntime : BaseSocketContainerRuntime
{
    public override string RuntimeName => "podman";

    protected override IReadOnlyList<string> CandidateSocketPaths { get; } =
    [
        "/run/podman/podman.sock",
        "/var/run/podman/podman.sock",
        "/run/user/1000/podman/podman.sock",
        "/run/user/0/podman/podman.sock"
    ];
}
