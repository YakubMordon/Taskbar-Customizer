// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Core.Services.Configuration;

using Windows.ApplicationModel.Background;

/// <summary>
/// Static class for registration of background tasks.
/// </summary>
public static class BackgroundTaskRegistrationService
{
    private const string SYNCHRONIZATION_TASK_NAME = "SynchronizationBackgroundTask";

    private static bool isSynchronizationTaskRegistered = false;

    /// <summary>
    /// Method for registration of background tasks.
    /// </summary>
    public static void RegisterBackgroundTasks()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == SYNCHRONIZATION_TASK_NAME)
            {
                isSynchronizationTaskRegistered = true;
                break;
            }
        }

        if (!isSynchronizationTaskRegistered)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = SYNCHRONIZATION_TASK_NAME,
                TaskEntryPoint = $"BgTaskComponent.{SYNCHRONIZATION_TASK_NAME}",
            };

            builder.SetTrigger(new TimeTrigger(15, false)); // Runs every 15 minutes
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            var task = builder.Register();
        }
    }
}
