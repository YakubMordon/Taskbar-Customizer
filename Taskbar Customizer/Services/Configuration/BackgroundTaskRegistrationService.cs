// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Configuration;

using Windows.ApplicationModel.Background;

/// <summary>
/// Static class for registration of background tasks.
/// </summary>
public static class BackgroundTaskRegistrationService
{
    /// <summary>
    /// Synchronization background task name.
    /// </summary>
    private const string SynchronizationTaskName = "SynchronizationBackgroundTask";

    /// <summary>
    /// Indicates whether synchronization background task is registered.
    /// </summary>
    private static bool isSynchronizationTaskRegistered = false;

    /// <summary>
    /// Method for registration of background tasks.
    /// </summary>
    public static void RegisterBackgroundTasks()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == SynchronizationTaskName)
            {
                isSynchronizationTaskRegistered = true;
                break;
            }
        }

        if (!isSynchronizationTaskRegistered)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = SynchronizationTaskName,
                TaskEntryPoint = $"BgTaskComponent.{SynchronizationTaskName}",
            };

            builder.SetTrigger(new TimeTrigger(15, false)); // Виконується кожні 15 хвилин

            var task = builder.Register();
        }
    }
}
