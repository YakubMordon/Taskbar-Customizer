// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Services.Taskbar;

using System.Threading.Tasks;

using Microsoft.UI;

using Taskbar_Customizer.Core.Contracts.Services.Configuration;
using Taskbar_Customizer.Core.Contracts.Services.Taskbar;

using Taskbar_Customizer.Core.Services.Configuration;

using Taskbar_Customizer.Core.Helpers;

using Taskbar_Customizer.Helpers.Helpers.Taskbar;

using Windows.UI;

/// <summary>
/// Service responsible for customizing and managing taskbar actions.
/// </summary>
public class TaskbarCustomizerService : ITaskbarCustomizerService
{
    private const string TASKBAR_COLOR_KEY = "AppBackgroundTaskbarColor";

    private const string TASKBAR_TRANSPARENT_KEY = "AppBackgroundTaskbarTransparent";

    private const string TASKBAR_START_BUTTON_KEY = "AppBackgroundTaskbarStartButton";

    private const string TASKBAR_SYNCHRONIZATION_KEY = "AppBackgroundSynchronizable";

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

        this.IsTaskbarTransparent = await this.LoadIndicatorFromSettingsAsync(TASKBAR_TRANSPARENT_KEY);
        this.IsStartButtonLeft = await this.LoadIndicatorFromSettingsAsync(TASKBAR_START_BUTTON_KEY);

        SynchronizationService.IsSynchronizable = await this.LoadIndicatorFromSettingsAsync(TASKBAR_SYNCHRONIZATION_KEY);

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

        await this.SaveIndicatorInSettingsAsync(TASKBAR_START_BUTTON_KEY, this.IsStartButtonLeft);
    }

    /// <inheritdoc />
    public async Task SetTaskbarTransparent(bool transparent)
    {
        this.IsTaskbarTransparent = transparent;

        TransparencyHelper.SetTaskbarTransparency(this.IsTaskbarTransparent);

        await this.SaveIndicatorInSettingsAsync(TASKBAR_TRANSPARENT_KEY, this.IsTaskbarTransparent);
    }

    /// <inheritdoc />
    public async Task SetSynchronization(bool isSynchronizable)
    {
        SynchronizationService.IsSynchronizable = isSynchronizable;

        await this.SaveIndicatorInSettingsAsync(TASKBAR_SYNCHRONIZATION_KEY, SynchronizationService.IsSynchronizable);
    }

    private async Task<Color> LoadColorFromSettingsAsync()
    {
        var colorString = await this.localSettingsService.ReadSettingAsync<string>(TASKBAR_COLOR_KEY);

        if (string.IsNullOrWhiteSpace(colorString))
        {
            return Colors.Blue;
        }

        return await Json.ToObjectAsync<Color>(colorString);
    }

    private async Task SaveColorInSettingsAsync(Color color)
    {
        var stringifiedColor = await Json.StringifyAsync(color);

        await this.localSettingsService.SaveSettingAsync(TASKBAR_COLOR_KEY, stringifiedColor);
    }

    private async Task<bool> LoadIndicatorFromSettingsAsync(string key)
    {
        var indicatorString = await this.localSettingsService.ReadSettingAsync<string>(key);

        if (bool.TryParse(indicatorString, out var indicator))
        {
            return indicator;
        }

        return default;
    }

    private async Task SaveIndicatorInSettingsAsync(string key, bool indicator)
    {
        await this.localSettingsService.SaveSettingAsync(key, indicator.ToString());
    }
}