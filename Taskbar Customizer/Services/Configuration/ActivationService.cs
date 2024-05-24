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
    /// The taskbar customizer service.
    /// </summary>
    private readonly ITaskbarCustomizerService taskbarCustomizerService;

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
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content is null)
        {
            shell = App.GetService<ShellPage>();
            App.MainWindow.Content = shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    /// <summary>
    /// Method for handling activation asynchronously.
    /// </summary>
    /// <param name="activationArgs">Activation arguments.</param>
    /// <returns>Completed Task.</returns>
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler is not null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (defaultHandler.CanHandle(activationArgs))
        {
            await defaultHandler.HandleAsync(activationArgs);
        }
    }

    /// <summary>
    /// Method, which initializes the singleton services.
    /// </summary>
    /// <returns>Completed Task.</returns>
    private async Task InitializeAsync()
    {
        await themeSelectorService.InitializeAsync().ConfigureAwait(false);

        await taskbarCustomizerService.InitializeAsync().ConfigureAwait(false);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Method for setting the requested application theme after activation.
    /// </summary>
    /// <returns>Completed Task.</returns>
    private async Task StartupAsync()
    {
        await themeSelectorService.SetRequestedThemeAsync();

        await Task.CompletedTask;
    }
}
