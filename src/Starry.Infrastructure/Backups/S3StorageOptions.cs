namespace Starry.Infrastructure.Backups;

public sealed record S3StorageOptions
{
    public string Bucket { get; init; } = string.Empty;
    public string? Endpoint { get; init; }
    public string? Region { get; init; } = "us-east-1";
    public string? AccessKey { get; init; }
    public string? SecretKey { get; init; }
    public string? Prefix { get; init; }
}
