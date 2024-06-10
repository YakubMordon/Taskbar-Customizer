// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Native;

using System.Runtime.InteropServices;

/// <summary>
/// Static class, which contains method for User32.dll with necessary methods.
/// </summary>
public static class User32Interop
{
    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_LAYERED = 0x80000;
    public const int LWA_ALPHA = 0x2;

    public const uint TransparencyColor = 0x000000;

    public enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19,
    }

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern nint FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern nint GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetLayeredWindowAttributes(nint hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern nint SendMessage(nint hWnd, int msg, int wParam, nint lParam);

    [DllImport("user32.dll")]
    public static extern int SetWindowCompositionAttribute(nint hwnd, ref WINDOWCOMPOSITIONATTRIBDATA data);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

    [StructLayout(LayoutKind.Sequential)]
    public struct ACCENT_POLICY
    {
        public int nAccentState;

        public int nFlags;

        public int nColor;

        public int nAnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWCOMPOSITIONATTRIBDATA
    {
        public WindowCompositionAttribute Attribute;

        public nint Data;

        public int SizeOfData;
    }
}
