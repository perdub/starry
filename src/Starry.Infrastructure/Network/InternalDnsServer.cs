using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Lightweight embedded DNS responder serving *.internal domain resolutions for Container VIPs.
/// </summary>
public sealed class InternalDnsServer : IAsyncDisposable
{
    private readonly Dictionary<string, IPAddress> _records = new(StringComparer.OrdinalIgnoreCase);
    private UdpClient? _udpListener;
    private CancellationTokenSource? _cts;
    private Task? _listenerTask;

    public int Port { get; }
    public IReadOnlyDictionary<string, IPAddress> Records => _records;

    public InternalDnsServer(int port = 5353)
    {
        Port = port;
    }

    public void RegisterRecord(string domain, IPAddress address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentNullException.ThrowIfNull(address);

        var normalized = domain.Trim().TrimEnd('.');
        _records[normalized] = address;
    }

    public Task StartAsync(IPAddress? bindAddress = null, CancellationToken ct = default)
    {
        var endpoint = new IPEndPoint(bindAddress ?? IPAddress.Any, Port);
        _udpListener = new UdpClient(endpoint);
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        _listenerTask = Task.Run(async () =>
        {
            var token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var receiveResult = await _udpListener.ReceiveAsync(token).ConfigureAwait(false);
                    var response = HandleDnsQuery(receiveResult.Buffer);
                    if (response != null)
                    {
                        await _udpListener.SendAsync(response, response.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }, ct);

        return Task.CompletedTask;
    }

    public byte[]? HandleDnsQuery(byte[] queryBuffer)
    {
        ArgumentNullException.ThrowIfNull(queryBuffer);

        if (queryBuffer.Length < 12)
        {
            return null;
        }

        var (domain, qtype) = ParseQuestion(queryBuffer, 12);
        if (string.IsNullOrEmpty(domain) || qtype != 1) // 1 = A record
        {
            return null;
        }

        if (!_records.TryGetValue(domain, out var ip) || ip.AddressFamily != AddressFamily.InterNetwork)
        {
            return null;
        }

        return BuildDnsResponse(queryBuffer, ip);
    }

    private static (string Domain, ushort QType) ParseQuestion(byte[] buffer, int offset)
    {
        var domainBuilder = new StringBuilder();
        var index = offset;

        while (index < buffer.Length && buffer[index] != 0)
        {
            int labelLen = buffer[index++];
            if (index + labelLen > buffer.Length)
            {
                return (string.Empty, 0);
            }

            if (domainBuilder.Length > 0)
            {
                domainBuilder.Append('.');
            }

            domainBuilder.Append(Encoding.ASCII.GetString(buffer, index, labelLen));
            index += labelLen;
        }

        index++; // Skip null terminator
        if (index + 4 > buffer.Length)
        {
            return (string.Empty, 0);
        }

        ushort qtype = (ushort)((buffer[index] << 8) | buffer[index + 1]);
        return (domainBuilder.ToString(), qtype);
    }

    private static byte[] BuildDnsResponse(byte[] queryBuffer, IPAddress answerIp)
    {
        var response = new byte[queryBuffer.Length + 16];
        Array.Copy(queryBuffer, response, queryBuffer.Length);

        // Flags: Standard response, No error (0x8180)
        response[2] = 0x81;
        response[3] = 0x80;

        // Answer count: 1
        response[6] = 0x00;
        response[7] = 0x01;

        var offset = queryBuffer.Length;
        // Pointer to domain name in question section (offset 12 = 0x0C)
        response[offset++] = 0xC0;
        response[offset++] = 0x0C;

        // Type A (0x0001)
        response[offset++] = 0x00;
        response[offset++] = 0x01;

        // Class IN (0x0001)
        response[offset++] = 0x00;
        response[offset++] = 0x01;

        // TTL: 60 seconds (0x0000003C)
        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x3C;

        // Data length: 4 bytes
        response[offset++] = 0x00;
        response[offset++] = 0x04;

        // IPv4 bytes
        var ipBytes = answerIp.GetAddressBytes();
        Array.Copy(ipBytes, 0, response, offset, 4);

        return response;
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync().ConfigureAwait(false);
        }

        _udpListener?.Dispose();

        if (_listenerTask != null)
        {
            try
            {
                await _listenerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected cancellation on shutdown
            }
            catch (SocketException)
            {
                // Expected socket close on shutdown
            }
        }

        _cts?.Dispose();
    }
}
