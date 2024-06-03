// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Taskbar;

using System.Threading.Tasks;

using Microsoft.UI;

using Taskbar_Customizer.Contracts.Services.Configuration;
using Taskbar_Customizer.Contracts.Services.Taskbar;

using Taskbar_Customizer.Services.Configuration;

using Taskbar_Customizer.Helpers.Helpers;
using Taskbar_Customizer.Helpers.Helpers.Taskbar;

using Windows.UI;

/// <summary>
/// Service responsible for customizing and managing taskbar actions.
/// </summary>
public class TaskbarCustomizerService : ITaskbarCustomizerService
{
    /// <summary>
    /// Taskbar Color key.
    /// </summary>
    private const string TaskbarColorKey = "AppBackgroundTaskbarColor";

    /// <summary>
    /// Taskbar Transparent key.
    /// </summary>
    private const string TaskbarTransparentKey = "AppBackgroundTaskbarTransparent";

    /// <summary>
    /// Taskbar Start Button key.
    /// </summary>
    private const string TaskbarStartButtonKey = "AppBackgroundTaskbarStartButton";

    /// <summary>
    /// Synchronization key.
    /// </summary>
    private const string TaskbarSynchronizationKey = "AppBackgroundSynchronizable";

    /// <summary>
    /// The service used to access local settings.
    /// </summary>
    private readonly ILocalSettingsService localSettingsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskbarCustomizerService"/> class.
    /// </summary>
    /// <param name="localSettingsService">The service used to access local settings.</param>
    public TaskbarCustomizerService(ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
    }

    /// <inheritdoc />
    public Color TaskbarColor
    {
        get; set;
    }

    /// <inheritdoc />
    public bool IsTaskbarTransparent
    {
        get; set;
    }

    /// <inheritdoc />
    public bool IsStartButtonLeft
    {
        get; set;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        this.TaskbarColor = await this.LoadColorFromSettingsAsync();

        this.IsTaskbarTransparent = await this.LoadIndicatorFromSettingsAsync(TaskbarTransparentKey);
        this.IsStartButtonLeft = await this.LoadIndicatorFromSettingsAsync(TaskbarStartButtonKey);

        SynchronizationService.IsSynchronizable = await this.LoadIndicatorFromSettingsAsync(TaskbarSynchronizationKey);

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task SetTaskbarColor(Color color)
    {
        this.TaskbarColor = color;

        TaskbarColorHelper.SetTaskbarColor(this.TaskbarColor);

        await this.SaveColorInSettingsAsync(this.TaskbarColor);
    }

    /// <inheritdoc />
    public async Task SetStartButtonPosition(bool isLeft)
    {
        this.IsStartButtonLeft = isLeft;

        if (OperationSystemChecker.IsWindows11OrGreater())
        {
            TaskbarAlignmentHelper.SetTaskbarAlignment(!this.IsStartButtonLeft);
        }

        await this.SaveIndicatorInSettingsAsync(TaskbarStartButtonKey, this.IsStartButtonLeft);
    }

    /// <inheritdoc />
    public async Task SetTaskbarTransparent(bool transparent)
    {
        this.IsTaskbarTransparent = transparent;

        TransparencyHelper.SetTaskbarTransparency(this.IsTaskbarTransparent);

        await this.SaveIndicatorInSettingsAsync(TaskbarTransparentKey, this.IsTaskbarTransparent);
    }

    /// <inheritdoc />
    public async Task SetSynchronization(bool isSynchronizable)
    {
        SynchronizationService.IsSynchronizable = isSynchronizable;

        await this.SaveIndicatorInSettingsAsync(TaskbarSynchronizationKey, SynchronizationService.IsSynchronizable);
    }

    /// <summary>
    /// Method for loading color from settings asynchronously.
    /// </summary>
    /// <returns>Color.</returns>
    private async Task<Color> LoadColorFromSettingsAsync()
    {
        var colorString = await this.localSettingsService.ReadSettingAsync<string>(TaskbarColorKey);

        if (string.IsNullOrWhiteSpace(colorString))
        {
            return Colors.Blue;
        }

        return await Json.ToObjectAsync<Color>(colorString);
    }

    /// <summary>
    /// Method for saving color into settings asynchronously.
    /// </summary>
    /// <param name="color">Color.</param>
    /// <returns>Completed Task.</returns>
    private async Task SaveColorInSettingsAsync(Color color)
    {
        var stringifiedColor = await Json.StringifyAsync(color);

        await this.localSettingsService.SaveSettingAsync(TaskbarColorKey, stringifiedColor);
    }

    /// <summary>
    /// Method for loading indicator (IsTaskbarTransparent or IsStartButtonLeft) from settings asynchronously.
    /// </summary>
    /// <param name="key">Key to the indicator.</param>
    /// <returns>Indicator.</returns>
    private async Task<bool> LoadIndicatorFromSettingsAsync(string key)
    {
        var indicatorString = await this.localSettingsService.ReadSettingAsync<string>(key);

        if (bool.TryParse(indicatorString, out var indicator))
        {
            return indicator;
        }

        return default;
    }

    /// <summary>
    /// Method for saving indicator (IsTaskbarTransparent or IsStartButtonLeft) into settings asynchronously.
    /// </summary>
    /// <param name="key">Key to the indicator.</param>
    /// <param name="indicator">Indicator.</param>
    /// <returns>Completed Task.</returns>
    private async Task SaveIndicatorInSettingsAsync(string key, bool indicator)
    {
        await this.localSettingsService.SaveSettingAsync(key, indicator.ToString());
    }
}