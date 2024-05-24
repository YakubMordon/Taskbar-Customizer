// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Taskbar;

using Microsoft.UI.Xaml;
using Taskbar_Customizer;
using Taskbar_Customizer.Contracts.Services.Configuration;
using Taskbar_Customizer.Contracts.Services.Taskbar;
using Taskbar_Customizer.Helpers.Helpers.Taskbar;

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
        Theme = await LoadThemeFromSettingsAsync();

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    /// <inheritdoc />
    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Method for loading theme from settings asynchronously.
    /// </summary>
    /// <returns>Theme.</returns>
    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await localSettingsService.ReadSettingAsync<string>(SettingsKey);

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
        await localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
