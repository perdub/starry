namespace Starry.Infrastructure.Network;

public sealed record WireGuardNodeConfig(
    string NodeName,
    string VpnIp,
    string PrivateKey,
    string PublicKey,
    int ListenPort = 51820,
    string? Endpoint = null,
    bool IsPublic = false);
