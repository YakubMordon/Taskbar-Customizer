// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;
using Taskbar_Customizer.Contracts.Services.Navigation;
using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

/// <summary>
/// Service responsible for managing page navigation within the application.
/// </summary>
public class PageService : IPageService
{
    /// <summary>
    /// Represents the mapping between ViewModel types (keys) and corresponding Page types (values) for page navigation.
    /// </summary>
    private readonly Dictionary<string, Type> pages = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PageService"/> class.
    /// </summary>
    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<SettingsViewModel, SettingsPage>();
    }

    /// <inheritdoc />
    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (pages)
        {
            if (!pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    /// <summary>
    /// Configures a ViewModel to Page mapping for navigation.
    /// </summary>
    /// <typeparam name="TVm">The type of the ViewModel.</typeparam>
    /// <typeparam name="TV">The type of the corresponding Page.</typeparam>
    /// <exception cref="ArgumentException">Thrown if the mapping is already configured.</exception>
    private void Configure<TVm, TV>()
        where TVm : ObservableObject
        where TV : Page
    {
        lock (pages)
        {
            var key = typeof(TVm).FullName!;
            if (pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {pages.First(p => p.Value == type).Key}");
            }

            pages.Add(key, type);
        }
    }
}
