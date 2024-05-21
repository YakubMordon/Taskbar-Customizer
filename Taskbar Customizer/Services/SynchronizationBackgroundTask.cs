// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using System.Diagnostics;

using Windows.ApplicationModel.Background;


/// <summary>
/// Class for synchronization of data in application.
/// </summary>
public sealed class SynchronizationBackgroundTask : IBackgroundTask
{
    /// <inheritdoc />
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        Debug.WriteLine("Hello");

        Console.WriteLine("Hello");
    }
}
