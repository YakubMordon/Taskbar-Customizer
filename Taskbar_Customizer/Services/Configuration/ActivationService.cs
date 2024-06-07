// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Configuration;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer;
using Taskbar_Customizer.Activation;
using Taskbar_Customizer.Contracts.Services.Configuration;
using Taskbar_Customizer.Contracts.Services.Taskbar;
using Taskbar_Customizer.Views;

/// <summary>
/// Service responsible for handling application activation and startup tasks.
/// </summary>
public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> defaultHandler;

    private readonly IEnumerable<IActivationHandler> activationHandlers;

    private readonly IThemeSelectorService themeSelectorService;

    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    private UIElement? shell = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivationService"/> class.
    /// </summary>
    /// <param name="defaultHandler">The default activation handler.</param>
    /// <param name="activationHandlers">The collection of activation handlers.</param>
    /// <param name="themeSelectorService">The theme selector service.</param>
    /// <param name="taskbarCustomizerService">The taskbar customizer service.</param>
    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService,
        ITaskbarCustomizerService taskbarCustomizerService)
    {
        this.defaultHandler = defaultHandler;
        this.activationHandlers = activationHandlers;
        this.themeSelectorService = themeSelectorService;
        this.taskbarCustomizerService = taskbarCustomizerService;
    }

    /// <inheritdoc />
    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await this.InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content is null)
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

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = this.activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler is not null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (this.defaultHandler.CanHandle(activationArgs))
        {
            await this.defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await this.themeSelectorService.InitializeAsync().ConfigureAwait(false);

        await this.taskbarCustomizerService.InitializeAsync().ConfigureAwait(false);

        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await this.themeSelectorService.SetRequestedThemeAsync();

        await Task.CompletedTask;
    }
}
