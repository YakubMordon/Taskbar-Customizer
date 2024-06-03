// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services.Taskbar;

using Windows.UI;

/// <summary>
/// Contract for customizing and managing taskbar actions.
/// </summary>
public interface ITaskbarCustomizerService
{
    /// <summary>
    /// Gets the color of the taskbar.
    /// </summary>
    public Color TaskbarColor
    {
        get;
    }

    /// <summary>
    /// Gets a value indicating whether the taskbar is transparent.
    /// </summary>
    public bool IsTaskbarTransparent
    {
        get;
    }

    /// <summary>
    /// Gets a value indicating whether the Start button is positioned on the left.
    /// </summary>
    public bool IsStartButtonLeft
    {
        get;
    }

    /// <summary>
    /// Method for initialization of the taskbar customizer service asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Method for setting Taskbar color.
    /// </summary>
    /// <param name="color">Color of taskbar.</param>
    /// <returns>Completed Task.</returns>
    Task SetTaskbarColor(Color color);

    /// <summary>
    /// Method for setting Taskbar transparent or non-transparent.
    /// </summary>
    /// <param name="transparent">Indicator for setting transparency.</param>
    /// <returns>Completed Task.</returns>
    Task SetTaskbarTransparent(bool transparent);

    /// <summary>
    /// Method for setting Windows button position.
    /// </summary>
    /// <param name="isLeft">Indicator if windows button is on left.</param>
    /// <returns>Completed Task.</returns>
    Task SetStartButtonPosition(bool isLeft);

    /// <summary>
    /// Method for settings synchronization.
    /// </summary>
    /// <param name="isSynchronizable">Indicator whether the settings of taskbar should be synchronized.</param>
    /// <returns>Completed Task.</returns>
    Task SetSynchronization(bool isSynchronizable);
}