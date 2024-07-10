// Copyright (c) Digital Cloud Technologies. All rights reserved.

using CommunityToolkit.Mvvm.Messaging;

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Helpers.Extensions.Resource;
using Taskbar_Customizer.Models.Messages;

/// <summary>
/// Code-Behind for MainPage.xaml.
/// </summary>
public sealed partial class MainPage : Page
{
    private readonly IMessenger messenger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
        this.messenger = App.GetService<IMessenger>();

        this.messenger.Register<LanguageChangedMessage>(this, (r, m) => this.UpdateUI());

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

    private void UpdateUI()
    {
        this.UpdateColorPicker();
        this.UpdateTransparency();
        this.UpdateStartPosition();
        this.UpdateResetButton();
    }

    private void UpdateColorPicker()
    {
        this.ColorPickerTextBlock.Text = "ColorPicker".GetLocalized();

        this.ColorPickerStateTextBlock.Text = "ColorPickerStateTextBlock".GetLocalized();
    }

    private void UpdateTransparency()
    {
        this.TransparentTextBlock.Text = "Transparent".GetLocalized();
        this.TransparentSwitch.OffContent = "TransparentSwitchOff".GetLocalized();
        this.TransparentSwitch.OnContent = "TransparentSwitchOn".GetLocalized();
    }

    private void UpdateStartPosition()
    {
        this.StartTextBlock.Text = "Start".GetLocalized();
        this.StartLeftRadio.Content = "StartLeft".GetLocalized();
        this.StartCenterRadio.Content = "StartCenter".GetLocalized();
    }

    private void UpdateResetButton()
    {
        this.ResetTextBlock.Text = "Reset".GetLocalized();
        this.ResetButton.Content = "ResetButton".GetLocalized();
    }
}
