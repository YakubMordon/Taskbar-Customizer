// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer;

using Taskbar_Customizer.Event_Handlers;
using Taskbar_Customizer.Helpers;
using Microsoft.UI.Dispatching;
using Windows.UI.ViewManagement;

/// <summary>
/// Code-Behind for MainWindow.xaml.
/// </summary>
public sealed partial class MainWindow : WindowEx
{
    /// <summary>
    /// Language change event handler.
    /// </summary>
    public LanguageChangeEventHandler EventHandler = new LanguageChangeEventHandler();

    /// <summary>
    /// Dispatcher queue for handling UI updates.
    /// </summary>
    private DispatcherQueue dispatcherQueue;

    /// <summary>
    /// Windows UI settings for system theme changes.
    /// </summary>
    private UISettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        this.Content = null;

        this.EventHandler.EventHandler += (sender, args) => this.UpdateUI();

        this.UpdateUI();

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        this.dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        this.settings = new UISettings();
        this.settings.ColorValuesChanged += this.Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    /// <summary>
    /// Event Handler for update of the caption button colors, when windows system theme is changed, while app is open.
    /// </summary>
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        this.dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }

    /// <summary>
    /// Method for updating UI.
    /// </summary>
    private void UpdateUI()
    {
        this.Title = "AppDisplayName".GetLocalized();
    }
}
