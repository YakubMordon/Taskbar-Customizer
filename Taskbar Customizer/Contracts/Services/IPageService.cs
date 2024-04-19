// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.Services;

/// <summary>
/// Contract for a service that retrieves page types based on a given key.
/// </summary>
public interface IPageService
{
    /// <summary>
    /// Method, which retrieves the type of page based on the provided key.
    /// </summary>
    /// <param name="key">The key associated with the page type.</param>
    /// <returns>The type of the page corresponding to the key, or null if not found.</returns>
    Type GetPageType(string key);
}
