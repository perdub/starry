namespace Starry.Infrastructure.Agent;

public sealed record WireGuardPeerDump(
    string PublicKey,
    string PresharedKey,
    string Endpoint,
    string AllowedIps,
    long LatestHandshake,
    long TransferRx,
    long TransferTx,
    int PersistentKeepalive);
