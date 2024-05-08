// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers;

using System.Runtime.InteropServices;

/// <summary>
/// Static helper class for working with transparency.
/// </summary>
public static class TransparencyHelper
{
    // Constants for window styles
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int LWA_ALPHA = 0x2;

    private static readonly uint TransparencyColor = 0x000000;

    /// <summary>
    /// Method to make taskbar transparent.
    /// </summary>
    /// <param name="isTransparent">Indicator of transparency.</param>
    public static void SetTaskbarTransparency(bool isTransparent)
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != nint.Zero)
        {
            if (isTransparent)
            {
                SetWindowLong(taskbarHandle, GWL_EXSTYLE, GetWindowLong(taskbarHandle, GWL_EXSTYLE) | WS_EX_LAYERED);
                SetLayeredWindowAttributes(taskbarHandle, TransparencyColor, 128, LWA_ALPHA);
            }
            else
            {
                SetWindowLong(taskbarHandle, GWL_EXSTYLE, GetWindowLong(taskbarHandle, GWL_EXSTYLE) & ~WS_EX_LAYERED);
            }
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetLayeredWindowAttributes(nint hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);
}