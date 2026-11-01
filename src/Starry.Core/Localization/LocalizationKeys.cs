namespace Starry.Core.Localization;

/// <summary>
/// Strongly-typed string constants for all localization keys across Starry.
/// </summary>
public static class LocalizationKeys
{
    public const string CliDescription = "cli.description";
    public const string CliVersion = "cli.version";
    public const string CliLangOption = "cli.option.lang";
    public const string CliVerboseOption = "cli.option.verbose";
    public const string CliConfigOption = "cli.option.config";

    public const string ConfigCommandDescription = "cmd.config.description";
    public const string ConfigValidateDescription = "cmd.config.validate.description";
    public const string ConfigPackDescription = "cmd.config.pack.description";
    public const string ConfigSplitDescription = "cmd.config.split.description";
    public const string ConfigFmtDescription = "cmd.config.fmt.description";
    public const string ConfigSchemaDescription = "cmd.config.schema.description";
    public const string ConfigPackSuccess = "cmd.config.pack.success";
    public const string ConfigSplitSuccess = "cmd.config.split.success";
    public const string ConfigFmtSuccess = "cmd.config.fmt.success";

    public const string EncryptCommandDescription = "cmd.encrypt.description";
    public const string EncryptGlobalDescription = "cmd.encrypt.global.description";

    public const string NodesCommandDescription = "cmd.nodes.description";
    public const string NodesUpgradeDescription = "cmd.nodes.upgrade.description";
    public const string NodesRotateKeysDescription = "cmd.nodes.rotate_keys.description";

    public const string PlanCommandDescription = "cmd.plan.description";
    public const string ApplyCommandDescription = "cmd.apply.description";
    public const string VolumeCommandDescription = "cmd.volume.description";
    public const string BundleCommandDescription = "cmd.bundle.description";

    public const string ValidationSuccess = "validation.success";
    public const string ValidationFailed = "validation.failed";
    public const string ErrorUnknownCommand = "error.unknown_command";
    public const string ErrorFileNotFound = "error.file_not_found";
    public const string ErrorInvalidLanguage = "error.invalid_language";
    public const string ErrorEncryptionFailed = "error.encryption_failed";
    public const string ErrorDecryptionFailed = "error.decryption_failed";
}
