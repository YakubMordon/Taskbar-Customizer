// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Extensions.UI;

using Windows.UI;

/// <summary>
/// Class with extension methods for <see cref="Color"/>.
/// </summary>
public static class WindowsColorExtensions
{
    /// <summary>
    /// Method for checking if color is transparent.
    /// </summary>
    /// <param name="color">Color.</param>
    /// <returns>True if color is at least partially transparent. False if not transparent.</returns>
    public static bool Transparent(this Color color) => color.A != 255;

    /// <summary>
    /// Method for converting color to ABGR format.
    /// </summary>
    /// <param name="color">Color.</param>
    /// <returns><c>int</c> representation of ABGR format.</returns>
    public static int ToABGR(this Color color) => color.A << 24 | color.B << 16 | color.G << 8 | color.R;
}
