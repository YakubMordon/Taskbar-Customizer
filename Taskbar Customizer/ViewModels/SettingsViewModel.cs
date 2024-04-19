// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Helpers;

using Windows.ApplicationModel;

/// <summary>
/// ViewModel for Settings Page.
/// </summary>
public partial class SettingsViewModel : ObservableRecipient
{
    /// <summary>
    /// Service for theme selection handling.
    /// </summary>
    private readonly IThemeSelectorService themeSelectorService;

    /// <summary>
    /// Current element theme.
    /// </summary>
    [ObservableProperty]
    private ElementTheme elementTheme;

    /// <summary>
    /// Version description of the app.
    /// </summary>
    [ObservableProperty]
    private string versionDescription;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="themeSelectorService">The theme selector service used to manage app themes.</param>
    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        this.themeSelectorService = themeSelectorService;
        this.elementTheme = this.themeSelectorService.Theme;
        this.versionDescription = GetVersionDescription();

        this.SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (this.ElementTheme != param)
                {
                    this.ElementTheme = param;
                    await this.themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    /// <summary>
    /// Gets the command to switch the app theme.
    /// </summary>
    public ICommand SwitchThemeCommand
    {
        get;
    }

    /// <summary>
    /// Method for retrieving the version description of the app.
    /// </summary>
    /// <returns>The version description string.</returns>
    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
