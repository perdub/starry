namespace Starry.Core.Security;

public sealed record SecretEnvelope(
    string NonceBase64,
    string CipherBase64,
    string TagBase64)
{
    public string FormatEncrypted() => $"ENC[AES256_GCM:v1:{NonceBase64}:{CipherBase64}:{TagBase64}]";
}
