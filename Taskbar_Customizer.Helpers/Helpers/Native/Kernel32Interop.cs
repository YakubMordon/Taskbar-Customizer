// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Native;

using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Static class, which contains methods for Kernel32.dll with necessary components.
/// </summary>
public static class Kernel32Interop
{
    /// <summary>
    /// Retrieves the full name of the package that is currently running.
    /// </summary>
    /// <param name="packageFullNameLength">The length of the package full name buffer. 
    /// On input, it specifies the size of the buffer. 
    /// On output, it receives the size of the full name, in characters, including the null terminator.</param>
    /// <param name="packageFullName">A StringBuilder that, on successful return, contains the full name of the package. 
    /// If the function fails, this parameter may be null.</param>
    /// <returns>If the function succeeds, it returns 0 (ERROR_SUCCESS). 
    /// If the function fails, it returns an error code. To get the full error code, use Marshal.GetLastWin32Error.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);
}
