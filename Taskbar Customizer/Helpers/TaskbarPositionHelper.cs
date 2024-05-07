// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using System.Runtime.InteropServices;

/// <summary>
/// Static helper class for changing the position of the start button on taskbar.
/// </summary>
public static class TaskbarPositionHelper
{
    private const int ABM_SETPOS = 0x00000003;
    private const int ABE_LEFT = 0;
    private const int ABE_CENTER = 1;

    /// <summary>
    /// Method for setting start button position on taskbar.
    /// </summary>
    /// <param name="isCentered">Indicates whether start button should be in center.</param>
    public static void SetStartButtonPosition(bool isCentered)
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != nint.Zero)
        {
            var appBarData = new APPBARDATA
            {
                cbSize = Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = taskbarHandle,
                uEdge = isCentered ? ABE_CENTER : ABE_LEFT,
            };

            SHAppBarMessage(ABM_SETPOS, ref appBarData);
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern nint FindWindow(string strClassName, string strWindowName);

    [DllImport("shell32.dll")]
    private static extern nint SHAppBarMessage(int msg, ref APPBARDATA data);

    [StructLayout(LayoutKind.Sequential)]
    private struct APPBARDATA
    {
        public int cbSize;
        public nint hWnd;
        public int uCallbackMessage;
        public int uEdge;
        public RECT rc;
        public nint lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}