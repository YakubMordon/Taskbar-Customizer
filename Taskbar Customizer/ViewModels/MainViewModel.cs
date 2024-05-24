// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System.Drawing;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taskbar_Customizer.Contracts.Services.Taskbar;
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
    /// <summary>
    /// The taskbar customizer service.
    /// </summary>
    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    /// <summary>
    /// The color of the taskbar.
    /// </summary>
    private Color taskbarColor;

    /// <summary>
    /// Indicates whether the taskbar is transparent.
    /// </summary>
    private bool isTaskbarTransparent;

    /// <summary>
    /// Indicates whether the Start button is positioned on the left.
    /// </summary>
    private bool isStartButtonLeft;

    /// <summary>
    /// Indicates whether the Start button is positioned in the center.
    /// </summary>
    private bool isStartButtonCenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="taskbarCustomizerService">The taskbar customizer service.</param>
    public MainViewModel(ITaskbarCustomizerService taskbarCustomizerService)
    {
        this.taskbarCustomizerService = taskbarCustomizerService;

        this.InitializeProperties();

        this.ResetToDefaultCommand = new RelayCommand(this.ResetToDefault);
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
                this.taskbarColor.A = (byte)(this.IsTaskbarTransparent ? 128 : 255);
                this.taskbarCustomizerService.SetTaskbarColor(this.taskbarColor);
                NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationColorChanged".GetLocalized());
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
                this.taskbarCustomizerService.SetTaskbarTransparent(this.isTaskbarTransparent);
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
        set
        {
            if (this.SetProperty(ref this.isStartButtonLeft, value))
            {
                this.taskbarCustomizerService.SetStartButtonPosition(this.isStartButtonLeft);
            }
        }
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
                NotificationManager.ShowNotification("AppDisplayName".GetLocalized(), "NotificationAlignmentChanged".GetLocalized());
            }
        }
    }

    /// <summary>
    /// Gets command for reseting taskbar settings to default.
    /// </summary>
    public ICommand ResetToDefaultCommand { get; }

    /// <summary>
    /// Method for initialization of properties.
    /// </summary>
    private void InitializeProperties()
    {
        this.InitializeColor();
        this.InitializeTransparency();
        this.InitializeStartButtons();
    }

    /// <summary>
    /// Method for initialization of color.
    /// </summary>
    private void InitializeColor()
    {
        this.taskbarColor = this.taskbarCustomizerService.TaskbarColor;
    }

    /// <summary>
    /// Method for initialization of transparency.
    /// </summary>
    private void InitializeTransparency()
    {
        this.isTaskbarTransparent = this.taskbarCustomizerService.IsTaskbarTransparent;
    }

    /// <summary>
    /// Method for initialization of start buttons.
    /// </summary>
    private void InitializeStartButtons()
    {
        var isStartButtonLeft = this.taskbarCustomizerService.IsStartButtonLeft;

        this.isStartButtonLeft = isStartButtonLeft;
        this.isStartButtonCenter = !isStartButtonLeft;
    }

    /// <summary>
    /// Method for reseting taskbar settings to default.
    /// </summary>
    private void ResetToDefault()
    {
        var color = SystemColors.MenuBar.ToUIColor();

        this.TaskbarColor = color;

        this.IsTaskbarTransparent = color.Transparent();

        this.IsStartButtonCenter = OperationSystemChecker.IsWindows11OrGreater();

        this.IsStartButtonLeft = !this.IsStartButtonCenter;
    }
}
