// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;
using Taskbar_Customizer.Contracts.Services.Navigation;
using Taskbar_Customizer.Views;

/// <summary>
/// ViewModel for Shell Page.
/// </summary>
public partial class ShellViewModel : ObservableRecipient
{
    /// <summary>
    /// Value indicating whether the back navigation is enabled.
    /// </summary>
    [ObservableProperty]
    private bool isBackEnabled;

    /// <summary>
    /// Currently selected navigation item.
    /// </summary>
    [ObservableProperty]
    private object? selected;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service used for navigating between pages.</param>
    /// <param name="navigationViewService">The navigation view service used for managing navigation view items.</param>
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        this.NavigationService = navigationService;
        this.NavigationService.Navigated += this.OnNavigated;

        this.NavigationViewService = navigationViewService;
    }

    /// <summary>
    /// Gets the navigation service instance used for navigating between pages.
    /// </summary>
    public INavigationService NavigationService
    {
        get;
    }

    /// <summary>
    /// Gets the navigation view service instance used for managing navigation view items.
    /// </summary>
    public INavigationViewService NavigationViewService
    {
        get;
    }

    /// <summary>
    /// Event Handler for navigation event.
    /// </summary>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        this.IsBackEnabled = this.NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            this.Selected = this.NavigationViewService.SettingsItem;
        }
        else
        {
            var selectedItem = this.NavigationViewService.GetSelectedItem(e.SourcePageType);
            if (selectedItem != null)
            {
                this.Selected = selectedItem;
            }
        }
    }
}
