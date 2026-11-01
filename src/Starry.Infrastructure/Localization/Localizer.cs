using System.Globalization;
using Starry.Core.Localization;

namespace Starry.Infrastructure.Localization;

/// <summary>
/// High-performance embedded localization engine supporting Russian and English.
/// </summary>
public sealed class Localizer : ILocalizer
{
    private string _currentLanguage = "en";

    private static readonly Dictionary<string, string> EnglishTranslations = new(StringComparer.Ordinal)
    {
        [LocalizationKeys.CliDescription] = "Starry - Decentralized GitOps orchestrator for Docker, Podman, WireGuard, nftables, and VPS fleet",
        [LocalizationKeys.CliVersion] = "Starry version {0}",
        [LocalizationKeys.CliLangOption] = "Set UI and output language (ru, en)",
        [LocalizationKeys.CliVerboseOption] = "Enable verbose debug logging",
        [LocalizationKeys.CliConfigOption] = "Path to configuration directory or file",

        [LocalizationKeys.ConfigCommandDescription] = "Manage, inspect, and validate Starry cluster configuration",
        [LocalizationKeys.ConfigValidateDescription] = "Validate syntax, schema, and secrets in configuration",
        [LocalizationKeys.ConfigPackDescription] = "Pack multi-file configuration directory into a single bundle file",
        [LocalizationKeys.ConfigSplitDescription] = "Split single bundle configuration into modular directory hierarchy",
        [LocalizationKeys.ConfigFmtDescription] = "Format configuration files canonically",
        [LocalizationKeys.ConfigSchemaDescription] = "Generate JSON Schema for Starry configuration",

        [LocalizationKeys.EncryptCommandDescription] = "Manage secret encryption and key rotation",
        [LocalizationKeys.EncryptGlobalDescription] = "Re-encrypt all secrets across configuration with a new master key",

        [LocalizationKeys.NodesCommandDescription] = "Manage and upgrade cluster nodes",
        [LocalizationKeys.NodesUpgradeDescription] = "Perform rolling OS security upgrade across cluster nodes",
        [LocalizationKeys.NodesRotateKeysDescription] = "Rotate SSH and WireGuard keys on cluster nodes",

        [LocalizationKeys.PlanCommandDescription] = "Generate and display migration execution plan (DAG)",
        [LocalizationKeys.ApplyCommandDescription] = "Apply planned changes to the cluster fleet",
        [LocalizationKeys.VolumeCommandDescription] = "Inspect, explore, or mount remote container volumes",
        [LocalizationKeys.BundleCommandDescription] = "Export or import disaster recovery bundle archive",

        [LocalizationKeys.ValidationSuccess] = "Configuration is valid.",
        [LocalizationKeys.ValidationFailed] = "Configuration validation failed with {0} error(s).",
        [LocalizationKeys.ErrorUnknownCommand] = "Unknown command: '{0}'. Run 'starry --help' for available commands.",
        [LocalizationKeys.ErrorFileNotFound] = "File or directory not found: '{0}'.",
        [LocalizationKeys.ErrorInvalidLanguage] = "Unsupported language code '{0}'. Supported languages: ru, en.",
        [LocalizationKeys.ErrorEncryptionFailed] = "Secret encryption failed for path '{0}': {1}",
        [LocalizationKeys.ErrorDecryptionFailed] = "Secret decryption failed for path '{0}': {1}"
    };

