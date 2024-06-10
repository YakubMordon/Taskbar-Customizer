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
    private const string SettingsKey = "AppBackgroundRequestedTheme";

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

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await this.localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await this.localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
