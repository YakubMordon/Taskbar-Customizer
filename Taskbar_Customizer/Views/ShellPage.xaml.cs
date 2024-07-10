// Copyright (c) Digital Cloud Technologies. All rights reserved.

using CommunityToolkit.Mvvm.Messaging;

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using Taskbar_Customizer.Helpers;

using Taskbar_Customizer.ViewModels;

using Taskbar_Customizer.Core.Contracts.Services.Navigation;

using Taskbar_Customizer.Helpers.Extensions.Resource;

using Windows.System;
using Taskbar_Customizer.Models.Messages;

/// <summary>
/// Code-Behind for ShellPage.xaml.
/// </summary>
public sealed partial class ShellPage : Page
{
    private readonly IMessenger messenger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellPage"/> class.
    /// </summary>
    /// <param name="viewModel">View Model.</param>
    public ShellPage(ShellViewModel viewModel)
    {
        this.messenger = App.GetService<IMessenger>();

        this.messenger.Register<LanguageChangedMessage>(this, (r, m) => this.UpdateUI());

        this.ViewModel = viewModel;

        this.InitializeComponent();

        this.UpdateUI();

        this.ViewModel.NavigationService.Frame = this.NavigationFrame;
        this.ViewModel.NavigationViewService.Initialize(this.NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(this.AppTitleBar);
        App.MainWindow.Activated += this.MainWindow_Activated;
    }

    /// <summary>
    /// Gets current ViewModel.
    /// </summary>
    public ShellViewModel ViewModel
    {
        get;
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(this.RequestedTheme);

        this.KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        this.KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = this.AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        this.AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = this.AppTitleBar.Margin.Top,
            Right = this.AppTitleBar.Margin.Right,
            Bottom = this.AppTitleBar.Margin.Bottom,
        };
    }

    private void UpdateUI()
    {
        this.MainItem.Content = "Main".GetLocalized();

        this.AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }
}
