// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.ViewModels;

/// <summary>
/// Code-Behind for MainPage.xaml.
/// </summary>
public sealed partial class MainPage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
        this.ViewModel = App.GetService<MainViewModel>();
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets current ViewModel.
    /// </summary>
    public MainViewModel ViewModel
    {
        get;
    }
}
