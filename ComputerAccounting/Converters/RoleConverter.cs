using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ComputerAccounting
{
    class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            EnumHelper.GetEnumDescription((Role)Enum.Parse(typeof(Role), value.ToString()));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return EnumHelper.GetValueFromDescription<Role>(value.ToString());
            else
                return "";
        }
    }
}
