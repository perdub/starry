namespace Starry.Core.Dns;

public sealed record DnsRecord(
    string Id,
    string RecordType,
    string Name,
    string Content,
    int Ttl = 300,
    bool Proxied = false);
