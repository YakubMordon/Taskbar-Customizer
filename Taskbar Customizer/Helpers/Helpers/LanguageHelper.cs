// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers;

using Windows.Globalization;

/// <summary>
/// Helper class to get/set language in application.
/// </summary>
public class LanguageHelper
{
    /// <summary>
    /// Abbreviations for languages. Index of language is the same as abbreviation language.
    /// </summary>
    private static readonly List<string> CultureAbbreviations;

    /// <summary>
    /// Initializes static members of the <see cref="LanguageHelper"/> class.
    /// </summary>
    static LanguageHelper()
    {
        AvailableLanguages = new List<string>
        {
            "English", "Українська",
        };

        CultureAbbreviations = new List<string>
        {
            "en", "uk",
        };
    }

    /// <summary>
    /// Gets available languages in application.
    /// </summary>
    public static List<string> AvailableLanguages
    {
        get;
    }

    /// <summary>
    /// Static method for getting language.
    /// </summary>
    /// <returns>Language analogue to abbreviation.</returns>
    public static string GetCurrentLanguage()
    {
        ApplicationLanguages.PrimaryLanguageOverride ??= CultureAbbreviations.First();

        var index = CultureAbbreviations.IndexOf(ApplicationLanguages.PrimaryLanguageOverride.ToLower());

        return AvailableLanguages[index];
    }

    /// <summary>
    /// Static method for setting language.
    /// </summary>
    /// <param name="language">Language.</param>
    public static void SetCurrentLanguage(string language)
    {
        var index = AvailableLanguages.IndexOf(language);

        var culture = CultureAbbreviations[index];

        ApplicationLanguages.PrimaryLanguageOverride = culture;
    }
}