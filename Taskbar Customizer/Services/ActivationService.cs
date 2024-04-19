﻿// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.Activation;
using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Views;

/// <summary>
/// Service responsible for handling application activation and startup tasks.
/// </summary>
public class ActivationService : IActivationService
{
    /// <summary>
    /// The default activation handler.
    /// </summary>
    private readonly ActivationHandler<LaunchActivatedEventArgs> defaultHandler;

    /// <summary>
    /// The collection of activation handlers.
    /// </summary>
    private readonly IEnumerable<IActivationHandler> activationHandlers;

    /// <summary>
    /// The theme selector service.
    /// </summary>
    private readonly IThemeSelectorService themeSelectorService;

    /// <summary>
    /// The UI element representing the application shell.
    /// </summary>
    private UIElement? shell = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivationService"/> class.
    /// </summary>
    /// <param name="defaultHandler">The default activation handler.</param>
    /// <param name="activationHandlers">The collection of activation handlers.</param>
    /// <param name="themeSelectorService">The theme selector service.</param>
    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService)
    {
        this.defaultHandler = defaultHandler;
        this.activationHandlers = activationHandlers;
        this.themeSelectorService = themeSelectorService;
    }

    /// <inheritdoc />
    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await this.InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            this.shell = App.GetService<ShellPage>();
            App.MainWindow.Content = this.shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await this.HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await this.StartupAsync();
    }

    /// <summary>
    /// Method for handling activation asynchronously.
    /// </summary>
    /// <param name="activationArgs">Activation arguments.</param>
    /// <returns>Completed Task.</returns>
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = this.activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (this.defaultHandler.CanHandle(activationArgs))
        {
            await this.defaultHandler.HandleAsync(activationArgs);
        }
    }

    /// <summary>
    /// Method, which initializes the theme selector service.
    /// </summary>
    /// <returns>Completed Task.</returns>
    private async Task InitializeAsync()
    {
        await this.themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Method for setting the requested application theme after activation.
    /// </summary>
    /// <returns>Completed Task.</returns>
    private async Task StartupAsync()
    {
        await this.themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
