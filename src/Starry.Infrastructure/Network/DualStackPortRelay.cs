using System.Net;
using System.Net.Sockets;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Asynchronous user-space Dual-Stack TCP/UDP socket relay that bridges
/// incoming IPv4 requests into IPv6 mesh containers (e.g. fd67:b00b::/32).
/// </summary>
public sealed class DualStackPortRelay : IAsyncDisposable
{
    private readonly IPAddress _bindIpv4;
    private readonly int _hostPort;
    private readonly IPAddress _targetIpv6;
    private readonly int _targetPort;
    private readonly string _protocol;
    private readonly CancellationTokenSource _cts = new();
    private Task? _relayTask;
    private TcpListener? _tcpListener;
    private UdpClient? _udpClient;

    public DualStackPortRelay(
        string bindIpv4,
        int hostPort,
        string targetIpv6,
        int targetPort,
        string protocol = "tcp")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bindIpv4);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetIpv6);
        ArgumentNullException.ThrowIfNull(protocol);

        _bindIpv4 = IPAddress.Parse(bindIpv4);
        _hostPort = hostPort;
        _targetIpv6 = IPAddress.Parse(targetIpv6);
        _targetPort = targetPort;
        _protocol = protocol.ToUpperInvariant();
    }

    public void Start()
    {
        if (_protocol is "TCP" or "BOTH")
        {
            _tcpListener = new TcpListener(_bindIpv4, _hostPort);
            _tcpListener.Start();
            _relayTask = RunTcpRelayLoopAsync(_cts.Token);
        }

        if (_protocol is "UDP" or "BOTH")
        {
            _udpClient = new UdpClient(new IPEndPoint(_bindIpv4, _hostPort));
            _relayTask = RunUdpRelayLoopAsync(_cts.Token);
        }
    }

    private async Task RunTcpRelayLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _tcpListener != null)
        {
            try
            {
                var clientSocket = await _tcpListener.AcceptTcpClientAsync(ct).ConfigureAwait(false);
                _ = HandleTcpClientAsync(clientSocket, ct);
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
    }

    private async Task HandleTcpClientAsync(TcpClient client, CancellationToken ct)
    {
        using (client)
        using (var target = new TcpClient(AddressFamily.InterNetworkV6))
        {
            try
            {
                await target.ConnectAsync(_targetIpv6, _targetPort, ct).ConfigureAwait(false);
                var clientStream = client.GetStream();
                var targetStream = target.GetStream();

                await Task.WhenAny(
                    clientStream.CopyToAsync(targetStream, ct),
                    targetStream.CopyToAsync(clientStream, ct)).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Connection terminated or refused
            }
        }
    }

    private async Task RunUdpRelayLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _udpClient != null)
        {
            try
            {
                var result = await _udpClient.ReceiveAsync(ct).ConfigureAwait(false);
                _ = HandleUdpDatagramAsync(result, ct);
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
    }

    private async Task HandleUdpDatagramAsync(UdpReceiveResult incoming, CancellationToken ct)
    {
        using var upstream = new UdpClient(AddressFamily.InterNetworkV6);
        try
        {
            await upstream.SendAsync(incoming.Buffer, new IPEndPoint(_targetIpv6, _targetPort), ct).ConfigureAwait(false);
            var reply = await upstream.ReceiveAsync(ct).ConfigureAwait(false);
            if (_udpClient != null)
            {
                await _udpClient.SendAsync(reply.Buffer, incoming.RemoteEndPoint, ct).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Transient UDP drop
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _cts.CancelAsync().ConfigureAwait(false);

        _tcpListener?.Dispose();
        _udpClient?.Dispose();

        if (_relayTask != null)
        {
            try
            {
                await _relayTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected on stop
            }
        }

        _cts.Dispose();
    }
}
