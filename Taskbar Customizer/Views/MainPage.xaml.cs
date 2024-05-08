// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Helpers.Extensions;

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
        ((MainWindow)App.MainWindow).EventHandler.EventHandler += (sender, e) => this.UpdateUI();

        this.ViewModel = App.GetService<MainViewModel>();
        this.InitializeComponent();

        this.UpdateUI();
    }

    /// <summary>
    /// Gets current ViewModel.
    /// </summary>
    public MainViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Method for updating UI.
    /// </summary>
    private void UpdateUI()
    {
        this.ColorPickerTextBlock.Text = "ColorPicker".GetLocalized();

        this.TransparentTextBlock.Text = "Transparent".GetLocalized();
        this.TransparentSwitch.OffContent = "TransparentSwitchOff".GetLocalized();
        this.TransparentSwitch.OnContent = "TransparentSwitchOn".GetLocalized();

        this.StartTextBlock.Text = "Start".GetLocalized();
        this.StartLeftRadio.Content = "StartLeft".GetLocalized();
        this.StartCenterRadio.Content = "StartCenter".GetLocalized();

        this.ResetTextBlock.Text = "Reset".GetLocalized();
        this.ResetButton.Content = "ResetButton".GetLocalized();
    }
}
