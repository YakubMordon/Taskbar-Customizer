// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Application;

using Taskbar_Customizer.Helpers.Helpers.Native;

/// <summary>
/// Helper class, which provides helper methods related to runtime environment information.
/// </summary>
public class RuntimeHelper
{
    /// <summary>
    /// Gets a value indicating whether the application is running as an MSIX package.
    /// </summary>
    public static bool IsMSIX
    {
        get
        {
            var length = 0;

            // Call GetCurrentPackageFullName with a null StringBuilder to get the required buffer size.
            // MSIX applications have a specific error code (15700L) when GetCurrentPackageFullName is called with a null buffer.
            // Check if the return value is different from the MSIX-specific error code to determine if the application is an MSIX package.
            return Kernel32Interop.GetCurrentPackageFullName(ref length, null) != 15700L;
        }
    }


}