// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Application;

using Windows.ApplicationModel.Background;

/// <summary>
/// Static class for registration of background tasks.
/// </summary>
public static class BackgroundTaskRegistrationHelper
{
    private const string SYNCHRONIZATIONTASKNAME = "SynchronizationBackgroundTask";

    private static bool isSynchronizationTaskRegistered = false;

    /// <summary>
    /// Method for registration of background tasks.
    /// </summary>
    public static void RegisterBackgroundTasks()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == SYNCHRONIZATIONTASKNAME)
            {
                isSynchronizationTaskRegistered = true;
                break;
            }
        }

        if (!isSynchronizationTaskRegistered)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = SYNCHRONIZATIONTASKNAME,
                TaskEntryPoint = $"BgTaskComponent.{SYNCHRONIZATIONTASKNAME}",
            };

            builder.SetTrigger(new TimeTrigger(15, false)); // Runs every 15 minutes
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            var task = builder.Register();
        }
    }
}
