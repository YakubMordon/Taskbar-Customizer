// Copyright (c) Digital Cloud Technologies. All rights reserved.

using System;

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

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
            if (isTransparent)
            {
                // Set the alpha value to 0 to make the taskbar fully transparent
                User32Interop.SetWindowLong(handle, User32Interop.GWL_EXSTYLE, User32Interop.GetWindowLong(handle, User32Interop.GWL_EXSTYLE) | User32Interop.WS_EX_LAYERED);
                User32Interop.SetLayeredWindowAttributes(handle, User32Interop.TransparencyColor, 128, User32Interop.LWA_ALPHA);
            }
            else
            {
                // Remove the layered attribute to restore the taskbar to its default appearance
                User32Interop.SetWindowLong(handle, User32Interop.GWL_EXSTYLE, User32Interop.GetWindowLong(handle, User32Interop.GWL_EXSTYLE) & ~User32Interop.WS_EX_LAYERED);
            }
        }
    }
}