// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Helpers.Extensions.Resource;
using Taskbar_Customizer.Services.Configuration;
using Taskbar_Customizer.Contracts.Services.Taskbar;

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
    /// The taskbar customizer service.
    /// </summary>
    private readonly ITaskbarCustomizerService taskbarCustomizerService;

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
    /// Selected language of the app.
    /// </summary>
    private string selectedLanguage;

    /// <summary>
    /// Indicates whether synchronization is on.
    /// </summary>
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

        this.SwitchThemeCommand = new RelayCommand<ElementTheme>(this.SwitchTheme);
    }

    /// <summary>
    /// Gets collection of available languages in app.
    /// </summary>
    public ObservableCollection<string> Languages { get; }

    /// <summary>
    /// Gets or sets indicator, which indicates whether synchronization is on.
    /// </summary>
    public bool IsSynchronizationOn
    {
        get => this.isSynchronizationOn;
        set
        {
            if (this.SetProperty(ref this.isSynchronizationOn, value))
            {
                this.taskbarCustomizerService.SetSynchronization(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets selected language of the app.
    /// </summary>
    public string SelectedLanguage
    {
        get => this.selectedLanguage;
        set
        {
            if (this.SetProperty(ref this.selectedLanguage, value))
            {
                this.UpdateLanguage();
                NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationLanguageChanged".GetLocalized());
            }
        }
    }

    /// <summary>
    /// Gets the command to switch the app theme.
    /// </summary>
    public ICommand SwitchThemeCommand
    {
        get;
    }

    /// <summary>
    /// Method for updating language in application.
    /// </summary>
    public void UpdateLanguage()
    {
        LanguageHelper.SetCurrentLanguage(this.selectedLanguage);

        ((MainWindow)App.MainWindow).EventHandler.OnLanguageChanged();
    }

    /// <summary>
    /// Method for updating version description.
    /// </summary>
    public void UpdateVersionDescription()
    {
        this.VersionDescription = GetVersionDescription();
    }

    /// <summary>
    /// Method for retrieving the version description of the app.
    /// </summary>
    /// <returns>The version description string.</returns>
    private static string GetVersionDescription()
    {
        var packageVersion = Package.Current.Id.Version;

        var version = RuntimeHelper.IsMSIX
            ? new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision)
            : Assembly.GetExecutingAssembly().GetName().Version!;

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    /// <summary>
    /// Method for switching theme of application theme.
    /// </summary>
    /// <param name="theme">Theme of application.</param>
    private async void SwitchTheme(ElementTheme theme)
    {
        if (this.ElementTheme != theme)
        {
            this.ElementTheme = theme;

            await this.themeSelectorService.SetThemeAsync(theme);

            NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationThemeChanged".GetLocalized());
        }
    }
}
