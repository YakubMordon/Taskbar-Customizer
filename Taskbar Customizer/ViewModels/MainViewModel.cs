// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Taskbar_Customizer.Contracts.Services;
using Windows.UI;

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
    public Color taskbarColor;

    /// <summary>
    /// Indicates whether the taskbar is transparent.
    /// </summary>
    public bool isTaskbarTransparent;

    /// <summary>
    /// Indicates whether the Start button is positioned on the left.
    /// </summary>
    public bool isStartButtonLeft;

    /// <summary>
    /// Indicates whether the Start button is positioned in the center.
    /// </summary>
    public bool isStartButtonCenter;

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
    }

    /// <summary>
    /// Gets or sets color of the taskbar.
    /// </summary>
    public Color TaskbarColor
    {
        get => this.taskbarColor;
        set
        {
            if (SetProperty(ref this.taskbarColor, value))
            {
                this.taskbarCustomizerService.SetTaskbarColor(this.taskbarColor);
            }
        }
    }

    /// <summary>
    /// Gets or sets indicator whether the taskbar is transparent.
    /// </summary>
    public bool IsTaskbarTransparent
    {
        get => this.isTaskbarTransparent;
        set
        {
            if (SetProperty(ref this.isTaskbarTransparent, value))
            {
                this.taskbarCustomizerService.SetTaskbarTransparent(this.isTaskbarTransparent);
            }
        }
    }

    /// <summary>
    /// Gets or sets indicator whether the Start button is positioned on the left.
    /// </summary>
    public bool IsStartButtonLeft
    {
        get => this.isStartButtonLeft;
        set
        {
            if (SetProperty(ref this.isStartButtonLeft, value))
            {
                this.taskbarCustomizerService.SetStartButtonPosition(this.isStartButtonLeft);
            }
        }
    }

    /// <summary>
    /// Gets or sets indicator whether the Start button is positioned in the center.
    /// </summary>
    public bool IsStartButtonCenter
    {
        get => this.isStartButtonCenter;
        set
        {
            if (SetProperty(ref this.isStartButtonCenter, value))
            {
                this.taskbarCustomizerService.SetStartButtonPosition(!this.isStartButtonCenter);
            }
        }
    }
}
