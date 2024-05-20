// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using System.Media;

using Microsoft.Extensions.Hosting;

using Serilog;


/// <summary>
/// Class for synchronization of data in application.
/// </summary>
public class SynchronizationBackgroundTask : BackgroundService
{
    /// <summary>
    /// Constant for second in milliseconds.
    /// </summary>
    private const int second = 1000;

    /// <summary>
    /// Constant for seconds in minute
    /// </summary>
    private const int secondsInMinute = 60;

    /// <inheritdoc />
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            // TODO: Add synchronization code here
            
            await Task.Delay(second, stoppingToken);

            NotificationManager.ShowNotification("Background Notification Title", "Your data was synchronized");
        }
    }
}
