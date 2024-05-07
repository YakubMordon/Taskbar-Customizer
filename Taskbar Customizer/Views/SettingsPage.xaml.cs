// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.Helpers;
using Taskbar_Customizer.ViewModels;

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
        this.Personalization.Text = "Personalization".GetLocalized();

        this.Theme.Text = "Theme".GetLocalized();
        this.ThemeDark.Content = "ThemeDark".GetLocalized();
        this.ThemeDefault.Content = "ThemeDefault".GetLocalized();
        this.ThemeLight.Content = "ThemeLight".GetLocalized();

        this.Language.Text = "Language".GetLocalized();
        this.LanguageWarning.Text = "LanguageWarning".GetLocalized();

        this.About.Text = "About".GetLocalized();
        this.AboutDescription.Text = "AboutDescription".GetLocalized();

        this.PrivacyTerms.Content = "PrivacyTerms".GetLocalized();

        this.ViewModel.UpdateVersionDescription();
    }
}
