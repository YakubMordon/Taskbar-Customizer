// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

using Taskbar_Customizer.Core.Contracts.Services.Taskbar;
using Taskbar_Customizer.Core.Services.Configuration;

using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Helpers.Extensions.Resource;

/// <summary>
/// ViewModel for Settings Page.
/// </summary>
public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService themeSelectorService;

    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    [ObservableProperty]
    private ElementTheme elementTheme;

    [ObservableProperty]
    private string versionDescription;

    [ObservableProperty]
    private string selectedLanguage;

    [ObservableProperty]
    private bool isSynchronizationOn;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="themeSelectorService">The theme selector service used to manage app themes.</param>
    /// <param name="taskbarCustomizerService">The taskbar customizer service.</param>
    public SettingsViewModel(IThemeSelectorService themeSelectorService, ITaskbarCustomizerService taskbarCustomizerService)
    {
        this.themeSelectorService = themeSelectorService;
        this.elementTheme = this.themeSelectorService.Theme;
        this.versionDescription = GetVersionDescription();

        this.taskbarCustomizerService = taskbarCustomizerService;

        this.Languages = new ObservableCollection<string>(LanguageHelper.AvailableLanguages);

        this.IsSynchronizationOn = SynchronizationService.IsSynchronizable;

        this.selectedLanguage = LanguageHelper.GetCurrentLanguage();
    }

    /// <summary>
    /// Gets collection of available languages in app.
    /// </summary>
    public ObservableCollection<string> Languages { get; }

    /// <summary>
    /// Method for updating version description.
    /// </summary>
    public void UpdateVersionDescription()
    {
        this.VersionDescription = GetVersionDescription();
    }

    private static string GetVersionDescription()
    {
        var packageVersion = Package.Current.Id.Version;

        var version = RuntimeHelper.IsMSIX
            ? new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision)
            : Assembly.GetExecutingAssembly().GetName().Version!;

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    [RelayCommand]
    private async Task SwitchTheme(ElementTheme theme)
    {
        if (this.ElementTheme != theme)
        {
            this.ElementTheme = theme;

            await this.themeSelectorService.SetThemeAsync(theme);

            NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationThemeChanged".GetLocalized());
        }
    }

    partial void OnIsSynchronizationOnChanged(bool value)
    {
        this.taskbarCustomizerService.SetSynchronization(value);
    }

    partial void OnSelectedLanguageChanging(string value)
    {
        LanguageHelper.SetCurrentLanguage(value);

        ((MainWindow)App.MainWindow).OnLanguageChanged();

        NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationLanguageChanged".GetLocalized());
    }
}
