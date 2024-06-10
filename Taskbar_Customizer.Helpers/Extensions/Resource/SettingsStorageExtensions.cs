// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Extensions.Resource;

using System;
using System.IO;
using System.Threading.Tasks;

using Taskbar_Customizer.Core.Helpers;

using Windows.Storage;
using Windows.Storage.Streams;

/// <summary>
/// Class, which contains extension methods for storing and retrieving local and roaming app data.
/// </summary>
public static class SettingsStorageExtensions
{
    private const string FileExtension = ".json";

    /// <summary>
    /// Method, which checks if roaming storage is available based on the roaming storage quota.
    /// </summary>
    /// <param name="appData">The ApplicationData instance.</param>
    /// <returns>True if roaming storage is available; otherwise, false.</returns>
    public static bool IsRoamingStorageAvailable(this ApplicationData appData)
    {
        return appData.RoamingStorageQuota > 0;
    }

    /// <summary>
    /// Method, for saving content to a file asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of content to save.</typeparam>
    /// <param name="folder">The StorageFolder where the file will be saved.</param>
    /// <param name="name">The name of the file to save.</param>
    /// <param name="content">The content to save.</param>
    /// <returns>A Task representing the asynchronous save operation.</returns>
    public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
    {
        var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);

        var fileContent = await Json.StringifyAsync(content);

        await FileIO.WriteTextAsync(file, fileContent);
    }

    /// <summary>
    /// Method for reading content from a file asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of content to read.</typeparam>
    /// <param name="folder">The StorageFolder where the file is located.</param>
    /// <param name="name">The name of the file to read.</param>
    /// <returns>The deserialized content read from the file.</returns>
    public static async Task<T?> ReadAsync<T>(this StorageFolder folder, string name)
    {
        if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
        {
            return default;
        }

        var file = await folder.GetFileAsync($"{name}.json");
        var fileContent = await FileIO.ReadTextAsync(file);

        return await Json.ToObjectAsync<T>(fileContent);
    }

    /// <summary>
    /// Method for saving value to application settings asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of value to save.</typeparam>
    /// <param name="settings">The ApplicationDataContainer where settings will be saved.</param>
    /// <param name="key">The key under which the value will be saved.</param>
    /// <param name="value">The value to save.</param>
    /// <returns>A Task representing the asynchronous save operation.</returns>
    public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
    {
        settings.SaveString(key, await Json.StringifyAsync(value));
    }

    /// <summary>
    /// Method for saving a string value to application settings asynchronously.
    /// </summary>
    /// <param name="settings">The ApplicationDataContainer where settings will be saved.</param>
    /// <param name="key">The key under which the value will be saved.</param>
    /// <param name="value">The string value to save.</param>
    public static void SaveString(this ApplicationDataContainer settings, string key, string value)
    {
        settings.Values[key] = value;
    }

    /// <summary>
    /// Method for reading value from application settings asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of value to read.</typeparam>
    /// <param name="settings">The ApplicationDataContainer from which to read settings.</param>
    /// <param name="key">The key of the value to read.</param>
    /// <returns>The deserialized value read from the settings.</returns>
    public static async Task<T?> ReadAsync<T>(this ApplicationDataContainer settings, string key)
    {
        object? obj;

        if (settings.Values.TryGetValue(key, out obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    /// <summary>
    /// Method for saving the specified byte array content to a file within the specified StorageFolder asynchronously.
    /// </summary>
    /// <param name="folder">The StorageFolder where the file will be saved.</param>
    /// <param name="content">The byte array content to write to the file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    /// <param name="options">The collision option to use when creating the file (default is ReplaceExisting).</param>
    /// <returns>A Task representing the asynchronous operation. The StorageFile that was saved.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="content"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="fileName"/> is null or empty.</exception>
    public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
    {
        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name is null or empty. Specify a valid file name", nameof(fileName));
        }

        var storageFile = await folder.CreateFileAsync(fileName, options);
        await FileIO.WriteBytesAsync(storageFile, content);
        return storageFile;
    }

    /// <summary>
    /// Method for reading the content of the specified file asynchronously as a byte array from the given StorageFolder.
    /// </summary>
    /// <param name="folder">The StorageFolder where the file is located.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>A Task representing the asynchronous operation. The byte array content of the file, or null if the file does not exist or is not accessible.</returns>
    public static async Task<byte[]?> ReadFileAsync(this StorageFolder folder, string fileName)
    {
        var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

        if (item is not null && item.IsOfType(StorageItemTypes.File))
        {
            var storageFile = await folder.GetFileAsync(fileName);
            var content = await storageFile.ReadBytesAsync();
            return content;
        }

        return null;
    }

    /// <summary>
    /// Method for reading the content of the specified StorageFile asynchronously as a byte array.
    /// </summary>
    /// <param name="file">The StorageFile to read.</param>
    /// <returns>A Task representing the asynchronous operation. The byte array content of the file, or null if the file does not exist or is not accessible.</returns>
    public static async Task<byte[]?> ReadBytesAsync(this StorageFile file)
    {
        if (file is not null)
        {
            using IRandomAccessStream stream = await file.OpenReadAsync();
            using var reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);
            var bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);
            return bytes;
        }

        return null;
    }

    private static string GetFileName(string name)
    {
        return string.Concat(name, FileExtension);
    }
}
