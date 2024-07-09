// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System;
using System.Drawing;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Newtonsoft.Json;

using Taskbar_Customizer.Core.Services.Configuration;
using Taskbar_Customizer.Core.Contracts.Services.Taskbar;

using Taskbar_Customizer.Helpers.Extensions.Resource;
using Taskbar_Customizer.Helpers.Extensions.UI;
using Taskbar_Customizer.Helpers.Helpers.Application;
using Taskbar_Customizer.Helpers.Helpers.Taskbar;

using Color = Windows.UI.Color;

/// <summary>
/// ViewModel for Main Page.
/// </summary>
public partial class MainViewModel : ObservableRecipient
{
    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    private readonly SynchronizationService synchronizationService;

    private readonly DispatcherTimer debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300), };

    [ObservableProperty]
    private Color taskbarColor;

    [ObservableProperty]
    private bool isTaskbarTransparent;

    [ObservableProperty]
    private bool isStartButtonLeft;

    [ObservableProperty]
    private bool isStartButtonCenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="taskbarCustomizerService">The taskbar customizer service.</param>
    /// <param name="synchronizationService">Synchronization service.</param>
    public MainViewModel(ITaskbarCustomizerService taskbarCustomizerService, SynchronizationService synchronizationService)
    {
        this.taskbarCustomizerService = taskbarCustomizerService;

        this.synchronizationService = synchronizationService;

        this.UpdateProperties();

        this.debounceTimer.Tick += (sender, e) => this.OnDebounceTimerTick();
    }

    private void UpdateProperties()
    {
        this.UpdateColor();
        this.UpdateTransparency();
        this.UpdateStartButtons();
    }

    private void UpdateColor()
    {
        this.taskbarColor = this.taskbarCustomizerService.TaskbarColor;
    }

    private void UpdateTransparency()
    {
        this.isTaskbarTransparent = this.taskbarCustomizerService.IsTaskbarTransparent;
    }

    private void UpdateStartButtons()
    {
        var startButtonLeftState = this.taskbarCustomizerService.IsStartButtonLeft;

        this.isStartButtonLeft = startButtonLeftState;
        this.isStartButtonCenter = !startButtonLeftState;
    }

    [RelayCommand]
    private void ResetToDefault()
    {
        this.ResetColorRelated();

        this.ResetStartButtons();
    }

    private void ResetColorRelated()
    {
        this.ResetColor();

        this.ResetTransparency();
    }

    private void ResetColor()
    {
        var isLightThemeEnabled = Application.Current.RequestedTheme == ApplicationTheme.Light;

        this.TaskbarColor = isLightThemeEnabled ? SystemColors.ControlLight.ToUIColor() : SystemColors.ControlDark.ToUIColor();
    }

    private void ResetTransparency()
    {
        this.IsTaskbarTransparent = SystemColors.MenuBar
                                        .ToUIColor()
                                        .Transparent();
    }

    private void ResetStartButtons()
    {
        this.IsStartButtonCenter = OperationSystemChecker.IsWindows11OrGreater();

        this.IsStartButtonLeft = !this.IsStartButtonCenter;
    }

    private void OnDebounceTimerTick()
    {
        this.debounceTimer.Stop();
        this.ApplyDebouncedChanges();
    }

    private void ApplyDebouncedChanges()
    {
        this.taskbarCustomizerService.SetTaskbarColor(this.TaskbarColor);
        this.synchronizationService.CallSyncService("Color", JsonConvert.SerializeObject(this.TaskbarColor));
        NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationColorChanged".GetLocalized());
    }

    partial void OnTaskbarColorChanged(Color value)
    {
        this.debounceTimer.Stop();
        this.debounceTimer.Start();
    }

    partial void OnIsStartButtonCenterChanged(bool value)
    {
        this.taskbarCustomizerService.SetStartButtonPosition(!value);

        this.synchronizationService.CallSyncService("Alignment", JsonConvert.SerializeObject(value));

        NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationAlignmentChanged".GetLocalized());
    }

    partial void OnIsTaskbarTransparentChanged(bool value)
    {
        this.taskbarCustomizerService.SetTaskbarTransparent(value);

        this.synchronizationService.CallSyncService("Transparency", JsonConvert.SerializeObject(value));

        NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationTransparencyChanged".GetLocalized());
    }
}
