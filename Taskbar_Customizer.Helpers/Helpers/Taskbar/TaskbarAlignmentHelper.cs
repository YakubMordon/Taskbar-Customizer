﻿// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System;

using Microsoft.Win32;

/// <summary>
/// Static helper class for changing the position of the start button on taskbar.
/// </summary>
public static class TaskbarAlignmentHelper
{
    private const string REGISTRY_KEY_PATH = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string REGISTRY_VALUE_NAME = "TaskbarAl";

    /// <summary>
    /// Method for setting taskbar alignment.
    /// </summary>
    /// <param name="isCentered">Indicates whether taskbar alignment is center.</param>
    public static void SetTaskbarAlignment(bool isCentered)
    {
        using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_PATH, true);

        if (key is not null)
        {
            var value = isCentered ? 1 : 0;

            key.SetValue(REGISTRY_VALUE_NAME, value, RegistryValueKind.DWord);
        }
    }
}