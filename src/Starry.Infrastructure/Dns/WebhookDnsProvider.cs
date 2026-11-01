using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Starry.Core.Dns;

namespace Starry.Infrastructure.Dns;

/// <summary>
/// Webhook-based DNS provider dispatching HTTP POST events on DNS record upsert and delete.
/// </summary>
public sealed class WebhookDnsProvider : IDnsProvider
{
    private readonly HttpClient _httpClient;
    private readonly Uri _webhookUri;

    public string ProviderName => "webhook";

    public WebhookDnsProvider(Uri webhookUri, HttpClient? httpClient = null)
    {
        ArgumentNullException.ThrowIfNull(webhookUri);
        _webhookUri = webhookUri;
        _httpClient = httpClient ?? new HttpClient();
    }

    public Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default)
    {
        return Task.FromResult<IReadOnlyList<DnsRecord>>([]);
    }

    public async Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentNullException.ThrowIfNull(record);

        var payload = new JsonObject
        {
            ["action"] = "upsert",
            ["zone"] = zoneDomain,
            ["record"] = new JsonObject
            {
                ["id"] = record.Id,
                ["type"] = record.RecordType,
                ["name"] = record.Name,
                ["content"] = record.Content,
                ["ttl"] = record.Ttl,
                ["proxied"] = record.Proxied
            }
        };

        using var content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(_webhookUri, content, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        var payload = new JsonObject
        {
            ["action"] = "delete",
            ["zone"] = zoneDomain,
            ["recordId"] = recordId
        };

        using var content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(_webhookUri, content, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}
