namespace Starry.Infrastructure.Network;

public sealed record PortForwardRule(
    int HostPort,
    string Protocol,
    string TargetVip,
    int TargetPort);
