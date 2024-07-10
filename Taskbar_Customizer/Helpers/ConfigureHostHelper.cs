// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.UI.Xaml;

using Taskbar_Customizer.Activation;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

using Taskbar_Customizer.Services.Configuration;
using Taskbar_Customizer.Services.Taskbar;

using Taskbar_Customizer.Core.Activation;

using Taskbar_Customizer.Core.Contracts.Services.Configuration;

using Taskbar_Customizer.Core.Services.Configuration;
using Taskbar_Customizer.Core.Services.Files;

using Taskbar_Customizer.Models;
using Taskbar_Customizer.Services.Navigation.Transient;
using Taskbar_Customizer.Services.Navigation.Singleton;

/// <summary>
/// Static service for configurating host of application.
/// </summary>
public static class ConfigureHostHelper
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

    private static void AddConfigurationServices(IServiceCollection services)
    {
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<ActivationService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<SynchronizationService>()
            .AddClasses(classes => classes.InNamespaceOf<SynchronizationService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    private static void AddNavigationServices(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<NavigationService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<NavigationViewService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    private static void AddTaskbarServices(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<TaskbarCustomizerService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    private static void AddCoreServices(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(FileService))
            .AddClasses(classes => classes.InNamespaceOf<FileService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(MainViewModel), typeof(App))
            .AddClasses(classes => classes.InNamespaceOf<MainViewModel>())
            .AsSelf()
            .WithTransientLifetime());
    }

    private static void AddViews(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.InNamespaceOf<MainPage>())
            .AsSelf()
            .WithTransientLifetime());
    }

    private static void ConfigureContainer(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
    }
}
