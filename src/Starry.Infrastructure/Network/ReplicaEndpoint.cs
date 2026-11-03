using System.Net;

namespace Starry.Infrastructure.Network;

public sealed record ReplicaEndpoint(
    string ReplicaName,
    IPAddress Ipv6Address,
    IPAddress? Ipv4Address = null);
