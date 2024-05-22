// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace BgTaskComponent;

using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

/// <summary>
/// Class for synchronization of data in application.
/// </summary>
public sealed class SynchronizationBackgroundTask: IBackgroundTask
{
    /// <inheritdoc />
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");

        // Perform the background task.
        SendNotification();
    }

    /// <summary>
    /// Method for sending notification.
    /// </summary>
    private static void SendNotification()
    {
        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

        var textElements = toastXml.GetElementsByTagName("text");

        textElements[0].AppendChild(toastXml.CreateTextNode("Background Notification Title"));
        textElements[1].AppendChild(toastXml.CreateTextNode("Your data was synchronized"));

        var notification = new ToastNotification(toastXml);

        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }
}
