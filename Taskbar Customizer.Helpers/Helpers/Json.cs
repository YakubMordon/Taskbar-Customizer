// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Core.Helpers;

using Newtonsoft.Json;

/// <summary>
/// Static class for JSON-serialization.
/// </summary>
public static class Json
{
    /// <summary>
    /// Method for Deserialization of object.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="value">JSON value.</param>
    /// <returns>Deserialized object.</returns>
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run<T>(() => JsonConvert.DeserializeObject<T>(value));
    }

    /// <summary>
    /// Method for Serialization of object.
    /// </summary>
    /// <param name="value">Value for serialization.</param>
    /// <returns>Serialized value.</returns>
    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run(() => JsonConvert.SerializeObject(value));
    }
}
