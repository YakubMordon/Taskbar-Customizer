// Copyright (c) Digital Cloud Technologies. All rights reserved.

using Newtonsoft.Json;
using Taskbar_Customizer.Contracts.Services.Taskbar;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.System.RemoteSystems;
using Windows.UI;

namespace Taskbar_Customizer.Services.Configuration;

/// <summary>
/// Static class for handling actions with synchronization.
/// </summary>
public class SynchronizationService
{
    /// <summary>
    /// Service for customizing taskbar.
    /// </summary>
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
    public void CallSyncService(string? key, string? value)
    {
        var remoteSystemWatcher = RemoteSystem.CreateWatcher();
        remoteSystemWatcher.RemoteSystemAdded += async (sender, args) =>
        {
            var connectionRequest = new RemoteSystemConnectionRequest(args.RemoteSystem);
            var connection = new AppServiceConnection
            {
                AppServiceName = "com.TaskbarCustomizer.SyncService",
                PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName
            };

            var status = await connection.OpenRemoteAsync(connectionRequest);

            if (status == AppServiceConnectionStatus.Success)
            {
                var message = new ValueSet
                {
                    { key, value }
                };

                var response = await connection.SendMessageAsync(message);

                if (response.Status == AppServiceResponseStatus.Success)
                {
                    var result = response.Message;

                    if (GetBooleanValue("Transparency", result, out var isTransparent))
                    {
                        await taskbarCustomizerService.SetTaskbarTransparent(isTransparent);
                    }

                    if (GetBooleanValue("Alignment", result, out var isAlignedOnCenter))
                    {
                        await taskbarCustomizerService.SetStartButtonPosition(!isAlignedOnCenter);
                    }

                    if (GetValue<Color?>("Color", result) is Color color)
                    {
                        await taskbarCustomizerService.SetTaskbarColor(color);
                    }
                }
            }
        };

        remoteSystemWatcher.Start();
    }

    /// <summary>
    /// Retrieves a value of type <typeparamref name="T"/> from the <paramref name="result"/> based on the specified <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="result">The collection containing the values.</param>
    /// <returns>The retrieved value if the key is found; otherwise, the default value of type <typeparamref name="T"/>.</returns>
    private T GetValue<T>(string key, ValueSet result)
    {
        if (result.TryGetValue(key, out var value) && value is string stringValue)
        {
            return JsonConvert.DeserializeObject<T>(stringValue);
        }
        return default;
    }

    /// <summary>
    /// Retrieves a boolean value from the <paramref name="result"/> based on the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the boolean value to retrieve.</param>
    /// <param name="result">The collection containing the values.</param>
    /// <param name="booleanValue">When this method returns, contains the boolean value associated with the specified key, if the key is found; otherwise, <c>false</c>.</param>
    /// <returns><c>true</c> if the boolean value was successfully retrieved; otherwise, <c>false</c>.</returns>
    private bool GetBooleanValue(string key, ValueSet result, out bool booleanValue)
    {
        booleanValue = false;

        if (result.TryGetValue(key, out var value) && value is string stringValue)
        {
            return bool.TryParse(stringValue, out booleanValue);
        }

        return false;
    }
}
