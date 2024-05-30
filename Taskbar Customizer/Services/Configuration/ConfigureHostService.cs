// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Activation;
using Taskbar_Customizer.Models;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

using Taskbar_Customizer.Helpers.Services;
using Taskbar_Customizer.Services.Taskbar;
using Taskbar_Customizer.Services.Navigation;
using Taskbar_Customizer.Contracts.Services.Navigation;
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
        AddConfigurationServices(services);
        AddNavigationServices(services);
        AddTaskbarServices(services);

        AddCoreServices(services);

        AddViewModels(services);
        AddViews(services);

        ConfigureContainer(context, services);
    }

    /// <summary>
    /// Method for adding configuration-related services to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddConfigurationServices(IServiceCollection services)
    {
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

        services.AddSingleton<IActivationService, ActivationService>();

        services.AddTransient<SynchronizationService>();
    }

    /// <summary>
    /// Method for adding navigation-related services to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddNavigationServices(IServiceCollection services)
    {
        services.AddTransient<INavigationViewService, NavigationViewService>();

        services.AddSingleton<IPageService, PageService>();

        services.AddSingleton<INavigationService, NavigationService>();
    }

    /// <summary>
    /// Method for adding taskbar-related services to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddTaskbarServices(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<TaskbarCustomizerService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    /// <summary>
    /// Method for adding core services to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddCoreServices(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(FileService))
            .AddClasses(classes => classes.InNamespaceOf<FileService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    /// <summary>
    /// Method for adding viewmodels to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddViewModels(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<MainViewModel>())
            .AsSelf()
            .WithTransientLifetime()
        );
    }

    /// <summary>
    /// Method for adding views to dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    private static void AddViews(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<MainPage>())
            .AsSelf()
            .WithTransientLifetime()
        );
    }

    /// <summary>
    /// Method for configuring container.
    /// </summary>
    /// <param name="context">Host builder context.</param>
    /// <param name="services">Collection of services.</param>
    private static void ConfigureContainer(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
    }
}
