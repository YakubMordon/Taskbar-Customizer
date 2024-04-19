// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services;

/// <summary>
/// Contract for reading and saving local settings asynchronously.
/// </summary>
public interface ILocalSettingsService
{
    /// <summary>
    /// Method for asynchronous reading a setting from local storage based on the provided key.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="key">The unique key used to identify the setting.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains
    /// the value of the setting if it exists; otherwise, returns <c>null</c>.
    /// </returns>
    Task<T?> ReadSettingAsync<T>(string key);

    /// <summary>
    /// Method for asynchronous saving a setting to local storage using the specified key and value.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="key">The unique key used to identify the setting.</param>
    /// <param name="value">The value of the setting to be saved.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveSettingAsync<T>(string key, T value);
}
