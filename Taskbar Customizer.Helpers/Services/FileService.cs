// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Services;

using System.Text;

using Newtonsoft.Json;
using Taskbar_Customizer.Helpers.Contracts.Services;

/// <summary>
/// Service for operation with files.
/// </summary>
public class FileService : IFileService
{
    /// <inheritdoc />
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);

        var json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <inheritdoc />
    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    /// <inheritdoc />
    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}
