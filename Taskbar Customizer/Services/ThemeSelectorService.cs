// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Contracts.Services;

using Taskbar_Customizer.Helpers.Helpers;

/// <summary>
/// Service responsible for managing and applying the application theme.
/// </summary>
public class ThemeSelectorService : IThemeSelectorService
{
    /// <summary>
    /// Settings key.
    /// </summary>
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    /// <summary>
    /// The service used to access local settings.
    /// </summary>
    private readonly ILocalSettingsService localSettingsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeSelectorService"/> class.
    /// </summary>
    /// <param name="localSettingsService">The service used to access local settings.</param>
    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
    }

    /// <summary>
    /// Gets or sets the currently selected application theme.
    /// </summary>
    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        this.Theme = await this.LoadThemeFromSettingsAsync();

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task SetThemeAsync(ElementTheme theme)
    {
        this.Theme = theme;

        await this.SetRequestedThemeAsync();
        await this.SaveThemeInSettingsAsync(this.Theme);
    }

    /// <inheritdoc />
    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = this.Theme;

            TitleBarHelper.UpdateTitleBar(this.Theme);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Method for loading theme from settings asynchronously.
    /// </summary>
    /// <returns>Theme.</returns>
    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await this.localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    /// <summary>
    /// Method for saving theme into settings asynchronously.
    /// </summary>
    /// <param name="theme">Theme.</param>
    /// <returns>Completed Task.</returns>
    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await this.localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
