// Copyright (c) Digital Cloud Technologies. All rights reserved.

using CommunityToolkit.Mvvm.Messaging;
using Taskbar_Customizer.Helpers.Helpers.Native;

namespace Taskbar_Customizer;

using System.Diagnostics;

using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Taskbar_Customizer.Core.Contracts.Services.Configuration;

using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Helpers;

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
        NotificationManager.Initialize();

        BackgroundTaskRegistrationHelper.RegisterBackgroundTasks();

        this.Host = ConfigureHostHelper.Configure();

        User32Interop.SetProcessDpiAwarenessContext(-4);

        this.InitializeComponent();

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

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        e.Handled = true;

        Debug.WriteLine(e.ToString());
    }
}
