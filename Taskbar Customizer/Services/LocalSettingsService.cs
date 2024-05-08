// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services;

using Microsoft.Extensions.Options;

using Taskbar_Customizer.Contracts.Services;
using Taskbar_Customizer.Models;

using Taskbar_Customizer.Helpers.Contracts.Services;

using Windows.Storage;
using Taskbar_Customizer.Helpers.Helpers;

/// <summary>
/// Service for reading and saving local settings asynchronously.
/// </summary>
public class LocalSettingsService : ILocalSettingsService
{
    /// <summary>
    /// The default folder path where application data is stored.
    /// </summary>
    private const string DefaultApplicationDataFolder = "Taskbar Customizer/ApplicationData";

    /// <summary>
    /// The default filename for local settings storage.
    /// </summary>
    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    /// <summary>
    /// The file service used for file operations.
    /// </summary>
    private readonly IFileService fileService;

    /// <summary>
    /// The options for local settings management.
    /// </summary>
    private readonly LocalSettingsOptions options;

    /// <summary>
    /// The path to the local application data folder.
    /// </summary>
    private readonly string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    /// <summary>
    /// The full path to the application data folder.
    /// </summary>
    private readonly string applicationDataFolder;

    /// <summary>
    /// The full path to the local settings file.
    /// </summary>
    private readonly string localsettingsFile;

    /// <summary>
    /// The dictionary to store local settings key-value pairs.
    /// </summary>
    private IDictionary<string, object> settings;

    /// <summary>
    /// Indicates whether the local settings service has been initialized.
    /// </summary>
    private bool isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalSettingsService"/> class.
    /// </summary>
    /// <param name="fileService">The file service to use for file operations.</param>
    /// <param name="options">The local settings options.</param>
    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        this.fileService = fileService;
        this.options = options.Value;

        this.applicationDataFolder = Path.Combine(this.localApplicationData, this.options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
        this.localsettingsFile = this.options.LocalSettingsFile ?? DefaultLocalSettingsFile;

        this.settings = new Dictionary<string, object>();
    }

    /// <inheritdoc />
    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMSIX)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }
        else
        {
            await this.InitializeAsync();

            if (this.settings is not null && this.settings.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    /// <inheritdoc />
    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMSIX)
        {
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
        }
        else
        {
            await this.InitializeAsync();

            this.settings[key] = await Json.StringifyAsync(value);

            await Task.Run(() => this.fileService.Save(this.applicationDataFolder, this.localsettingsFile, this.settings));
        }
    }

    /// <summary>
    /// Method, which initializes the local settings service by reading settings from a file if not already initialized.
    /// </summary>
    private async Task InitializeAsync()
    {
        if (!this.isInitialized)
        {
            this.settings = await Task.Run(() => this.fileService.Read<IDictionary<string, object>>(this.applicationDataFolder, this.localsettingsFile)) ?? new Dictionary<string, object>();

            this.isInitialized = true;
        }
    }
}
