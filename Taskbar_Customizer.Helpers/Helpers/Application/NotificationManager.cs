// Copyright (c) Digital Cloud Technologies. All rights reserved.

using System;

namespace Taskbar_Customizer.Helpers.Helpers.Application;

using CommunityToolkit.WinUI.Notifications;

using Microsoft.Windows.AppNotifications;

/// <summary>
/// Static class for managing sending and handling notifications in a WinUI 3 application.
/// </summary>
public static class NotificationManager
{
    /// <summary>
    /// Method for displaying a notification with the specified title and content.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="content">The content of the notification.</param>
    public static void ShowNotification(string title, string content)
    {
        var toastContent = new ToastContentBuilder()
            .AddText(title)
            .AddText(content)
            .GetToastContent();

        var xmlDoc = toastContent.GetXml();
        var xmlString = xmlDoc.GetXml();

        var toastNotification = new AppNotification(xmlString);

        AppNotificationManager.Default.Show(toastNotification);
    }

    /// <summary>
    /// Method for initialization of the notification manager and registers for notification events.
    /// </summary>
    public static void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
        AppNotificationManager.Default.Register();
    }

    /// <summary>
    /// Notification activation event handler.
    /// </summary>
    private static void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        // Handle notification activation here
    }
}
