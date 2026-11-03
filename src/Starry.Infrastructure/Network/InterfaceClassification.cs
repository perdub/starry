namespace Starry.Infrastructure.Network;

public enum InterfaceClassification
{
    DedicatedPublic,
    RoutedIpv6Subnet,
    BehindNatOrCgnat,
    Firewalled,
    InternalOnly
}
