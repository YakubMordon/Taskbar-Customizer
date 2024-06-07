// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System.Runtime.InteropServices;

using Windows.UI;

/// <summary>
/// Static helper class for changing the color of the taskbar in Windows.
/// </summary>
public static class TaskbarColorHelper
{
    /// <summary>
    /// Enumeration of window composition attributes used for setting taskbar color.
    /// </summary>
    public enum WindowCompositionAttribute
    {
        /// <summary>
        /// Attribute code for setting accent policy.
        /// </summary>
        WCA_ACCENT_POLICY = 19,
    }

    /// <summary>
    /// Method for changing taskbar color.
    /// </summary>
    /// <param name="color">Color in argb format.</param>
    public static void SetTaskbarColor(Color color)
    {
        var accentPolicy = default(ACCENT_POLICY);

        accentPolicy.nColor = color.A << 24 | color.B << 16 | color.G << 8 | color.R;
        accentPolicy.nAccentState = 2;
        accentPolicy.nFlags = 2;

        var data = default(WINDOWCOMPOSITIONATTRIBDATA);

        data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
        data.SizeOfData = Marshal.SizeOf(typeof(ACCENT_POLICY));
        data.Data = Marshal.AllocHGlobal(data.SizeOfData);

        Marshal.StructureToPtr(accentPolicy, data.Data, false);

        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != nint.Zero)
        {
            SetWindowCompositionAttribute(taskbarHandle, ref data);
        }

        Marshal.FreeHGlobal(data.Data);
    }

    [DllImport("user32.dll")]
    private static extern int SetWindowCompositionAttribute(nint hwnd, ref WINDOWCOMPOSITIONATTRIBDATA data);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint FindWindow(string lpClassName, string lpWindowName);

    [StructLayout(LayoutKind.Sequential)]
    private struct ACCENT_POLICY
    {
        /// <summary>
        /// The current state of the accent.
        /// </summary>
        public int nAccentState;

        /// <summary>
        /// Flags modifying the accent state.
        /// </summary>
        public int nFlags;

        /// <summary>
        /// The RGB color value of the accent.
        /// </summary>
        public int nColor;

        /// <summary>
        /// Animation identifier.
        /// </summary>
        public int nAnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWCOMPOSITIONATTRIBDATA
    {
        /// <summary>
        /// The attribute to modify.
        /// </summary>
        public WindowCompositionAttribute Attribute;

        /// <summary>
        /// Pointer to the data to set.
        /// </summary>
        public nint Data;

        /// <summary>
        /// Size of the data.
        /// </summary>
        public int SizeOfData;
    }
}