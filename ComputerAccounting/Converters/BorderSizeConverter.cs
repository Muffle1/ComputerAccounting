using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputerAccounting
{
    public class BorderSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (typeof(ProfileViewModel).GetProperty(parameter.ToString()).GetValue(value as ProfileViewModel) != null)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Hidden;
        }
    }
}
