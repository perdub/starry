using System.Net;

namespace Starry.Infrastructure.Agent;

public sealed record AgentOptions
{
    public int DnsPort { get; init; } = 53;
    public IPAddress? DnsBindAddress { get; init; }
    public IReadOnlyDictionary<string, string> InternalDnsRecords { get; init; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public TimeSpan HealthCheckInterval { get; init; } = TimeSpan.FromSeconds(30);
    public IReadOnlyList<BackupPolicy> BackupPolicies { get; init; } = [];
    public string? NodeName { get; init; }

    public AgentOptions WithDnsRecord(string domain, string ip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentException.ThrowIfNullOrWhiteSpace(ip);

        var dict = new Dictionary<string, string>(InternalDnsRecords, StringComparer.OrdinalIgnoreCase)
        {
            [domain] = ip
        };
        return this with { InternalDnsRecords = dict };
    }

    public AgentOptions WithDnsRecord(string domain, IPAddress ip)
    {
        ArgumentNullException.ThrowIfNull(ip);
        return WithDnsRecord(domain, ip.ToString());
    }
}
