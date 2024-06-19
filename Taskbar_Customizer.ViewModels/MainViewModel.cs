// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System;
using System.Drawing;
using System.Windows.Input;

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
public class MainViewModel : ObservableRecipient
{
    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    private readonly SynchronizationService synchronizationService;

    private DispatcherTimer debounceTimer;

    private Color taskbarColor;

    private bool isTaskbarTransparent;

    private bool isStartButtonLeft;

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

        this.ResetToDefaultCommand = new RelayCommand(this.ResetToDefault);

        this.debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };
        this.debounceTimer.Tick += (sender, e) => this.OnDebounceTimerTick();
    }

    /// <summary>
    /// Gets or sets color of the taskbar.
    /// </summary>
    public Color TaskbarColor
    {
        get => this.taskbarColor;
        set
        {
            if (this.SetProperty(ref this.taskbarColor, value))
            {
                this.taskbarColor.A = (byte)(this.isTaskbarTransparent ? 128 : 255);
                this.debounceTimer.Stop();
                this.debounceTimer.Start();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the taskbar is transparent.
    /// </summary>
    public bool IsTaskbarTransparent
    {
        get => this.isTaskbarTransparent;
        set
        {
            if (this.SetProperty(ref this.isTaskbarTransparent, value))
            {
                this.ResetColor();

                this.taskbarCustomizerService.SetTaskbarTransparent(this.isTaskbarTransparent);

                this.synchronizationService.CallSyncService("Transparency", JsonConvert.SerializeObject(this.isTaskbarTransparent));

                NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationTransparencyChanged".GetLocalized());
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Start button is positioned on the left.
    /// </summary>
    public bool IsStartButtonLeft
    {
        get => this.isStartButtonLeft;
        set => this.SetProperty(ref this.isStartButtonLeft, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Start button is positioned in the center.
    /// </summary>
    public bool IsStartButtonCenter
    {
        get => this.isStartButtonCenter;
        set
        {
            if (this.SetProperty(ref this.isStartButtonCenter, value))
            {
                this.taskbarCustomizerService.SetStartButtonPosition(!this.isStartButtonCenter);

                this.synchronizationService.CallSyncService("Alignment", JsonConvert.SerializeObject(this.isStartButtonCenter));

                NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationAlignmentChanged".GetLocalized());
            }
        }
    }

    /// <summary>
    /// Gets command for reseting taskbar settings to default.
    /// </summary>
    public ICommand ResetToDefaultCommand { get; }

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
        var isStartButtonLeft = this.taskbarCustomizerService.IsStartButtonLeft;

        this.isStartButtonLeft = isStartButtonLeft;
        this.isStartButtonCenter = !isStartButtonLeft;
    }

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
        this.taskbarCustomizerService.SetTaskbarColor(this.taskbarColor);
        this.synchronizationService.CallSyncService("Color", JsonConvert.SerializeObject(this.taskbarColor));
        NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationColorChanged".GetLocalized());
    }
}
