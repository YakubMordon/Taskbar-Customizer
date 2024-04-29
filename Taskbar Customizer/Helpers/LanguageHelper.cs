// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using Windows.Globalization;

/// <summary>
/// Helper class to get/set language in application.
/// </summary>
public class LanguageHelper
{
    /// <summary>
    /// Gets available languages in application.
    /// </summary>
    public static List<string> AvailableLanguages { get; }

    /// <summary>
    /// Abbreviations for languages. Index of language is the same as abbreviation language.
    /// </summary>
    private static List<string> cultureAbbreviations;

    /// <summary>
    /// Initializes static members of <see cref="LanguageHelper"/> class.
    /// </summary>
    static LanguageHelper()
    {
        AvailableLanguages = new List<string>
        {
            "English", "Українська",
        };

        cultureAbbreviations = new List<string>
        {
            "en", "uk",
        };
    }

    /// <summary>
    /// Static method for getting language.
    /// </summary>
    /// <returns>Language analogue to abbreviation.</returns>
    public static string GetCurrentLanguage()
    {
        var index = cultureAbbreviations.IndexOf(ApplicationLanguages.PrimaryLanguageOverride.ToLower());
        return AvailableLanguages[index];
    }

    /// <summary>
    /// Static method for setting language.
    /// </summary>
    /// <param name="language">Language.</param>
    public static void SetCurrentLanguage(string language)
    {
        var index = AvailableLanguages.IndexOf(language);
        var culture = cultureAbbreviations[index];

        ApplicationLanguages.PrimaryLanguageOverride = culture;
    }
}