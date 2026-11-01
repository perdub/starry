using System.Security.Cryptography;
using System.Text;
using Starry.Core.Security;

namespace Starry.Infrastructure.Security;

/// <summary>
/// High-performance AES-256-GCM encryption service with canonical JSON path AAD binding.
/// </summary>
public sealed class CryptoService : ICryptoService
{
    private const string EncryptedPrefix = "ENC[AES256_GCM:v1:";
    private const char EncryptedSuffix = ']';
    public const int KeySizeBytes = 32;
    public const int NonceSizeBytes = 12;
    public const int TagSizeBytes = 16;

    public SecretEnvelope Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, string canonicalJsonPath)
    {
        if (key.Length != KeySizeBytes)
        {
            throw new ArgumentException($"Master key must be exactly {KeySizeBytes} bytes (256 bits), got {key.Length} bytes.", nameof(key));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalJsonPath);

        Span<byte> nonce = stackalloc byte[NonceSizeBytes];
        RandomNumberGenerator.Fill(nonce);

        var cipher = new byte[plaintext.Length];
        Span<byte> tag = stackalloc byte[TagSizeBytes];

        var aad = Encoding.UTF8.GetBytes(canonicalJsonPath);

        using var aes = new AesGcm(key, TagSizeBytes);
        aes.Encrypt(nonce, plaintext, cipher, tag, aad);

        return new SecretEnvelope(
            Convert.ToBase64String(nonce),
            Convert.ToBase64String(cipher),
            Convert.ToBase64String(tag));
    }

    public byte[] Decrypt(SecretEnvelope envelope, ReadOnlySpan<byte> key, string canonicalJsonPath)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        if (key.Length != KeySizeBytes)
        {
            throw new ArgumentException($"Master key must be exactly {KeySizeBytes} bytes (256 bits), got {key.Length} bytes.", nameof(key));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalJsonPath);

        var nonce = Convert.FromBase64String(envelope.NonceBase64);
        var cipher = Convert.FromBase64String(envelope.CipherBase64);
        var tag = Convert.FromBase64String(envelope.TagBase64);

        var plaintext = new byte[cipher.Length];
        var aad = Encoding.UTF8.GetBytes(canonicalJsonPath);

        using var aes = new AesGcm(key, TagSizeBytes);
        aes.Decrypt(nonce, cipher, tag, plaintext, aad);

        return plaintext;
    }

    public bool TryParseEncrypted(string value, out SecretEnvelope? envelope)
    {
        envelope = null;
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith(EncryptedPrefix, StringComparison.Ordinal) || !value.EndsWith(EncryptedSuffix))
        {
            return false;
        }

        var inner = value[EncryptedPrefix.Length..^1];
        var parts = inner.Split(':', StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
        {
            return false;
        }

        envelope = new SecretEnvelope(parts[0], parts[1], parts[2]);
        return true;
    }

    public static string GenerateMasterKeyBase64()
    {
        Span<byte> key = stackalloc byte[KeySizeBytes];
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }

    public static byte[] ParseMasterKey(string keyString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString);

        var trimmed = keyString.Trim();
        try
        {
            var bytes = Convert.FromBase64String(trimmed);
            if (bytes.Length == KeySizeBytes)
            {
                return bytes;
            }
        }
        catch (FormatException)
        {
            // Try hex decoding if not base64
        }

        if (trimmed.Length == KeySizeBytes * 2)
        {
            return Convert.FromHexString(trimmed);
        }

        var utf8 = Encoding.UTF8.GetBytes(trimmed);
        if (utf8.Length == KeySizeBytes)
        {
            return utf8;
        }

        throw new ArgumentException($"Invalid master key length. Key must be 32 bytes (Base64 string or 64 hex characters). Got '{trimmed.Length}' characters.", nameof(keyString));
    }
}
