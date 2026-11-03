namespace Starry.Infrastructure.Network;

public sealed record AllocatedPortInfo(
    string RuleName,
    string From,
    string To,
    string Protocol,
    string Family);
