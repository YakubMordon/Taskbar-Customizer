// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System;

using Taskbar_Customizer.Helpers.Helpers.Native;

/// <summary>
/// Static helper class for working with transparency.
/// </summary>
public static class TransparencyHelper
{
    /// <summary>
    /// Method to make taskbar transparent.
    /// </summary>
    /// <param name="isTransparent">Indicator of transparency.</param>
    public static void SetTaskbarTransparency(bool isTransparent)
    {
        var taskbarHandle = User32Interop.FindWindow("Shell_TrayWnd", null);

        var secondaryTaskbarHandle = User32Interop.FindWindow("Shell_SecondaryTrayWnd", null);

        SetTransparency(taskbarHandle, isTransparent);

        SetTransparency(secondaryTaskbarHandle, isTransparent);
    }

    private static void SetTransparency(IntPtr handle, bool isTransparent)
    {
        if (handle != nint.Zero)
        {
            var style = User32Interop.GetWindowLong(handle, User32Interop.GWL_STYLE);
            var exStyle = User32Interop.GetWindowLong(handle, User32Interop.GWL_EXSTYLE);

            if (isTransparent)
            {
                // Restore border style
                style |= User32Interop.WS_BORDER;
            }
            else
            {
                // Remove border style
                style &= ~User32Interop.WS_BORDER;
            }

            User32Interop.SetWindowLong(handle, User32Interop.GWL_STYLE, style);
            User32Interop.SetWindowLong(handle, User32Interop.GWL_EXSTYLE, exStyle);
            User32Interop.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, User32Interop.SWP_NOMOVE | User32Interop.SWP_NOSIZE | User32Interop.SWP_NOZORDER | User32Interop.SWP_NOACTIVATE | User32Interop.SWP_FRAMECHANGED);
        }
    }
}