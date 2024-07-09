// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Native;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Static class, which contains method for User32.dll with necessary methods.
/// </summary>
public static class User32Interop
{
    /// <summary>
    /// Window style: used to retrieve or modify the window styles.
    /// </summary>
    public const int GwlStyle = -16;

    /// <summary>
    /// Extended window style: used to retrieve or modify the extended window styles.
    /// </summary>
    public const int GwlExstyle = -20;

    /// <summary>
    /// Window border style.
    /// </summary>
    public const int WsBorder = 0x00800000;

    /// <summary>
    /// Window positioning flags: frame changed.
    /// </summary>
    public const int SwpFramechanged = 0x0020;

    /// <summary>
    /// Window positioning flags: no size.
    /// </summary>
    public const int SwpNosize = 0x0001;

    /// <summary>
    /// Window positioning flags: no move.
    /// </summary>
    public const int SwpNomove = 0x0002;

    /// <summary>
    /// Window positioning flags: no Z order.
    /// </summary>
    public const int SwpNozorder = 0x0004;

    /// <summary>
    /// Window positioning flags: no activate.
    /// </summary>
    public const int SwpNoactivate = 0x0010;

    /// <summary>
    /// Enum for window composition attribute.
    /// </summary>
    public enum WindowCompositionAttribute
    {
        /// <summary>
        /// Accent policy attribute.
        /// </summary>
        WcaAccentPolicy = 19,
    }

    /// <summary>
    /// Finds a window by its class name and window name.
    /// </summary>
    /// <param name="lpClassName">The class name of the window.</param>
    /// <param name="lpWindowName">The name of the window.</param>
    /// <returns>The handle to the window if found, otherwise zero.</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern nint FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// Retrieves a handle to the active window.
    /// </summary>
    /// <returns>The handle to the active window.</returns>
    [DllImport("user32.dll")]
    public static extern nint GetActiveWindow();

    /// <summary>
    /// Retrieves information about the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>The requested value.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(nint hWnd, int nIndex);

    /// <summary>
    /// Changes the size, position, and Z order of a child, pop-up, or top-level window.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
    /// <param name="x">The new position of the left side of the window.</param>
    /// <param name="y">The new position of the top of the window.</param>
    /// <param name="cx">The new width of the window.</param>
    /// <param name="cy">The new height of the window.</param>
    /// <param name="uFlags">The window sizing and positioning flags.</param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    /// <summary>
    /// Sends the specified message to a window or windows.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The result of the message processing; it depends on the message sent.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern nint SendMessage(nint hWnd, int msg, int wParam, nint lParam);

    /// <summary>
    /// Sets the process-default DPI awareness context.
    /// </summary>
    /// <param name="dpiFlag">The DPI awareness context.</param>
    /// <returns>Returns TRUE if successful, or FALSE otherwise.</returns>
    [DllImport("user32.dll")]
    public static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

    /// <summary>
    /// Sets the window composition attribute.
    /// </summary>
    /// <param name="hwnd">A handle to the window.</param>
    /// <param name="data">A pointer to a <see cref="Windowcompositionattribdata"/> structure that specifies the new value for the attribute.</param>
    /// <returns>Returns S_OK if successful, or an HRESULT error code otherwise.</returns>
    [DllImport("user32.dll")]
    public static extern int SetWindowCompositionAttribute(nint hwnd, ref Windowcompositionattribdata data);

    /// <summary>
    /// Changes an attribute of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>The previous value of the specified offset.</returns>
    [DllImport("user32.dll")]
    public static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

    /// <summary>
    /// Contains information about the accent policy for window composition.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        /// <summary>
        /// The accent state.
        /// </summary>
        public int nAccentState;

        /// <summary>
        /// Flags for the accent policy.
        /// </summary>
        public int nFlags;

        /// <summary>
        /// The color value.
        /// </summary>
        public int nColor;

        /// <summary>
        /// The animation ID.
        /// </summary>
        public int nAnimationId;
    }

    /// <summary>
    /// Contains information about the window composition attribute data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Windowcompositionattribdata
    {
        /// <summary>
        /// The window composition attribute.
        /// </summary>
        public WindowCompositionAttribute Attribute;

        /// <summary>
        /// A pointer to the data.
        /// </summary>
        public nint Data;

        /// <summary>
        /// The size of the data.
        /// </summary>
        public int SizeOfData;
    }
}
