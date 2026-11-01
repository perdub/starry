using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.UnitTests;

public class CryptoServiceTests
{
    private readonly CryptoService _cryptoService = new();
    private readonly SecretManager _secretManager = new();

    [Fact]
    public void EncryptAndDecrypt_RoundTripsSuccessfully()
    {
        var key = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var path = "services.api.jwt_secret";
        var rawText = "very-secure-jwt-signing-key-12345";

        var envelope = _cryptoService.Encrypt(System.Text.Encoding.UTF8.GetBytes(rawText), key, path);
        Assert.NotNull(envelope);

        var encryptedFormatted = envelope.FormatEncrypted();
        Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedFormatted, StringComparison.Ordinal);

        Assert.True(_cryptoService.TryParseEncrypted(encryptedFormatted, out var parsedEnvelope));
        Assert.NotNull(parsedEnvelope);

        var decryptedBytes = _cryptoService.Decrypt(parsedEnvelope, key, path);
        var decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

        Assert.Equal(rawText, decryptedText);
    }

    [Fact]
    public void SecretManager_EncryptsOpenSecretsInTree()
    {
        var key = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[my-private-key]"));
        tree.SetNode("services.db.password", JsonValue.Create("PLAIN[db-pass-456]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "starry_sec_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var targetFile = Path.Combine(tempDir, "starry.yaml");
        File.WriteAllText(targetFile, "dummy: true\n");

        try
        {
            var count = _secretManager.EncryptOpenSecrets(tree, key, targetFile, new VirtualConfigEngine());
            Assert.Equal(2, count);

            var encryptedSsh = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(encryptedSsh);
            Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedSsh, StringComparison.Ordinal);

            Assert.True(_cryptoService.TryParseEncrypted(encryptedSsh, out var envelope));
            Assert.NotNull(envelope);
            var decrypted = _cryptoService.Decrypt(envelope, key, "nodes.bocchi.ssh_key");
            Assert.Equal("my-private-key", System.Text.Encoding.UTF8.GetString(decrypted));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void SecretManager_ReencryptsAllSecretsWithNewKey()
    {
        var oldKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var newKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());

        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[initial-secret]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "starry_resec_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var targetFile = Path.Combine(tempDir, "starry.yaml");
        File.WriteAllText(targetFile, "dummy: true\n");

        try
        {
            var engine = new VirtualConfigEngine();
            _secretManager.EncryptOpenSecrets(tree, oldKey, targetFile, engine);

            var reencryptedCount = _secretManager.ReencryptAll(tree, oldKey, newKey, targetFile, engine);
            Assert.Equal(1, reencryptedCount);

            var currentVal = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(currentVal);

            // Must decrypt with new key
            Assert.True(_cryptoService.TryParseEncrypted(currentVal, out var envelope));
            Assert.NotNull(envelope);
            var decrypted = _cryptoService.Decrypt(envelope, newKey, "nodes.bocchi.ssh_key");
            Assert.Equal("initial-secret", System.Text.Encoding.UTF8.GetString(decrypted));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
