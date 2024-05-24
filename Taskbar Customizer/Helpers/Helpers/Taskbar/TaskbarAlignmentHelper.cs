// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using Microsoft.Win32;

/// <summary>
/// Static helper class for changing the position of the start button on taskbar.
/// </summary>
public static class TaskbarAlignmentHelper
{
    /// <summary>
    /// Position of Taskbar alignment.
    /// </summary>
    private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    /// <summary>
    /// Taskbar alignment item.
    /// </summary>
    private const string RegistryValueName = "TaskbarAl";

    /// <summary>
    /// Method for setting taskbar alignment.
    /// </summary>
    /// <param name="isCentered">Indicates whether taskbar alignment is center.</param>
    public static void SetTaskbarAlignment(bool isCentered)
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true);

        if (key is not null)
        {
            var value = isCentered ? 1 : 0;

            key.SetValue(RegistryValueName, value, RegistryValueKind.DWord);
        }
    }

    /// <summary>
    /// Method for getting taskbar alignment.
    /// </summary>
    /// <returns>Indicator whether taskbar alignment is center.</returns>
    /// <exception cref="NullReferenceException">Exception thrown when key is not found.</exception>
    public static bool GetTaskbarAlignment()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false);

        if (key is not null)
        {
            var value = (int?)key.GetValue(RegistryValueName);

            return value == 0;
        }

        throw new NullReferenceException();
    }
}