namespace Starry.Core.Security;

/// <summary>
/// AES-256-GCM authenticated encryption service with canonical JSON path AAD binding.
/// </summary>
public interface ICryptoService
{
    /// <summary>
    /// Encrypts plaintext bytes with AES-256-GCM using the specified key and JSON path as AAD.
    /// </summary>
    SecretEnvelope Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, string canonicalJsonPath);

    /// <summary>
    /// Decrypts ciphertext envelope with AES-256-GCM using the specified key and validating AAD.
    /// </summary>
    byte[] Decrypt(SecretEnvelope envelope, ReadOnlySpan<byte> key, string canonicalJsonPath);

    /// <summary>
    /// Parses an ENC[...] string into SecretEnvelope.
    /// </summary>
    bool TryParseEncrypted(string value, out SecretEnvelope? envelope);
}
