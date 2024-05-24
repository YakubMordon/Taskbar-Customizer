// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Helpers.Extensions.Resource;

/// <summary>
/// Code-Behind for SettingsPage.xaml.
/// </summary>
public sealed partial class SettingsPage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class.
    /// </summary>
    public SettingsPage()
    {
        ((MainWindow)App.MainWindow).EventHandler.EventHandler += (sender, e) => this.UpdateUI();

        this.ViewModel = App.GetService<SettingsViewModel>();
        this.InitializeComponent();

        this.UpdateUI();
    }

    /// <summary>
    /// Gets current ViewModel.
    /// </summary>
    public SettingsViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Method for updating UI.
    /// </summary>
    private void UpdateUI()
    {
        this.UpdatePersonalization();
        this.UpdateTheme();

        this.UpdateLanguage();

        this.UpdateAbout();
        this.UpdatePrivacy();

        this.ViewModel.UpdateVersionDescription();
    }

    /// <summary>
    /// Method for updating personalization elements.
    /// </summary>
    private void UpdatePersonalization()
    {
        this.Personalization.Text = "Personalization".GetLocalized();
    }

    /// <summary>
    /// Method for updating theme elements.
    /// </summary>
    private void UpdateTheme()
    {
        this.Theme.Text = "Theme".GetLocalized();
        this.ThemeDark.Content = "ThemeDark".GetLocalized();
        this.ThemeDefault.Content = "ThemeDefault".GetLocalized();
        this.ThemeLight.Content = "ThemeLight".GetLocalized();
    }

    /// <summary>
    /// Method for updating language elements.
    /// </summary>
    private void UpdateLanguage()
    {
        this.Language.Text = "Language".GetLocalized();
        this.LanguageWarning.Text = "LanguageWarning".GetLocalized();
    }

    /// <summary>
    /// Method for updating about elements.
    /// </summary>
    private void UpdateAbout()
    {
        this.About.Text = "About".GetLocalized();
        this.AboutDescription.Text = "AboutDescription".GetLocalized();
    }

    /// <summary>
    /// Method for updating privacy elements.
    /// </summary>
    private void UpdatePrivacy()
    {
        this.PrivacyTerms.Content = "PrivacyTerms".GetLocalized();
    }
}
