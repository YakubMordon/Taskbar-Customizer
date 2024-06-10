// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Activation;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.ViewModels;

using Taskbar_Customizer.Core.Activation;

using Taskbar_Customizer.Core.Contracts.Services.Navigation;

/// <summary>
/// Default activation handler class for launching the application.
/// </summary>
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultActivationHandler"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service used for navigation.</param>
    public DefaultActivationHandler(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return this.navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        this.navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
