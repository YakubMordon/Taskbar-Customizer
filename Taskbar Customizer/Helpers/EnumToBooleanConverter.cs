// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

/// <summary>
/// Converter, which converts an enum value to a boolean based on a specified parameter.
/// </summary>
public class EnumToBooleanConverter : IValueConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumToBooleanConverter"/> class.
    /// </summary>
    public EnumToBooleanConverter()
    {
    }

    /// <summary>
    /// Method, which converts an enum value to a boolean based on the specified parameter.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>True if the enum value matches the specified parameter; otherwise, false.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            if (!Enum.IsDefined(typeof(ElementTheme), value))
            {
                throw new ArgumentException("Value must be a valid ElementTheme enum.", nameof(value));
            }

            var enumValue = Enum.Parse(typeof(ElementTheme), enumString);
            return enumValue.Equals(value);
        }

        throw new ArgumentException("Parameter must be a valid enum name.", nameof(parameter));
    }

    /// <summary>
    /// Method, which converts a boolean value back to the corresponding enum value based on the specified parameter.
    /// </summary>
    /// <param name="value">The boolean value to convert back.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>The corresponding enum value based on the specified parameter.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            return Enum.Parse(typeof(ElementTheme), enumString);
        }

        throw new ArgumentException("Parameter must be a valid enum name.", nameof(parameter));
    }
}
