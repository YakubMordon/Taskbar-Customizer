// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Windows.UI;

/// <summary>
/// ViewModel for Main Page.
/// </summary>
public partial class MainViewModel : ObservableRecipient
{
    /// <summary>
    /// The color of the taskbar.
    /// </summary>
    [ObservableProperty]
    public Color taskbarColor;

    /// <summary>
    /// Indicates whether the taskbar is transparent.
    /// </summary>
    [ObservableProperty]
    public bool isTaskbarTransparent;

    /// <summary>
    /// Indicates whether the Start button is positioned on the left.
    /// </summary>
    [ObservableProperty]
    public bool isStartButtonLeft;

    /// <summary>
    /// Indicates whether the Start button is positioned on the center.
    /// </summary>
    [ObservableProperty]
    public bool isStartButtonCenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        taskbarColor = Colors.Blue;
        isTaskbarTransparent = false;
        isStartButtonLeft = true; 
        isStartButtonCenter = false;
    }
}
