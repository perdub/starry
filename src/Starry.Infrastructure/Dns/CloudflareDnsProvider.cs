using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Starry.Core.Dns;

namespace Starry.Infrastructure.Dns;

/// <summary>
/// Cloudflare DNS v4 API provider implementation.
/// </summary>
public sealed class CloudflareDnsProvider : IDnsProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;
    private static readonly Uri CloudflareBaseUri = new("https://api.cloudflare.com/client/v4/");

    public string ProviderName => "cloudflare";

    public CloudflareDnsProvider(string apiToken, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiToken);
        _apiToken = apiToken;
        _httpClient = httpClient ?? new HttpClient { BaseAddress = CloudflareBaseUri };
    }

    public async Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

        using var zoneRes = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
        zoneRes.EnsureSuccessStatusCode();

        var zoneJson = await zoneRes.Content.ReadFromJsonAsync<JsonObject>(ct).ConfigureAwait(false);
        var zoneId = ExtractFirstResultId(zoneJson);
        if (string.IsNullOrEmpty(zoneId))
        {
            return [];
        }

        using var recordsReq = new HttpRequestMessage(HttpMethod.Get, $"zones/{zoneId}/dns_records");
        recordsReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

        using var recordsRes = await _httpClient.SendAsync(recordsReq, ct).ConfigureAwait(false);
        recordsRes.EnsureSuccessStatusCode();

        var recordsJson = await recordsRes.Content.ReadFromJsonAsync<JsonObject>(ct).ConfigureAwait(false);
        var resultList = new List<DnsRecord>();

        if (recordsJson?["result"] is JsonArray arr)
        {
            foreach (var item in arr)
            {
                if (item is JsonObject obj)
                {
                    resultList.Add(new DnsRecord(
                        Id: obj["id"]?.ToString() ?? string.Empty,
                        RecordType: obj["type"]?.ToString() ?? "A",
                        Name: obj["name"]?.ToString() ?? string.Empty,
                        Content: obj["content"]?.ToString() ?? string.Empty,
                        Ttl: obj["ttl"]?.GetValue<int>() ?? 300,
                        Proxied: obj["proxied"]?.GetValue<bool>() ?? false));
                }
            }
        }

        return resultList;
    }

    public async Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentNullException.ThrowIfNull(record);

        var existingRecords = await GetRecordsAsync(zoneDomain, ct).ConfigureAwait(false);
        var existing = existingRecords.FirstOrDefault(r =>
            string.Equals(r.Name, record.Name, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(r.RecordType, record.RecordType, StringComparison.OrdinalIgnoreCase));

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);
        var payload = new JsonObject
        {
            ["type"] = record.RecordType,
            ["name"] = record.Name,
            ["content"] = record.Content,
            ["ttl"] = record.Ttl,
            ["proxied"] = record.Proxied
        };

        if (existing != null)
        {
            using var putReq = new HttpRequestMessage(HttpMethod.Put, $"zones/{zoneId}/dns_records/{existing.Id}")
            {
                Content = JsonContent.Create(payload)
            };
            putReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            using var putRes = await _httpClient.SendAsync(putReq, ct).ConfigureAwait(false);
            putRes.EnsureSuccessStatusCode();
        }
        else
        {
            using var postReq = new HttpRequestMessage(HttpMethod.Post, $"zones/{zoneId}/dns_records")
            {
                Content = JsonContent.Create(payload)
            };
            postReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            using var postRes = await _httpClient.SendAsync(postReq, ct).ConfigureAwait(false);
            postRes.EnsureSuccessStatusCode();
        }
    }

    public async Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);
        using var delReq = new HttpRequestMessage(HttpMethod.Delete, $"zones/{zoneId}/dns_records/{recordId}");
        delReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

        using var delRes = await _httpClient.SendAsync(delReq, ct).ConfigureAwait(false);
        delRes.EnsureSuccessStatusCode();
    }

    private async Task<string> ResolveZoneIdAsync(string zoneDomain, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

        using var zoneRes = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
        zoneRes.EnsureSuccessStatusCode();

        var zoneJson = await zoneRes.Content.ReadFromJsonAsync<JsonObject>(ct).ConfigureAwait(false);
        var zoneId = ExtractFirstResultId(zoneJson);
        if (string.IsNullOrEmpty(zoneId))
        {
            throw new InvalidOperationException($"Cloudflare DNS zone not found for domain: '{zoneDomain}'.");
        }

        return zoneId;
    }

    private static string? ExtractFirstResultId(JsonObject? json)
    {
        if (json?["result"] is JsonArray arr && arr.Count > 0 && arr[0] is JsonObject first)
        {
            return first["id"]?.ToString();
        }

        return null;
    }
}
