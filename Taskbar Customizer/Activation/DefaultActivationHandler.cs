// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Activation;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.ViewModels;

/// <summary>
/// Default activation handler class for launching the application.
/// </summary>
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    /// <summary>
    /// Service for handling navigation.
    /// </summary>
    private readonly INavigationService navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultActivationHandler"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service used for navigation.</param>
    public DefaultActivationHandler(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    /// <summary>
    /// Method for determine whether this activation handler can handle the provided activation arguments.
    /// </summary>
    /// <param name="args">The activation arguments.</param>
    /// <returns>True if this activation handler can handle the activation; otherwise, false.</returns>
    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return this.navigationService.Frame?.Content == null;
    }

    /// <summary>
    /// Asynchronous Event Handler for activation.
    /// </summary>
    /// <param name="args">The activation arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        this.navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
