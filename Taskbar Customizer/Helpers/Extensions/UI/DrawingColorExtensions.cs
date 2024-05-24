// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Extensions.UI;

using Windows.UI;
using DrawingColor = System.Drawing.Color;

/// <summary>
/// Provides methods for converting colors between different formats.
/// </summary>
public static class DrawingColorExtensions
{
    /// <summary>
    /// Converts a System.Drawing.Color to a Windows.UI.Color.
    /// </summary>
    /// <param name="drawingColor">The System.Drawing.Color to convert.</param>
    /// <returns>The equivalent Windows.UI.Color.</returns>
    public static Color ToUIColor(this DrawingColor drawingColor)
    {
        return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
    }
}