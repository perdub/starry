using System.Net;
using Starry.Core.Dns;
using Starry.Infrastructure.Dns;
using Xunit;

namespace Starry.UnitTests;

public class DnsProviderTests
{
    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [Fact]
    public async Task WebhookDnsProvider_DispatchesUpsertPayload()
    {
        using var handler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);

        var provider = new WebhookDnsProvider(new Uri("https://webhook.example.com/dns"), httpClient);
        var record = new DnsRecord("rec1", "A", "bocchi.example.com", "198.51.100.10", 120, false);

        await provider.UpsertRecordAsync("example.com", record);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        var body = await handler.LastRequest.Content!.ReadAsStringAsync();
        Assert.Contains("upsert", body, StringComparison.Ordinal);
        Assert.Contains("bocchi.example.com", body, StringComparison.Ordinal);
        Assert.Contains("198.51.100.10", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task WebhookDnsProvider_DispatchesDeletePayload()
    {
        using var handler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);

        var provider = new WebhookDnsProvider(new Uri("https://webhook.example.com/dns"), httpClient);

        await provider.DeleteRecordAsync("example.com", "rec-to-delete");

        Assert.NotNull(handler.LastRequest);
        var body = await handler.LastRequest.Content!.ReadAsStringAsync();
        Assert.Contains("delete", body, StringComparison.Ordinal);
        Assert.Contains("rec-to-delete", body, StringComparison.Ordinal);
    }
}
