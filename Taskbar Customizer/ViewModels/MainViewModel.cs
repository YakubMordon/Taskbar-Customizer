// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using System.Drawing;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Services;
using Color = Windows.UI.Color;
using ColorConverter = Taskbar_Customizer.Services.ColorConverter;

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

        this.taskbarColor = this.taskbarCustomizerService.TaskbarColor;
        this.isTaskbarTransparent = this.taskbarCustomizerService.IsTaskbarTransparent;
        this.isStartButtonLeft = this.taskbarCustomizerService.IsStartButtonLeft;
        this.isStartButtonCenter = !this.taskbarCustomizerService.IsStartButtonLeft;

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
            }
        }
    }

    /// <summary>
    /// Gets command for reseting taskbar settings to default.
    /// </summary>
    public ICommand ResetToDefaultCommand { get; }

    /// <summary>
    /// Method for reseting taskbar settings to default.
    /// </summary>
    private void ResetToDefault()
    {
        this.TaskbarColor = ColorConverter.ToUIColor(SystemColors.MenuBar);

        this.IsTaskbarTransparent = SystemColors.MenuBar.A != 255;

        var startButtonPosition = OperationSystemChecker.IsWindows11OrGreater();

        this.IsStartButtonCenter = startButtonPosition;
        this.IsStartButtonLeft = !startButtonPosition;
    }
}
