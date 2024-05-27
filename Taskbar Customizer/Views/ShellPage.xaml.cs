// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Taskbar_Customizer.ViewModels;

using Windows.System;
using Taskbar_Customizer.Helpers.Helpers.Taskbar;
using Taskbar_Customizer.Helpers.Extensions.Resource;
using Taskbar_Customizer.Contracts.Services.Navigation;

/// <summary>
/// Code-Behind for ShellPage.xaml.
/// </summary>
public sealed partial class ShellPage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellPage"/> class.
    /// </summary>
    /// <param name="viewModel">View Model.</param>
    public ShellPage(ShellViewModel viewModel)
    {
        ((MainWindow)App.MainWindow).EventHandler.EventHandler += (sender, e) => this.UpdateUI();

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

    /// <summary>
    /// Method for building a keyboard accelerator with the specified key and optional modifiers.
    /// </summary>
    /// <param name="key">The virtual key to use for the keyboard accelerator.</param>
    /// <param name="modifiers">Optional modifiers for the keyboard accelerator.</param>
    /// <returns>The constructed <see cref="KeyboardAccelerator"/>.</returns>
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

    /// <summary>
    /// Event handler for keyboard accelerator invocation.
    /// </summary>
    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    /// <summary>
    /// Event Handler for Loading event for the ShellPage.
    /// </summary>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(this.RequestedTheme);

        this.KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        this.KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    /// <summary>
    /// Event Handler for Window Activation event.
    /// </summary>
    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = this.AppTitleBarText as UIElement;
    }

    /// <summary>
    /// Event Handler for DisplayModeChanged event of the NavigationViewControl.
    /// </summary>
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

    /// <summary>
    /// Method for updating UI.
    /// </summary>
    private void UpdateUI()
    {
        this.MainItem.Content = "Main".GetLocalized();

        this.AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }
}
