// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation.Singleton;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Taskbar_Customizer;
using Taskbar_Customizer.Core.Contracts.Services.Navigation;

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
            if (frame is null)
            {
                frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return frame;
        }

        set
        {
            UnregisterFrameEvents();
            frame = value;
            RegisterFrameEvents();
        }
    }

    /// <summary>
    /// Gets a value indicating whether it's possible to navigate back to the previous page.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Frame), nameof(frame))]
    public bool CanGoBack => Frame is not null && Frame.CanGoBack;

    /// <inheritdoc />
    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = frame.GetPageViewModel();

            frame.GoBack();

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
        var pageType = pageService.GetPageType(pageKey);

        if (frame is not null && (frame.Content?.GetType() != pageType || parameter is not null && !parameter.Equals(lastParameterUsed)))
        {
            frame.Tag = clearNavigation;
            var vmBeforeNavigation = frame.GetPageViewModel();
            var navigated = frame.Navigate(pageType, parameter);
            if (navigated)
            {
                lastParameterUsed = parameter;
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
        if (frame is not null)
        {
            frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (frame is not null)
        {
            frame.Navigated -= OnNavigated;
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

            Navigated?.Invoke(sender, e);
        }
    }
}
