// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System;

using Microsoft.Win32;

/// <summary>
/// Static helper class for changing the position of the start button on taskbar.
/// </summary>
public static class TaskbarAlignmentHelper
{
    private const string REGISTRYKEYPATH = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string REGISTRYVALUENAME = "TaskbarAl";

    /// <summary>
    /// Method for setting taskbar alignment.
    /// </summary>
    /// <param name="isCentered">Indicates whether taskbar alignment is center.</param>
    public static void SetTaskbarAlignment(bool isCentered)
    {
        using var key = Registry.CurrentUser.OpenSubKey(REGISTRYKEYPATH, true);

        if (key is not null)
        {
            var value = isCentered ? 1 : 0;

            key.SetValue(REGISTRYVALUENAME, value, RegistryValueKind.DWord);
        }
    }
}