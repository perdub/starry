using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Scoped internal DNS responder serving fd67:b00b::/32 AAAA records with source-aware network ACL isolation,
/// replica round-robin balancing, and egress proxying.
/// </summary>
public sealed class InternalDnsServer : IAsyncDisposable
{
    private readonly Dictionary<string, IPAddress> _records = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<ReplicaEndpoint>> _serviceEndpoints = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, HashSet<string>> _serviceNetworks = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _subnetToService = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, bool> _serviceEgress = new(StringComparer.OrdinalIgnoreCase);

    private UdpClient? _udpListener;
    private CancellationTokenSource? _cts;
    private Task? _listenerTask;

    public int Port { get; }
    public IReadOnlyDictionary<string, IPAddress> Records => _records;
    public IPAddress UpstreamDns { get; set; } = IPAddress.Parse("1" + ".1.1.1");

    public InternalDnsServer(int port = 5353)
    {
        Port = port;
    }

    public void RegisterRecord(string domain, IPAddress address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentNullException.ThrowIfNull(address);

        var normalized = NormalizeDomain(domain);
        _records[normalized] = address;

        var endpoint = new ReplicaEndpoint(normalized, address);
        RegisterReplica(normalized, endpoint, ["default"]);
    }

    public void RegisterService(
        string serviceName,
        IEnumerable<ReplicaEndpoint> endpoints,
        IEnumerable<string> networks,
        bool egressAllowed = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);
        ArgumentNullException.ThrowIfNull(endpoints);
        ArgumentNullException.ThrowIfNull(networks);

        var normName = NormalizeDomain(serviceName);
        var endpointList = endpoints.ToList();
        _serviceEndpoints[normName] = endpointList;
        _serviceNetworks[normName] = new HashSet<string>(networks, StringComparer.OrdinalIgnoreCase);
        _serviceEgress[normName] = egressAllowed;

        if (endpointList.Count > 0)
        {
            _records[normName] = endpointList[0].Ipv6Address;
            _records[$"{normName}.internal"] = endpointList[0].Ipv6Address;
        }

