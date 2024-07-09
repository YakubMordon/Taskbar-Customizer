// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Converters;

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

/// <summary>
/// Converts a boolean value to a Visibility value (Visible or Collapsed).
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Method for Convertion a boolean value to a Visibility value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the target property (Visibility).</param>
    /// <param name="parameter">An optional parameter used for custom conversion (not used).</param>
    /// <param name="language">The language for which the conversion is applied (not used).</param>
    /// <returns>Visible if the value is true; Collapsed if the value is false.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var result = Visibility.Collapsed;

        var visibility = (bool)value;

        if (visibility)
        {
            result = Visibility.Visible;
        }

        return result;
    }

    /// <summary>
    /// Method for Convertion a Visibility value back to a boolean value (not supported in this converter).
    /// </summary>
    /// <param name="value">The Visibility value to convert back.</param>
    /// <param name="targetType">The type to convert back to (boolean).</param>
    /// <param name="parameter">An optional parameter used for custom conversion (not used).</param>
    /// <param name="language">The language for which the conversion is applied (not used).</param>
    /// <returns>NotImplementedException is thrown since ConvertBack is not supported.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException("ConvertBack method is not supported in BooleanToVisibilityConverter.");
    }
}