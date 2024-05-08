// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Taskbar_Customizer.Activation;
using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Models;
using Taskbar_Customizer.Services;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

using Taskbar_Customizer.Helpers.Contracts.Services;
using Taskbar_Customizer.Helpers.Services;

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

        this.Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ITaskbarCustomizerService, TaskbarCustomizerService>();

            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

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
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }
}
