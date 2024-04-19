// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Contracts.ViewModels;
using Taskbar_Customizer.Helpers;

/// <summary>
/// Service, which provides navigation functionality between pages in a WinUI application.
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    /// Service for retrieving page type, based on key.
    /// </summary>
    private readonly IPageService pageService;

    /// <summary>
    /// Last parameter used to navigate.
    /// </summary>
    private object? lastParameterUsed;

    /// <summary>
    /// Frame control used for navigation.
    /// </summary>
    private Frame? frame;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="pageService">Service for retrieving page type, based on key.</param>
    public NavigationService(IPageService pageService)
    {
        this.pageService = pageService;
    }

    /// <summary>
    /// Occurs when a navigation operation has completed.
    /// </summary>
    public event NavigatedEventHandler? Navigated;

    /// <summary>
    /// Gets or sets the Frame control used for navigation.
    /// </summary>
    public Frame? Frame
    {
        get
        {
            if (this.frame == null)
            {
                this.frame = App.MainWindow.Content as Frame;
                this.RegisterFrameEvents();
            }

            return this.frame;
        }

        set
        {
            this.UnregisterFrameEvents();
            this.frame = value;
            this.RegisterFrameEvents();
        }
    }

    /// <summary>
    /// Gets a value indicating whether it's possible to navigate back to the previous page.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Frame), nameof(frame))]
    public bool CanGoBack => this.Frame != null && this.Frame.CanGoBack;

    /// <inheritdoc />
    public bool GoBack()
    {
        if (this.CanGoBack)
        {
            var vmBeforeNavigation = this.frame.GetPageViewModel();
            this.frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = this.pageService.GetPageType(pageKey);

        if (this.frame != null && (this.frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(this.lastParameterUsed))))
        {
            this.frame.Tag = clearNavigation;
            var vmBeforeNavigation = this.frame.GetPageViewModel();
            var navigated = this.frame.Navigate(pageType, parameter);
            if (navigated)
            {
                this.lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    /// <summary>
    /// Method for registering event handlers for the navigation events of the associated frame.
    /// </summary>
    private void RegisterFrameEvents()
    {
        if (this.frame != null)
        {
            this.frame.Navigated += this.OnNavigated;
        }
    }

    /// <summary>
    /// Method for unregistering event handlers for the navigation events of the associated frame.
    /// </summary>
    private void UnregisterFrameEvents()
    {
        if (this.frame != null)
        {
            this.frame.Navigated -= this.OnNavigated;
        }
    }

    /// <summary>
    /// Event Handler for the Navigated event of the frame.
    /// </summary>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            this.Navigated?.Invoke(sender, e);
        }
    }
}
