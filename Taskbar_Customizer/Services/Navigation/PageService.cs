// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.Core.Contracts.Services.Navigation;

using Taskbar_Customizer.ViewModels;
using Taskbar_Customizer.Views;

/// <summary>
/// Service responsible for managing page navigation within the application.
/// </summary>
public class PageService : IPageService
{
    private readonly Dictionary<string, Type> pages = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="PageService"/> class.
    /// </summary>
    public PageService()
    {
        this.Configure<MainViewModel, MainPage>();
        this.Configure<SettingsViewModel, SettingsPage>();
    }

    /// <inheritdoc />
    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (this.pages)
        {
            if (!this.pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<TVm, TV>()
        where TVm : ObservableObject
        where TV : Page
    {
        lock (this.pages)
        {
            var key = typeof(TVm).FullName!;
            if (this.pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (this.pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {this.pages.First(p => p.Value == type).Key}");
            }

            this.pages.Add(key, type);
        }
    }
}
