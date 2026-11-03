namespace Starry.Infrastructure.Security;

public sealed record SecretItemInfo(string Path, bool IsEncrypted, string? SourceFile);
