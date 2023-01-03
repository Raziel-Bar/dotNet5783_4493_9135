using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace PL.Converter;

[ValueConversion(typeof(bool), typeof(Visibility))]
internal class IsCheckedToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    => (bool)value ? Visibility.Visible : Visibility.Hidden;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();

}
