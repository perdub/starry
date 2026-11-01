using System.Security.Cryptography;
using System.Text;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.ChaosTests;

public class CryptographyChaosTests
{
    private readonly CryptoService _cryptoService = new();

    [Fact]
    public void Decrypt_WithCorruptedTag_ThrowsCryptographicException()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var path = "nodes.bocchi.ssh_key";
        var secret = "super-secret-ssh-key"u8.ToArray();

        var envelope = _cryptoService.Encrypt(secret, key, path);

        // Corrupt the tag bytes
        var tagBytes = Convert.FromBase64String(envelope.TagBase64);
        tagBytes[0] ^= 0xFF; // flip bits
        var corruptedEnvelope = envelope with { TagBase64 = Convert.ToBase64String(tagBytes) };

        Assert.ThrowsAny<CryptographicException>(() => _cryptoService.Decrypt(corruptedEnvelope, key, path));
    }

    [Fact]
    public void Decrypt_WithCorruptedCipher_ThrowsCryptographicException()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var path = "nodes.bocchi.ssh_key";
        var secret = "super-secret-ssh-key"u8.ToArray();

        var envelope = _cryptoService.Encrypt(secret, key, path);

        // Corrupt the ciphertext
        var cipherBytes = Convert.FromBase64String(envelope.CipherBase64);
        cipherBytes[0] ^= 0xAA;
        var corruptedEnvelope = envelope with { CipherBase64 = Convert.ToBase64String(cipherBytes) };

        Assert.ThrowsAny<CryptographicException>(() => _cryptoService.Decrypt(corruptedEnvelope, key, path));
    }

    [Fact]
    public void Decrypt_TransferredToDifferentJsonPath_ThrowsCryptographicExceptionDueToAadMismatch()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var originalPath = "nodes.bocchi.ssh_key";
        var attackerTargetField = "services.database.password"; // Attacker attempts to copy encrypted secret
        var secret = "bocchi-private-key-material"u8.ToArray();

        // Legitimate encryption bound to nodes.bocchi.ssh_key
        var envelope = _cryptoService.Encrypt(secret, key, originalPath);

        // Attempt to decrypt with another field's AAD must fail
        Assert.ThrowsAny<CryptographicException>(() =>
            _cryptoService.Decrypt(envelope, key, attackerTargetField));
    }

    [Fact]
    public void AtomicWriteFile_WritesAtomicallyWithoutLeavingTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_chaos_atomic_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var targetFile = Path.Combine(tempDir, "config.yaml");
            File.WriteAllText(targetFile, "initial: content\n");

            var newContent = "updated: true\nsecret: encrypted\n";
            SecretManager.AtomicWriteFile(targetFile, newContent);

            Assert.Equal(newContent, File.ReadAllText(targetFile));

            // Verify no leftover .tmp files
            var tmpFiles = Directory.GetFiles(tempDir, "*.tmp");
            Assert.Empty(tmpFiles);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
