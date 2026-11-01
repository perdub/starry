namespace Starry.Core.Localization;

/// <summary>
/// Service contract for application localization supporting Russian and English.
/// </summary>
public interface ILocalizer
{
    /// <summary>
    /// Gets the current active language code (e.g. "ru", "en").
    /// </summary>
    string CurrentLanguage { get; }

    /// <summary>
    /// Sets the active language for localization.
    /// </summary>
    /// <param name="languageCode">ISO language code (e.g. "ru", "en").</param>
    void SetLanguage(string languageCode);

    /// <summary>
    /// Retrieves the localized string by key.
    /// </summary>
    /// <param name="key">Localization resource key.</param>
    /// <returns>Localized string or key fallback.</returns>
    string GetString(string key);

    /// <summary>
    /// Retrieves the formatted localized string by key and arguments.
    /// </summary>
    /// <param name="key">Localization resource key.</param>
    /// <param name="args">Format arguments.</param>
    /// <returns>Formatted localized string.</returns>
    string FormatString(string key, params object?[] args);
}
