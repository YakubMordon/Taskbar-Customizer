// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Extensions;

using Microsoft.UI.Xaml.Controls;

/// <summary>
/// Class with extension methods for working with a Frame control in a Windows Presentation Foundation (WPF) application.
/// </summary>
public static class FrameExtensions
{
    /// <summary>
    /// Method for getting the view model associated with the content of the specified Frame control.
    /// </summary>
    /// <param name="frame">The Frame control whose content's view model is retrieved.</param>
    /// <returns>
    /// The view model object associated with the content of the Frame control, or null if the Frame, its content, or the view model property is not accessible.
    /// </returns>
    public static object? GetPageViewModel(this Frame frame)
    {
        // Ensure that the Frame and its Content are not null, and attempt to retrieve the view model property named "ViewModel" from the content.
        return frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
    }
}
