// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

/// <summary>
/// Class for checking operating system.
/// </summary>
public static class OperationSystemChecker
{
    /// <summary>
    /// Method for checking, if operating system is windows 11.
    /// </summary>
    /// <returns></returns>
    public static bool IsWindows11OrGreater()
    {
        var os = Environment.OSVersion;
        var version = os.Version;

        // Windows 11 version number (10.0.22000.0 or higher)
        var windows11Version = new Version(10, 0, 22000, 0);

        return version >= windows11Version;
    }
}