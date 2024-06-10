// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer;

using Microsoft.UI.Dispatching;

using Windows.UI.ViewManagement;

using Taskbar_Customizer.Helpers.Helpers.Taskbar;
using Taskbar_Customizer.Helpers.Extensions.Resource;

/// <summary>
/// Code-Behind for MainWindow.xaml.
/// </summary>
public sealed partial class MainWindow : WindowEx
{
    /// <summary>
    /// Language change event handler.
    /// </summary>
    public event EventHandler EventHandler;

    private readonly DispatcherQueue dispatcherQueue;

    private UISettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        this.Content = null;

        this.EventHandler += (sender, args) => this.UpdateUI();

        this.UpdateUI();

        this.dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        this.ConfigureUISettings();
    }

    /// <summary>
    /// Method for firing event of <see cref="EventHandler"/>.
    /// </summary>
    public void OnLanguageChanged()
    {
        this.EventHandler.Invoke(this, EventArgs.Empty);
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
