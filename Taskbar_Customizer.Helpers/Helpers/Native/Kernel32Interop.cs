// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Native;

using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Static class, which contains methods for Kernel32.dll with necessary components.
/// </summary>
public static class Kernel32Interop
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);
}
