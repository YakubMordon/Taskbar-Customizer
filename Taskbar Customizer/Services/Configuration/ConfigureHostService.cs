// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Activation;
using Taskbar_Customizer.Models;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

using Taskbar_Customizer.Helpers.Contracts.Services;
using Taskbar_Customizer.Helpers.Services;
using Taskbar_Customizer.Services.Taskbar;
using Taskbar_Customizer.Services.Navigation;
using Taskbar_Customizer.Contracts.Services.Navigation;
using Taskbar_Customizer.Contracts.Services.Taskbar;
using Taskbar_Customizer.Contracts.Services.Configuration;

/// <summary>
/// Static service for configurating host of application.
/// </summary>
public static class ConfigureHostService
{
    /// <summary>
    /// Method for configuration host of application.
    /// </summary>
    /// <returns>Configured host of application.</returns>
    public static IHost Configure()
    {
        var host = Host
            .CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices(ConfigureServices)
            .Build();

        return host;
    }

    /// <summary>
    /// Method for configuring services, necessary for Dependency Injection.
    /// </summary>
    /// <param name="context">Host builder context.</param>
    /// <param name="services">Collection of services.</param>
    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Default Activation Handler
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        // Other Activation Handlers

        // Services
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<ITaskbarCustomizerService, TaskbarCustomizerService>();

        services.AddTransient<INavigationViewService, NavigationViewService>();
        services.AddTransient<SynchronizationService>();

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
    }
}
