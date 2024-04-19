// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services;

using Microsoft.UI.Xaml.Controls;

/// <summary>
/// Contract, which represents a service for managing a NavigationView control.
/// </summary>
public interface INavigationViewService
{
    /// <summary>
    /// Gets the collection of menu items displayed in the NavigationView.
    /// </summary>
    IList<object>? MenuItems
    {
        get;
    }

    /// <summary>
    /// Gets the settings item displayed in the NavigationView.
    /// </summary>
    object? SettingsItem
    {
        get;
    }

    /// <summary>
    /// Method, which initializes the navigation service with the specified NavigationView control.
    /// </summary>
    /// <param name="navigationView">The NavigationView control to initialize.</param>
    void Initialize(NavigationView navigationView);

    /// <summary>
    /// Method for unregistration of event handlers and performing cleanup tasks.
    /// </summary>
    void UnregisterEvents();

    /// <summary>
    /// Method, which retrieves the selected NavigationViewItem associated with the specified page type.
    /// </summary>
    /// <param name="pageType">The type of the page for which to find the selected NavigationViewItem.</param>
    /// <returns>The selected NavigationViewItem, or null if no match is found.</returns>
    NavigationViewItem? GetSelectedItem(Type pageType);
}
