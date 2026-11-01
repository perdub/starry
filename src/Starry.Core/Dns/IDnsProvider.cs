namespace Starry.Core.Dns;

/// <summary>
/// DNS management provider abstraction (Cloudflare, Hetzner, Route53, Porkbun, Webhook).
/// </summary>
public interface IDnsProvider
{
    string ProviderName { get; }
    Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default);
    Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default);
    Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default);
}
