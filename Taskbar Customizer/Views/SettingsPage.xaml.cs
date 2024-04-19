// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;

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
        this.ViewModel = App.GetService<SettingsViewModel>();
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets current ViewModel.
    /// </summary>
    public SettingsViewModel ViewModel
    {
        get;
    }
}
