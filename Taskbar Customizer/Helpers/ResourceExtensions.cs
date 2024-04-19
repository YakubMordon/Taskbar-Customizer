// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using Microsoft.Windows.ApplicationModel.Resources;

/// <summary>
/// Class with extension methods for retrieving localized strings using a <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceLoader"/>.
/// </summary>
public static class ResourceExtensions
{
    /// <summary>
    /// Resource loader.
    /// </summary>
    private static readonly ResourceLoader ResourceLoader = new ResourceLoader();

    /// <summary>
    /// Method for getting the localized string corresponding to the specified resource key.
    /// </summary>
    /// <param name="resourceKey">The resource key used to retrieve the localized string.</param>
    /// <returns>The localized string associated with the resource key, or <see langword="null"/> if the key is not found.</returns>
    public static string GetLocalized(this string resourceKey)
    {
        return ResourceLoader.GetString(resourceKey);
    }
}
