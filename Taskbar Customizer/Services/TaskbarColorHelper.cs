// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.UI;

/// <summary>
/// Static helper class for changing the color of the taskbar in Windows.
/// </summary>
public static class TaskbarColorHelper
{
    private const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320;
    private const int WM_DWMCOLORIZATIONPARAMCHANGED = 0x0321;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    public static void SetTaskbarColor(Color color)
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle == IntPtr.Zero)
        {
            Debug.WriteLine("Taskbar handle not found.");
            return;
        }

        var rgb = ((uint)color.B << 16) | ((uint)color.G << 8) | (uint)color.R;
        var colorPtr = (IntPtr)rgb;

        SendMessage(taskbarHandle, WM_DWMCOLORIZATIONCOLORCHANGED, IntPtr.Zero, colorPtr);
        SendMessage(taskbarHandle, WM_DWMCOLORIZATIONPARAMCHANGED, IntPtr.Zero, IntPtr.Zero);
    }
}