// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using System.Runtime.InteropServices;
using Windows.UI;

/// <summary>
/// Static helper class for changing the color of the taskbar in Windows.
/// </summary>
public static class TaskbarColorHelper
{
    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWCOMPOSITIONATTRIBDATA
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    public enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    // Define the ACCENT_POLICY struct
    [StructLayout(LayoutKind.Sequential)]
    private struct ACCENT_POLICY
    {
        public int nAccentState;
        public int nFlags;
        public int nColor;
        public int nAnimationId;
    }

    /// <summary>
    /// Method for changing taskbar color.
    /// </summary>
    /// <param name="color">Color in argb format.</param>
    public static void SetTaskbarColor(Color color)
    {
        var accentPolicy = new ACCENT_POLICY();

        accentPolicy.nColor = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        accentPolicy.nAccentState = 2;
        accentPolicy.nFlags = 2; 

        var data = new WINDOWCOMPOSITIONATTRIBDATA();
        data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
        data.SizeOfData = Marshal.SizeOf(typeof(ACCENT_POLICY));
        data.Data = Marshal.AllocHGlobal(data.SizeOfData);

        Marshal.StructureToPtr(accentPolicy, data.Data, false);

        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != IntPtr.Zero)
        {
            SetWindowCompositionAttribute(taskbarHandle, ref data);
        }

        Marshal.FreeHGlobal(data.Data);
    }

    [DllImport("user32.dll")]
    private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WINDOWCOMPOSITIONATTRIBDATA data);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
}