using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Dns;

namespace Starry.Infrastructure.Dns;

/// <summary>
/// Cloudflare DNS v4 API provider implementation with automated retry and backoff.
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

        using var zoneRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        zoneRes.EnsureSuccessStatusCode();

        var zoneStr = await zoneRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var zoneJson = JsonNode.Parse(zoneStr) as JsonObject;
        var zoneId = ExtractFirstResultId(zoneJson);
        if (string.IsNullOrEmpty(zoneId))
        {
            return [];
        }

        using var recordsRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones/{zoneId}/dns_records");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        recordsRes.EnsureSuccessStatusCode();

        var recordsStr = await recordsRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var recordsJson = JsonNode.Parse(recordsStr) as JsonObject;
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
            using var putRes = await SendWithRetryAsync(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Put, $"zones/{zoneId}/dns_records/{existing.Id}")
                {
                    Content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json")
                };
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
                return req;
            }, ct).ConfigureAwait(false);

            putRes.EnsureSuccessStatusCode();
        }
        else
        {
            using var postRes = await SendWithRetryAsync(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Post, $"zones/{zoneId}/dns_records")
                {
                    Content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json")
                };
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
                return req;
            }, ct).ConfigureAwait(false);

            postRes.EnsureSuccessStatusCode();
        }
    }

    public async Task ConfigureOriginRuleAsync(string zoneDomain, string hostname, int originPort, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(hostname);

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);

        var payload = new JsonObject
        {
            ["rules"] = new JsonArray
            {
                new JsonObject
                {
                    ["expression"] = $"http.host eq \"{hostname}\"",
                    ["description"] = $"Starry origin port rewrite to {originPort}",
                    ["action"] = "route",
                    ["action_parameters"] = new JsonObject
                    {
                        ["origin"] = new JsonObject
                        {
                            ["port"] = originPort
                        }
                    }
                }
            }
        };

        using var putRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Put, $"zones/{zoneId}/rulesets/phases/http_request_origin/entrypoint")
            {
                Content = new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);
    }

    public async Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);
        using var delRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"zones/{zoneId}/dns_records/{recordId}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        delRes.EnsureSuccessStatusCode();
    }

    private async Task<string> ResolveZoneIdAsync(string zoneDomain, CancellationToken ct)
    {
        using var zoneRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        zoneRes.EnsureSuccessStatusCode();

        var zoneStr = await zoneRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var zoneJson = JsonNode.Parse(zoneStr) as JsonObject;
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

    private async Task<HttpResponseMessage> SendWithRetryAsync(Func<HttpRequestMessage> requestFactory, CancellationToken ct)
    {
        var delays = new[] { 100, 300, 700 };

        for (var attempt = 0; attempt <= delays.Length; attempt++)
        {
            var req = requestFactory();
            try
            {
                var response = await _httpClient.SendAsync(req, ct).ConfigureAwait(false);
                var code = (int)response.StatusCode;
                if (code is not (429 or 502 or 503 or 504) || attempt == delays.Length)
                {
                    return response;
                }

                response.Dispose();
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (attempt < delays.Length)
            {
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
        }

        throw new HttpRequestException("HTTP request failed after retries.");
    }
}
