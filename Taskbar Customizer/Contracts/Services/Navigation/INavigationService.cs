// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services.Navigation;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

/// <summary>
/// Contract, which represents a navigation service that allows navigating between pages within an application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Event that occurs when navigation to a new page is complete.
    /// </summary>
    event NavigatedEventHandler Navigated;

    /// <summary>
    /// Gets a value indicating whether there is at least one entry in the back navigation history.
    /// </summary>
    bool CanGoBack
    {
        get;
    }

    /// <summary>
    /// Gets or sets the frame used for navigation.
    /// </summary>
    Frame? Frame
    {
        get; set;
    }

    /// <summary>
    /// Method for navigation to the specified page identified by its unique key.
    /// </summary>
    /// <param name="pageKey">The unique key of the page to navigate to.</param>
    /// <param name="parameter">An optional parameter to pass to the target page.</param>
    /// <param name="clearNavigation">A flag indicating whether to clear the navigation history.</param>
    /// <returns>True if navigation succeeds; otherwise, false.</returns>
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    /// <summary>
    /// Method for navigation to the most recent entry in the back navigation history.
    /// </summary>
    /// <returns>True if navigation succeeds; otherwise, false.</returns>
    bool GoBack();
}
