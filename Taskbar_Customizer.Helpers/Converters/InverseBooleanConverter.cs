// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Converters;

using System;

using Microsoft.UI.Xaml.Data;

/// <summary>
/// Converter, which converts a boolean to inversed value.
/// </summary>
public class InverseBooleanConverter : IValueConverter
{
    /// <summary>
    /// Method, which converts a boolean to inversed value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name (not used).</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>Inversed value of boolean if it was able to inverse; otherwise, false.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool booleanValue)
        {
            return !booleanValue;
        }

        return false;
    }

    /// <summary>
    /// Method, which converts a boolean back to normal value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name (not used).</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>Normal value of boolean if it was able to inverse back; otherwise, false.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool booleanValue)
        {
            return !booleanValue;
        }

        return false;
    }
}