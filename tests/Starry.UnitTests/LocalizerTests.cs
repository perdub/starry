using Starry.Core.Localization;
using Starry.Infrastructure.Localization;
using Xunit;

namespace Starry.UnitTests;

public class LocalizerTests
{
    [Fact]
    public void Localizer_DefaultsToEnglish_WhenNotSpecified()
    {
        var localizer = new Localizer("en");

        Assert.Equal("en", localizer.CurrentLanguage);
        var desc = localizer.GetString(LocalizationKeys.CliDescription);
        Assert.Contains("Decentralized GitOps orchestrator", desc, StringComparison.Ordinal);
    }

    [Fact]
    public void Localizer_SwitchesToRussian_WhenRequested()
    {
        var localizer = new Localizer("ru");

        Assert.Equal("ru", localizer.CurrentLanguage);
        var desc = localizer.GetString(LocalizationKeys.CliDescription);
        Assert.Contains("децентрализованный GitOps-оркестратор", desc, StringComparison.Ordinal);
    }

    [Fact]
    public void Localizer_FormatString_InjectsArgumentsCorrectly()
    {
        var localizer = new Localizer("ru");

        var formatted = localizer.FormatString(LocalizationKeys.ValidationFailed, 3);
        Assert.Equal("Валидация конфигурации завершилась с 3 ошибками.", formatted);
    }

    [Fact]
    public void Localizer_FallbackToKey_WhenKeyNotFound()
    {
        var localizer = new Localizer("en");

        var result = localizer.GetString("non.existent.key");
        Assert.Equal("non.existent.key", result);
    }
}
