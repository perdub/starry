namespace Starry.Infrastructure.Security;

/// <summary>
/// Resolves the 32-byte master encryption key in priority order:
/// 1. Explicit CLI argument / flag.
/// 2. Environment variable STARRY_MASTER_KEY.
/// 3. Local file &lt;configDir&gt;/.starry/master.key or ./.starry/master.key.
/// </summary>
public static class MasterKeyResolver
{
    public static byte[]? ResolveKey(string? explicitKeyOrFlag = null, string? configDir = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitKeyOrFlag))
        {
            var key = TryParse(explicitKeyOrFlag);
            if (key != null)
            {
                return key;
            }
        }

        var envVal = Environment.GetEnvironmentVariable("STARRY_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(envVal))
        {
            var key = TryParse(envVal);
            if (key != null)
            {
                return key;
            }
        }

        if (!string.IsNullOrWhiteSpace(configDir))
        {
            var candidate1 = Path.Combine(configDir, ".starry", "master.key");
            if (File.Exists(candidate1))
            {
                var key = TryParse(File.ReadAllText(candidate1).Trim());
                if (key != null)
                {
                    return key;
                }
            }
        }

        var candidate2 = Path.Combine(Directory.GetCurrentDirectory(), ".starry", "master.key");
        if (File.Exists(candidate2))
        {
            var key = TryParse(File.ReadAllText(candidate2).Trim());
            if (key != null)
            {
                return key;
            }
        }

        return null;
    }

    private static byte[]? TryParse(string candidate)
    {
        try
        {
            return CryptoService.ParseMasterKey(candidate);
        }
        catch (FormatException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
