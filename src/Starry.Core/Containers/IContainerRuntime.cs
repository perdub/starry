using Starry.Core.Nodes;

namespace Starry.Core.Containers;

/// <summary>
/// Container runtime engine abstraction (Docker Engine, Podman rootful, Podman rootless).
/// </summary>
public interface IContainerRuntime
{
    /// <summary>
    /// Runtime identifier name ("docker", "podman").
    /// </summary>
    string RuntimeName { get; }

    /// <summary>
    /// Probes node availability and runtime info.
    /// </summary>
    Task<RuntimeInfo> ProbeAsync(INodeConnection node, CancellationToken ct = default);

    /// <summary>
    /// Lists all containers on the target node.
    /// </summary>
    Task<IReadOnlyList<ContainerState>> ListContainersAsync(INodeConnection node, CancellationToken ct = default);

    /// <summary>
    /// Deploys or reconciles a container service on the target node according to spec.
    /// </summary>
    Task DeployContainerAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct = default);

    /// <summary>
    /// Streams a container image directly peer-to-peer from source node to target node
    /// (e.g. docker save | zstd | ssh load).
    /// </summary>
    Task StreamImagePeerToPeerAsync(INodeConnection source, INodeConnection target, string imageTag, CancellationToken ct = default);
}
