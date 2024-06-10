// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Configuration;

using Newtonsoft.Json;

using Taskbar_Customizer.Core.Contracts.Services.Taskbar;

using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.System.RemoteSystems;
using Windows.UI;

/// <summary>
/// Static class for handling actions with synchronization.
/// </summary>
public class SynchronizationService
{
    /// <summary>
    /// Indicates whether service should synchronize data.
    /// </summary>
    public static bool IsSynchronizable = true;

    private readonly ITaskbarCustomizerService taskbarCustomizerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SynchronizationService"/> class.
    /// </summary>
    /// <param name="taskbarCustomizerService">Service for customizing taskbar.</param>
    public SynchronizationService(ITaskbarCustomizerService taskbarCustomizerService)
    {
        this.taskbarCustomizerService = taskbarCustomizerService;
    }

    /// <summary>
    /// Method for calling async service synchronization.
    /// </summary>
    /// <param name="key">Key for synchronization.</param>
    /// <param name="value">Value for synchronization.</param>
    public async void CallSyncService(string? key, string? value)
    {
        if (IsSynchronizable)
        {
            var message = new ValueSet();

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                message.Add(key, value);
            }

            await this.UpdateAppService(message);

            var remoteSystemWatcher = RemoteSystem.CreateWatcher();

            remoteSystemWatcher.RemoteSystemAdded += async (sender, args) =>
            {
                await this.UpdateRemoteSystem(args.RemoteSystem, message);
            };

            remoteSystemWatcher.Start();
        }
    }

    private static T? GetValue<T>(string key, ValueSet result)
    {
        if (result.TryGetValue(key, out var value) && value is string stringValue)
        {
            return JsonConvert.DeserializeObject<T>(stringValue);
        }

        return default;
    }

    private static bool GetBooleanValue(string key, ValueSet result, out bool booleanValue)
    {
        booleanValue = false;

        if (result.TryGetValue(key, out var value) && value is string stringValue)
        {
            return bool.TryParse(stringValue, out booleanValue);
        }

        return false;
    }

    private async Task UpdateAppService(ValueSet message)
    {
        var connection = new AppServiceConnection
        {
            AppServiceName = "com.TaskbarCustomizer.SyncService",
            PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName,
        };

        var status = await connection.OpenAsync();

        await this.SendDataToService(connection, status, message);
    }

    private async Task UpdateRemoteSystem(RemoteSystem remoteSystem, ValueSet message)
    {
        var connectionRequest = new RemoteSystemConnectionRequest(remoteSystem);
        var connection = new AppServiceConnection
        {
            AppServiceName = "com.TaskbarCustomizer.SyncService",
            PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName,
        };

        var status = await connection.OpenRemoteAsync(connectionRequest);

        await this.SendDataToService(connection, status, message);
    }

    private async Task SendDataToService(AppServiceConnection connection, AppServiceConnectionStatus status, ValueSet message)
    {
        if (status == AppServiceConnectionStatus.Success)
        {
            var response = await connection.SendMessageAsync(message);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                var result = response.Message;

                if (GetBooleanValue("Transparency", result, out var isTransparent))
                {
                    await this.taskbarCustomizerService.SetTaskbarTransparent(isTransparent);
                }

                if (GetBooleanValue("Alignment", result, out var isAlignedOnCenter))
                {
                    await this.taskbarCustomizerService.SetStartButtonPosition(!isAlignedOnCenter);
                }

                if (GetValue<Color?>("Color", result) is Color color)
                {
                    await this.taskbarCustomizerService.SetTaskbarColor(color);
                }
            }
        }
    }
}