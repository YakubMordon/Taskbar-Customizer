﻿// Copyright (c) Digital Cloud Technologies. All rights reserved.

using System.Runtime.InteropServices;

namespace Taskbar_Customizer.Services;

public class TaskbarPositionHelper
{
    private const int ABM_SETPOS = 0x00000003;
    private const int ABE_LEFT = 0;
    private const int ABE_CENTER = 1;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("shell32.dll")]
    private static extern IntPtr SHAppBarMessage(int msg, ref APPBARDATA data);

    [StructLayout(LayoutKind.Sequential)]
    private struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public int uCallbackMessage;
        public int uEdge;
        public RECT rc;
        public IntPtr lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static void SetStartButtonPosition(bool isCentered)
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != IntPtr.Zero)
        {
            var appBarData = new APPBARDATA
            {
                cbSize = Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = taskbarHandle,
                uEdge = isCentered ? ABE_CENTER : ABE_LEFT
            };

            SHAppBarMessage(ABM_SETPOS, ref appBarData);
        }
    }
}