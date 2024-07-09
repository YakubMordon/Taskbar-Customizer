// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System.Runtime.InteropServices;

using Taskbar_Customizer.Helpers.Extensions.UI;

using Taskbar_Customizer.Helpers.Helpers.Native;

using Windows.UI;

/// <summary>
/// Static helper class for changing the color of the taskbar in Windows.
/// </summary>
public static class TaskbarColorHelper
{
    /// <summary>
    /// Method for changing taskbar color.
    /// </summary>
    /// <param name="color">Color in argb format.</param>
    public static void SetTaskbarColor(Color color)
    {
        var accentPolicy = default(User32Interop.AccentPolicy);

        accentPolicy.nColor = color.ToABGR();
        accentPolicy.nAccentState = 2;
        accentPolicy.nFlags = 2;

        var data = default(User32Interop.Windowcompositionattribdata);

        data.Attribute = User32Interop.WindowCompositionAttribute.WcaAccentPolicy;
        data.SizeOfData = Marshal.SizeOf(typeof(User32Interop.AccentPolicy));
        data.Data = Marshal.AllocHGlobal(data.SizeOfData);

        Marshal.StructureToPtr(accentPolicy, data.Data, false);
        
        var taskbarHandle = User32Interop.FindWindow("Shell_TrayWnd", null);

        var secondaryTaskbarHandle = User32Interop.FindWindow("Shell_SecondaryTrayWnd", null);

        if (taskbarHandle != nint.Zero)
        {
            User32Interop.SetWindowCompositionAttribute(taskbarHandle, ref data);
        }

        if (secondaryTaskbarHandle != nint.Zero)
        {
            User32Interop.SetWindowCompositionAttribute(secondaryTaskbarHandle, ref data);
        }

        Marshal.FreeHGlobal(data.Data);
    }
}