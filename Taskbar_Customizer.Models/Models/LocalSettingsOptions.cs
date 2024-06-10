// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Models;

/// <summary>
/// Options for configuring local settings storage.
/// </summary>
public class LocalSettingsOptions
{
    /// <summary>
    /// Gets or sets the folder path for application data storage.
    /// </summary>
    public string? ApplicationDataFolder
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the file name for local settings storage.
    /// </summary>
    public string? LocalSettingsFile
    {
        get; set;
    }
}