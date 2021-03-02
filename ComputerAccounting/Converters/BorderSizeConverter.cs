using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
                if (parameter.ToString() == "ComputerConfigControl")
                {
                    if ((value as ProfileViewModel).ComputerConfigControl != null)
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                }

                else if (parameter.ToString() == "LaboratoryInfoControl")
                {
                    if ((value as ProfileViewModel).LaboratoryInfoControl != null)
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                }

                else if (parameter.ToString() == "FullInfoControl")
                {
                    if ((value as ProfileViewModel).FullInfoControl != null)
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                }

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
