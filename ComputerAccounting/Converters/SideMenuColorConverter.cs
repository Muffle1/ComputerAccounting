using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputerAccounting
{
    public class SideMenuColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NameSideMenu nameSideMenuValue = (NameSideMenu)Enum.Parse(typeof(NameSideMenu), value.ToString());
            NameSideMenu nameSideMenuParameter = (NameSideMenu)Enum.Parse(typeof(NameSideMenu), parameter.ToString());

            if (((nameSideMenuValue == NameSideMenu.FirstMenu) && (nameSideMenuParameter == NameSideMenu.FirstMenu)) ||
                ((nameSideMenuValue == NameSideMenu.SecondMenu) && (nameSideMenuParameter == NameSideMenu.SecondMenu)))
                return Visibility.Visible;

            if (((nameSideMenuValue == NameSideMenu.FirstMenu) && (nameSideMenuParameter == NameSideMenu.SecondMenu)) ||
                ((nameSideMenuValue == NameSideMenu.SecondMenu) && (nameSideMenuParameter == NameSideMenu.FirstMenu)))
                return Visibility.Collapsed;

            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