    private static readonly Dictionary<string, string> RussianTranslations = new(StringComparer.Ordinal)
    {
        [LocalizationKeys.CliDescription] = "Starry — децентрализованный GitOps-оркестратор для Docker, Podman, WireGuard, nftables и флота VPS",
        [LocalizationKeys.CliVersion] = "Версия Starry {0}",
        [LocalizationKeys.CliLangOption] = "Язык вывода и интерфейса (ru, en)",
        [LocalizationKeys.CliVerboseOption] = "Включить подробный вывод отладки",
        [LocalizationKeys.CliConfigOption] = "Путь к директории или файлу конфигурации",

        [LocalizationKeys.ConfigCommandDescription] = "Управление, проверка и валидация конфигурации кластера Starry",
        [LocalizationKeys.ConfigValidateDescription] = "Проверить синтаксис, схему и целостность секретов в конфигурации",
        [LocalizationKeys.ConfigPackDescription] = "Упаковать многофайловую структуру конфигурации в единый файл",
        [LocalizationKeys.ConfigSplitDescription] = "Разбить единую конфигурацию на модульную иерархию папок и файлов",
        [LocalizationKeys.ConfigFmtDescription] = "Канонически отформатировать файлы конфигурации",
        [LocalizationKeys.ConfigSchemaDescription] = "Сгенерировать JSON Schema для конфигурации Starry",

        [LocalizationKeys.EncryptCommandDescription] = "Управление шифрованием секретов и ротацией ключей",
        [LocalizationKeys.EncryptGlobalDescription] = "Перешифровать все секреты в конфигурации новым мастер-ключом",

        [LocalizationKeys.NodesCommandDescription] = "Управление нодами кластера и обновление",
        [LocalizationKeys.NodesUpgradeDescription] = "Выполнить скользящее (rolling) обновление безопасности ОС на нодах",
        [LocalizationKeys.NodesRotateKeysDescription] = "Ротировать SSH и WireGuard ключи на нодах кластера",

        [LocalizationKeys.PlanCommandDescription] = "Построить и отобразить граф плана миграций (DAG)",
        [LocalizationKeys.ApplyCommandDescription] = "Применить запланированные изменения к флоту серверов",
        [LocalizationKeys.VolumeCommandDescription] = "Просмотр, монтирование и управление томами контейнеров",
        [LocalizationKeys.BundleCommandDescription] = "Экспорт или импорт архива аварийного восстановления",

        [LocalizationKeys.ValidationSuccess] = "Конфигурация валидна и готова к применению.",
        [LocalizationKeys.ValidationFailed] = "Валидация конфигурации завершилась с {0} ошибками.",
        [LocalizationKeys.ErrorUnknownCommand] = "Неизвестная команда: '{0}'. Используйте 'starry --help' для справки.",
        [LocalizationKeys.ErrorFileNotFound] = "Файл или папка не найдены: '{0}'.",
        [LocalizationKeys.ErrorInvalidLanguage] = "Неподдерживаемый язык '{0}'. Доступные языки: ru, en.",
        [LocalizationKeys.ErrorEncryptionFailed] = "Ошибка шифрования секрета для пути '{0}': {1}",
        [LocalizationKeys.ErrorDecryptionFailed] = "Ошибка расшифровки секрета для пути '{0}': {1}"
    };

    public Localizer(string? defaultLanguage = null)
    {
        SetLanguage(ResolveInitialLanguage(defaultLanguage));
    }

    public string CurrentLanguage => _currentLanguage;

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            _currentLanguage = "en";
            return;
        }

        var trimmed = languageCode.Trim();
        if (string.Equals(trimmed, "ru", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(trimmed, "rus", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(trimmed, "russian", StringComparison.OrdinalIgnoreCase))
        {
            _currentLanguage = "ru";
        }
        else
        {
            _currentLanguage = "en";
        }
    }

    public string GetString(string key)
    {
        var dictionary = string.Equals(_currentLanguage, "ru", StringComparison.Ordinal)
            ? RussianTranslations
            : EnglishTranslations;

        if (dictionary.TryGetValue(key, out var val))
        {
            return val;
        }

        if (EnglishTranslations.TryGetValue(key, out var enVal))
        {
            return enVal;
        }

        return key;
    }

    public string FormatString(string key, params object?[] args)
    {
        var template = GetString(key);
        return string.Format(CultureInfo.InvariantCulture, template, args);
    }

    private static string ResolveInitialLanguage(string? defaultLanguage)
    {
        if (!string.IsNullOrWhiteSpace(defaultLanguage))
        {
            return defaultLanguage;
        }

        var envLang = Environment.GetEnvironmentVariable("STARRY_LANG");
        if (!string.IsNullOrWhiteSpace(envLang))
        {
            return envLang;
        }

        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }
}
