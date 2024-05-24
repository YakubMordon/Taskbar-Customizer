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

    /// <summary>
    /// Method for setting the window composition attribute for the specified window.
    /// </summary>
    /// <param name="hwnd">A handle to the window.</param>
    /// <param name="data">A reference to a WINDOWCOMPOSITIONATTRIBDATA structure containing the attribute data.</param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
    /// </returns>
    [DllImport("user32.dll")]
    private static extern int SetWindowCompositionAttribute(nint hwnd, ref WINDOWCOMPOSITIONATTRIBDATA data);

    /// <summary>
    /// Method for retrieving a handle to the top-level window whose class name and window name match the specified strings.
    /// </summary>
    /// <param name="lpClassName">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function.</param>
    /// <param name="lpWindowName">The window name (the window's title).</param>
    /// <returns>
    /// If the function succeeds, the return value is a handle to the window that has the specified class name and window name. If the function fails, the return value is NULL.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// Represents an access policy that contains a set of access policy entries.
    /// </summary>
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

    /// <summary>
    /// Describes a key/value pair that specifies a window composition attribute and its value.
    /// </summary>
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