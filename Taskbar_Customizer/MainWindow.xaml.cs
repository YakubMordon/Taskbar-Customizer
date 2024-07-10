// Copyright (c) Digital Cloud Technologies. All rights reserved.

using Taskbar_Customizer.Models.Messages;

namespace Taskbar_Customizer;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.UI.Dispatching;

using Taskbar_Customizer.Helpers;

using Taskbar_Customizer.Helpers.Extensions.Resource;

using Windows.UI.ViewManagement;

/// <summary>
/// Code-Behind for MainWindow.xaml.
/// </summary>
public sealed partial class MainWindow : WindowEx
{
    private readonly IMessenger messenger;

    private readonly DispatcherQueue dispatcherQueue;

    private UISettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.messenger = App.GetService<IMessenger>();

        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        this.Content = null;

        this.messenger.Register<LanguageChangedMessage>(this, (r, m) => this.UpdateUI());

        this.UpdateUI();

        this.dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        this.ConfigureUISettings();
    }

    private void ConfigureUISettings()
    {
        this.settings = new UISettings();

        this.settings.ColorValuesChanged += this.Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        this.dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }

    private void UpdateUI()
    {
        this.Title = "AppDisplayName".GetLocalized();
    }
}
