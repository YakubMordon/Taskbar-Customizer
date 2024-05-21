// Copyright (c) Digital Cloud Technologies. All rights reserved.

using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace Taskbar_Customizer.Services;

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
    private static bool IsSynchronizationTaskRegistered = false;

    /// <summary>
    /// Method for registration of background tasks.
    /// </summary>
    public static void RegisterBackgroundTasks()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == SynchronizationTaskName)
            {
                task.Value.Unregister(true);
                break;
            }
        }

        if (!IsSynchronizationTaskRegistered)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = SynchronizationTaskName,
                TaskEntryPoint = $"Taskbar_Customizer.Services.Tasks.{SynchronizationTaskName}"
            };

            builder.SetTrigger(new SystemTrigger(SystemTriggerType.BackgroundWorkCostChange, false)); // Виконується кожні 15 хвилин

            var task = builder.Register();

            task.Completed += new BackgroundTaskCompletedEventHandler(OnSynchronizationBackgroundTaskCompleted);
        }
    }

    private static void OnSynchronizationBackgroundTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
    {
        try
        {
            args.CheckResult();

            NotificationManager.ShowNotification("Background Notification Title", "Your data was synchronized");
            Debug.WriteLine("Background task completed successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Background task completion error: {ex}");
        }
    }
}
