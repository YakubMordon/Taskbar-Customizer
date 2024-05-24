// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Converters;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

/// <summary>
/// Converter, which converts an enum value to a boolean based on a specified parameter.
/// </summary>
public class EnumToBooleanConverter : IValueConverter
{
    private readonly Type elementTheme;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumToBooleanConverter"/> class.
    /// </summary>
    public EnumToBooleanConverter()
    {
        this.elementTheme = typeof(ElementTheme);
    }

    /// <summary>
    /// Method, which converts an enum value to a boolean based on the specified parameter.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>True if the enum value matches the specified parameter; otherwise, false.</returns>
    public object Convert(object value, Type targetType, object parameter, string language) => value.Equals(Enum.Parse(this.elementTheme, (string)parameter));

    /// <summary>
    /// Method, which converts a boolean value back to the corresponding enum value based on the specified parameter.
    /// </summary>
    /// <param name="value">The boolean value to convert back.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The parameter specifying the enum type name.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>The corresponding enum value based on the specified parameter.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => Enum.Parse(this.elementTheme, (string)parameter);
}