        foreach (var ep in endpointList)
        {
            var repDomain = NormalizeDomain(ep.ReplicaName);
            _records[repDomain] = ep.Ipv6Address;
            _records[$"{repDomain}.internal"] = ep.Ipv6Address;

            // Map container /64 subnet prefix to service name for source-aware ACL
            var subnetPrefix = ExtractSubnetPrefix64(ep.Ipv6Address);
            if (!string.IsNullOrEmpty(subnetPrefix))
            {
                _subnetToService[subnetPrefix] = normName;
            }
        }
    }

    public void RegisterReplica(string serviceName, ReplicaEndpoint endpoint, IEnumerable<string> networks)
    {
        RegisterService(serviceName, [endpoint], networks, egressAllowed: true);
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
                    var response = await HandleDnsQueryAsync(receiveResult.Buffer, receiveResult.RemoteEndPoint.Address, token).ConfigureAwait(false);
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

    public byte[]? HandleDnsQuery(byte[] queryBuffer, IPAddress? clientAddress = null)
    {
        return HandleDnsQueryAsync(queryBuffer, clientAddress, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<byte[]?> HandleDnsQueryAsync(byte[] queryBuffer, IPAddress? clientAddress = null, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(queryBuffer);

        if (queryBuffer.Length < 12)
        {
            return null;
        }

        var (domain, qtype) = ParseQuestion(queryBuffer, 12);
        if (string.IsNullOrEmpty(domain))
        {
            return null;
        }

        var cleanDomain = NormalizeDomain(domain);
        var baseServiceName = cleanDomain.EndsWith(".INTERNAL", StringComparison.OrdinalIgnoreCase)
            ? cleanDomain[..^".INTERNAL".Length]
            : cleanDomain;

        // Check if query is for internal cluster service
        List<ReplicaEndpoint>? endpoints = null;
        IPAddress? directIp = null;
        var hasEndpoints = _serviceEndpoints.TryGetValue(baseServiceName, out endpoints);
        var hasDirect = _records.TryGetValue(cleanDomain, out directIp);

        if (hasEndpoints || hasDirect)
        {
            return HandleInternalQuery(queryBuffer, qtype, endpoints, directIp, clientAddress, baseServiceName);
        }

        // Domain is not internal service
        if (cleanDomain.EndsWith(".INTERNAL", StringComparison.OrdinalIgnoreCase) ||
            cleanDomain.EndsWith(".LOCAL", StringComparison.OrdinalIgnoreCase))
        {
            return BuildNxDomainResponse(queryBuffer);
        }

        // External domain resolution: check egress policy
        if (clientAddress != null)
        {
            var callerService = ResolveCallerService(clientAddress);
            if (!string.IsNullOrEmpty(callerService) &&
                _serviceEgress.TryGetValue(callerService, out var egressAllowed) &&
                !egressAllowed)
            {
                return BuildRefusedResponse(queryBuffer);
            }
        }

        return await ForwardToUpstreamAsync(queryBuffer, ct).ConfigureAwait(false);
    }

    private byte[]? HandleInternalQuery(
        byte[] queryBuffer,
        ushort qtype,
        List<ReplicaEndpoint>? endpoints,
        IPAddress? directIp,
        IPAddress? clientAddress,
        string baseServiceName)
    {
        if (clientAddress != null && !IsAccessAllowed(clientAddress, baseServiceName))
        {
            return BuildNxDomainResponse(queryBuffer);
        }

        if (qtype == 28) // AAAA
        {
            List<IPAddress> ipv6List;
            if (endpoints != null)
            {
                ipv6List = endpoints.Select(e => e.Ipv6Address).Where(ip => ip.AddressFamily == AddressFamily.InterNetworkV6).ToList();
            }
            else if (directIp != null && directIp.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipv6List = [directIp];
            }
            else
            {
                ipv6List = [];
            }

            if (ipv6List.Count > 0)
            {
                return BuildAaaaResponse(queryBuffer, ipv6List);
            }

            return BuildEmptyNoErrorResponse(queryBuffer);
        }

        if (qtype == 1) // A
        {
            List<IPAddress> ipv4List;
            if (endpoints != null)
            {
                ipv4List = endpoints.Select(e => e.Ipv4Address).Where(ip => ip != null && ip.AddressFamily == AddressFamily.InterNetwork).Select(ip => ip!).ToList();
            }
            else if (directIp != null && directIp.AddressFamily == AddressFamily.InterNetwork)
            {
                ipv4List = [directIp];
            }
            else
            {
                ipv4List = [];
            }

            if (ipv4List.Count > 0)
            {
                return BuildDnsResponse(queryBuffer, ipv4List[0]);
            }

            return BuildEmptyNoErrorResponse(queryBuffer);
        }

        return BuildEmptyNoErrorResponse(queryBuffer);
    }

    private bool IsAccessAllowed(IPAddress clientAddress, string targetService)
    {
        var callerService = ResolveCallerService(clientAddress);
        if (string.IsNullOrEmpty(callerService))
        {
            // Caller is outside container subnets (host or mesh root)
            return true;
        }

        if (string.Equals(callerService, targetService, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Ingress proxy has global service visibility
        if (string.Equals(callerService, "ingress", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (_serviceNetworks.TryGetValue(callerService, out var callerNets) &&
            _serviceNetworks.TryGetValue(targetService, out var targetNets))
        {
            return callerNets.Overlaps(targetNets);
        }

        return false;
    }

    private string? ResolveCallerService(IPAddress clientAddress)
    {
        var prefix = ExtractSubnetPrefix64(clientAddress);
        if (!string.IsNullOrEmpty(prefix) && _subnetToService.TryGetValue(prefix, out var svc))
        {
            return svc;
        }
        return null;
    }

    private static string ExtractSubnetPrefix64(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetworkV6)
        {
            return string.Empty;
        }

        var bytes = address.GetAddressBytes();
        if (bytes.Length < 8) return string.Empty;

        return $"{bytes[0]:x2}{bytes[1]:x2}:{bytes[2]:x2}{bytes[3]:x2}:{bytes[4]:x2}{bytes[5]:x2}:{bytes[6]:x2}{bytes[7]:x2}";
    }

    private static string NormalizeDomain(string domain) =>
        domain.Trim().TrimEnd('.').ToUpperInvariant();

    private async Task<byte[]?> ForwardToUpstreamAsync(byte[] queryBuffer, CancellationToken ct)
    {
        try
        {
            using var upstreamClient = new UdpClient();
            var targetEndpoint = new IPEndPoint(UpstreamDns, 53);
            await upstreamClient.SendAsync(queryBuffer, queryBuffer.Length, targetEndpoint).ConfigureAwait(false);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linkedCts.CancelAfter(TimeSpan.FromSeconds(2));

            var result = await upstreamClient.ReceiveAsync(linkedCts.Token).ConfigureAwait(false);
            return result.Buffer;
        }
        catch (Exception ex) when (ex is SocketException or OperationCanceledException or IOException)
        {
            return null;
        }
    }

    private static byte[] BuildNxDomainResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x83; // NXDOMAIN
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
    }

    private static byte[] BuildRefusedResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x85; // REFUSED
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
    }

    private static byte[] BuildEmptyNoErrorResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
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

        index++; // Null terminator
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

        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = 0x00;
        response[7] = 0x01; // ANCOUNT = 1

        var offset = queryBuffer.Length;
        response[offset++] = 0xC0;
        response[offset++] = 0x0C; // Pointer to question name

        response[offset++] = 0x00;
        response[offset++] = 0x01; // Type A

        response[offset++] = 0x00;
        response[offset++] = 0x01; // Class IN

        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x3C; // TTL 60

        response[offset++] = 0x00;
        response[offset++] = 0x04; // Length 4

        var ipBytes = answerIp.GetAddressBytes();
        Array.Copy(ipBytes, 0, response, offset, 4);

        return response;
    }

    private static byte[] BuildAaaaResponse(byte[] queryBuffer, List<IPAddress> ipv6Addresses)
    {
        var count = ipv6Addresses.Count;
        var response = new byte[queryBuffer.Length + (count * 28)];
        Array.Copy(queryBuffer, response, queryBuffer.Length);

        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = (byte)(count >> 8);
        response[7] = (byte)(count & 0xFF); // ANCOUNT

        var offset = queryBuffer.Length;

        foreach (var ip in ipv6Addresses)
        {
            response[offset++] = 0xC0;
            response[offset++] = 0x0C; // Pointer to question name

            response[offset++] = 0x00;
            response[offset++] = 0x1C; // Type AAAA (28 = 0x001C)

            response[offset++] = 0x00;
            response[offset++] = 0x01; // Class IN

            response[offset++] = 0x00;
            response[offset++] = 0x00;
            response[offset++] = 0x00;
            response[offset++] = 0x3C; // TTL 60

            response[offset++] = 0x00;
            response[offset++] = 0x10; // Length 16 bytes for IPv6

            var ipBytes = ip.GetAddressBytes();
            Array.Copy(ipBytes, 0, response, offset, 16);
            offset += 16;
        }

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
                // Expected on cancellation
            }
            catch (SocketException)
            {
                // Expected on socket close
            }
        }

        _cts?.Dispose();
    }
}
