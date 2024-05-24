// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Contracts.Services.Navigation;

/// <summary>
/// Service for managing navigation within a NavigationView control.
/// </summary>
public class NavigationViewService : INavigationViewService
{
    /// <summary>
    /// The navigation service to use for page navigation.
    /// </summary>
    private readonly INavigationService navigationService;

    /// <summary>
    /// The page service to retrieve page types.
    /// </summary>
    private readonly IPageService pageService;

    /// <summary>
    /// Navigation View.
    /// </summary>
    private NavigationView? navigationView;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationViewService"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service to use for page navigation.</param>
    /// <param name="pageService">The page service to retrieve page types.</param>
    public NavigationViewService(INavigationService navigationService, IPageService pageService)
    {
        this.navigationService = navigationService;
        this.pageService = pageService;
    }

    /// <summary>
    /// Gets the collection of menu items associated with the NavigationView control.
    /// </summary>
    public IList<object>? MenuItems => navigationView?.MenuItems;

    /// <summary>
    /// Gets the settings item associated with the NavigationView control.
    /// </summary>
    public object? SettingsItem => navigationView?.SettingsItem;

    /// <inheritdoc />
    [MemberNotNull(nameof(NavigationViewService.navigationView))]
    public void Initialize(NavigationView navigationView)
    {
        this.navigationView = navigationView;
        this.navigationView.BackRequested += OnBackRequested;
        this.navigationView.ItemInvoked += OnItemInvoked;
    }

    /// <inheritdoc />
    public void UnregisterEvents()
    {
        if (navigationView is not null)
        {
            navigationView.BackRequested -= OnBackRequested;
            navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    /// <inheritdoc />
    public NavigationViewItem? GetSelectedItem(Type pageType)
    {
        if (navigationView is not null)
        {
            return GetSelectedItem(navigationView.MenuItems, pageType) ?? GetSelectedItem(navigationView.FooterMenuItems, pageType);
        }

        return null;
    }

    /// <summary>
    /// Event Handler for the back button request in the navigation view by navigating to the previous page.
    /// </summary>
    /// <param name="sender">The <see cref="NavigationView"/> instance that triggered the event.</param>
    /// <param name="args">The <see cref="NavigationViewBackRequestedEventArgs"/> containing event data.</param>
    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => navigationService.GoBack();

    /// <summary>
    /// Event Handler for the item invoked event in the navigation view.
    /// </summary>
    /// <param name="sender">The <see cref="NavigationView"/> instance that triggered the event.</param>
    /// <param name="args">The <see cref="NavigationViewItemInvokedEventArgs"/> containing event data.</param>
    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                navigationService.NavigateTo(pageKey);
            }
        }
    }

    /// <summary>
    /// Method, which recursively searches for the selected navigation item that corresponds to the specified page type within a collection of menu items.
    /// </summary>
    /// <param name="menuItems">The collection of menu items to search within.</param>
    /// <param name="pageType">The type of the page to find the selected item for.</param>
    /// <returns>The <see cref="NavigationViewItem"/> that corresponds to the specified page type, or <c>null</c> if not found.</returns>
    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(item.MenuItems, pageType);

            if (selectedChild is not null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    /// <summary>
    /// Method, which determines whether the specified navigation menu item corresponds to the specified page type.
    /// </summary>
    /// <param name="menuItem">The navigation menu item to check.</param>
    /// <param name="sourcePageType">The type of the page to compare against.</param>
    /// <returns><c>true</c> if the menu item corresponds to the specified page type; otherwise, <c>false</c>.</returns>
    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            return pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }
}
