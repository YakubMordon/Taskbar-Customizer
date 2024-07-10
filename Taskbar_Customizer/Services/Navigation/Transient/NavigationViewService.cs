// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation.Transient;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.ViewModels;

using Taskbar_Customizer.Core.Contracts.Services.Navigation;

using Taskbar_Customizer.Helpers.Helpers.Application;

/// <summary>
/// Service for managing navigation within a NavigationView control.
/// </summary>
public class NavigationViewService : INavigationViewService
{
    private readonly INavigationService navigationService;

    private readonly IPageService pageService;

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

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => navigationService.GoBack();

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

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            return pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }
}
