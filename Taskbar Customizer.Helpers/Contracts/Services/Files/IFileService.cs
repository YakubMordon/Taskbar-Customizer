// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Core.Contracts.Services.Files;

/// <summary>
/// Contract for file operations.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Method for reading from file.
    /// </summary>
    /// <typeparam name="T">Object type for reading.</typeparam>
    /// <param name="folderPath">Folder path.</param>
    /// <param name="fileName">File name.</param>
    /// <returns>Deserialized object.</returns>
    T Read<T>(string folderPath, string fileName);

    /// <summary>
    /// Method for saving into file.
    /// </summary>
    /// <typeparam name="T">Object type for saving.</typeparam>
    /// <param name="folderPath">Folder path.</param>
    /// <param name="fileName">File name.</param>
    /// <param name="content">Content for saving into file.</param>
    void Save<T>(string folderPath, string fileName, T content);

    /// <summary>
    /// Method for deleting file.
    /// </summary>
    /// <param name="folderPath">Folder path.</param>
    /// <param name="fileName">File name.</param>
    void Delete(string folderPath, string fileName);
}
