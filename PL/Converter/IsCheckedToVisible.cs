using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace PL.Converter;

/// <summary>
/// The IsCheckedToVisible class converts a boolean value to a Visibility value
/// It implements the IValueConverter interface, which allows it to be used in data binding scenarios
/// where the source and target properties have different types
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
internal class IsCheckedToVisible : IValueConverter
{
    /// <summary>
    /// The Convert method takes a boolean value and converts it to a Visibility value
    /// If the boolean value is true, it returns Visibility.Visible
    /// If the boolean value is false, it returns Visibility.Collapsed
    /// </summary>
    /// <param name="value">The boolean value to convert</param>
    /// <param name="targetType">The type of the target property</param>
    /// <param name="parameter">An optional parameter</param>
    /// <param name="culture">The culture information to use</param>
    /// <returns>A Visibility value</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// The ConvertBack method is not implemented in this class, so it throws a NotImplementedException
    /// </summary>
    /// <param name="value">The value to convert</param>
    /// <param name="targetType">The type of the target property</param>
    /// <param name="parameter">An optional parameter</param>
    /// <param name="culture">The culture information to use</param>
    /// <returns>Not implemented</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
