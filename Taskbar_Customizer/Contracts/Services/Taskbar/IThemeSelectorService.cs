// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services.Taskbar;

using Microsoft.UI.Xaml;

/// <summary>
/// Contract for selecting and managing application themes.
/// </summary>
public interface IThemeSelectorService
{
    /// <summary>
    /// Gets the current theme of the application.
    /// </summary>
    ElementTheme Theme
    {
        get;
    }

    /// <summary>
    /// Method for initialization of the theme selector service asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Method for setting the application theme asynchronously.
    /// </summary>
    /// <param name="theme">The new theme to set.</param>
    /// <returns>A task representing the asynchronous operation of setting the theme.</returns>
    Task SetThemeAsync(ElementTheme theme);

    /// <summary>
    /// Method for setting the requested theme for the application asynchronously based on the current system theme.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of setting the requested theme.</returns>
    Task SetRequestedThemeAsync();
}
