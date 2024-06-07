// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace BgTaskComponent;

using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;

using Windows.Foundation.Collections;

using Windows.UI.Notifications;

/// <summary>
/// Class for synchronization of data in application.
/// </summary>
public sealed class SynchronizationBackgroundTask : IBackgroundTask
{
    private BackgroundTaskDeferral deferral;

    private AppServiceConnection connection;

    private static ValueSet synchronizedData = new ();

    /// <inheritdoc />
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        deferral = taskInstance.GetDeferral();
        taskInstance.Canceled += OnTaskCanceled;

        var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
        connection = details.AppServiceConnection;
        connection.RequestReceived += OnRequestReceived;
    }

    private void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
        var messageDeferral = args.GetDeferral();
        var input = args.Request.Message;

        foreach (var key in input.Keys)
        {
            synchronizedData[key] = input[key];
        }

        args.Request.SendResponseAsync(synchronizedData).Completed = (info, status) =>
        {
            messageDeferral.Complete();
        };

        SendNotification();
    }

    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
        connection.Dispose();
        deferral.Complete();
    }

    private static void SendNotification()
    {
        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

        var textElements = toastXml.GetElementsByTagName("text");

        textElements[0].AppendChild(toastXml.CreateTextNode("Background Notification"));
        textElements[1].AppendChild(toastXml.CreateTextNode("Your data was synchronized"));

        var notification = new ToastNotification(toastXml);

        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }
}
