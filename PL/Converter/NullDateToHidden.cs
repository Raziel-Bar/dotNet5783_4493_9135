using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace PL.Converter;

/// <summary>
/// The NullDateToHidden class converts a nullable DateTime value to a Visibility value
/// It implements the IValueConverter interface, which allows it to be used in data binding scenarios
/// where the source and target properties have different types
/// </summary>
[ValueConversion(typeof(DateTime?), typeof(Visibility))]
internal class NullDateToHidden : IValueConverter
{
    /// <summary>
    /// The Convert method takes a nullable DateTime value and converts it to a Visibility value
    /// If the DateTime value is not null, it returns Visibility.Hidden
    /// If the DateTime value is null, it returns Visibility.Visible
    /// </summary>
    /// <param name="value">The nullable DateTime value to convert</param>
    /// <param name="targetType">The type of the target property</param>
    /// <param name="parameter">An optional parameter</param>
    /// <param name="culture">The culture information to use</param>
    /// <returns>A Visibility value</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (DateTime?)value != null ? Visibility.Hidden : Visibility.Visible;
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


