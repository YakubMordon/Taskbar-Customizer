// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer;

using System.Diagnostics;

using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Taskbar_Customizer.Contracts.Services.Configuration;
using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Services.Configuration;

/// <summary>
/// Code-Behind for App.xaml.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        NotificationManager.Initialize();

        BackgroundTaskRegistrationService.RegisterBackgroundTasks();

        this.Host = ConfigureHostService.Configure();

        this.UnhandledException += this.App_UnhandledException;
    }

    /// <summary>
    /// Gets the main application window.
    /// </summary>
    public static WindowEx MainWindow { get; } = new MainWindow();

    /// <summary>
    /// Gets or sets the custom application title bar element.
    /// </summary>
    public static UIElement? AppTitlebar
    {
        get; set;
    }

    /// <summary>
    /// Gets the host builder instance for the application.
    /// </summary>
    public IHost Host
    {
        get;
    }

    /// <summary>
    /// Method for retrieving a registered service of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>The registered service instance.</returns>
    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    /// <inheritdoc />
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    /// <summary>
    /// Method for handling unhandled exceptions raised by the application.
    /// </summary>
    /// <param name="sender">Sender of an exception.</param>
    /// <param name="e">Arguments of exception.</param>
    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Debug.WriteLine(e.ToString());
    }
}
