// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using System.Runtime.InteropServices;

/// <summary>
/// Static helper class for working with transparency.
/// </summary>
public static class TransparencyHelper
{
    // Constants for window styles
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const int LWA_ALPHA = 0x2;

    private static readonly uint transparencyColor = 0x000000;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    /// <summary>
    /// Method to make taskbar transparent.
    /// </summary>
    /// <param name="isTransparent">Indicator of transparency.</param>
    public static void SetTaskbarTransparency(bool isTransparent)
    {
        IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != IntPtr.Zero)
        {
            if (isTransparent)
            {
                SetWindowLong(taskbarHandle, GWL_EXSTYLE, GetWindowLong(taskbarHandle, GWL_EXSTYLE) | WS_EX_LAYERED);
                SetLayeredWindowAttributes(taskbarHandle, transparencyColor, 128, LWA_ALPHA);
            }
            else
            {
                SetWindowLong(taskbarHandle, GWL_EXSTYLE, GetWindowLong(taskbarHandle, GWL_EXSTYLE) & ~WS_EX_LAYERED);
            }
        }
    }

    /// <summary>
    /// Method to get if taskbar is transparent (Currently not in use).
    /// </summary>
    /// <returns>True if taskbar is transparent, False if not.</returns>
    public static bool GetTaskbarTransparency()
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != IntPtr.Zero)
        {
            var currentStyle = GetWindowLong(taskbarHandle, GWL_EXSTYLE);
            return (currentStyle & WS_EX_LAYERED) != 0 && (currentStyle & WS_EX_TRANSPARENT) != 0;
        }

        return false;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}