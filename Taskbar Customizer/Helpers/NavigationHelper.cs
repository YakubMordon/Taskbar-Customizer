// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

/// <summary>
/// Helper class to set the navigation target for a NavigationViewItem.
/// </summary>
public static class NavigationHelper
{
    /// <summary>
    /// Method for getting the navigation target associated with the specified NavigationViewItem.
    /// </summary>
    /// <param name="item">The NavigationViewItem.</param>
    /// <returns>The navigation target as a string.</returns>
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    /// <summary>
    /// Method for setting the navigation target for the specified NavigationViewItem.
    /// </summary>
    /// <param name="item">The NavigationViewItem.</param>
    /// <param name="value">The navigation target as a string.</param>
    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    /// <summary>
    /// NavigateTo property.
    /// </summary>
    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
}
