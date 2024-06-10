// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Taskbar_Customizer;
using Taskbar_Customizer.Contracts.Services.Navigation;
using Taskbar_Customizer.Contracts.ViewModels;

using Taskbar_Customizer.Helpers.Extensions.UI;

/// <summary>
/// Service, which provides navigation functionality between pages in a WinUI application.
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IPageService pageService;

    private object? lastParameterUsed;

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
            if (this.frame is null)
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
    public bool CanGoBack => this.Frame is not null && this.Frame.CanGoBack;

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

        if (this.frame is not null && (this.frame.Content?.GetType() != pageType || (parameter is not null && !parameter.Equals(this.lastParameterUsed))))
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

    private void RegisterFrameEvents()
    {
        if (this.frame is not null)
        {
            this.frame.Navigated += this.OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (this.frame is not null)
        {
            this.frame.Navigated -= this.OnNavigated;
        }
    }

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
