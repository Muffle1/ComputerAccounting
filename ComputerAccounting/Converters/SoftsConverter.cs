using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ComputerAccounting
{
    public class SoftsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new List<Soft>();
            List<Soft> softs = new List<Soft>();
            foreach (var soft in ((string)value).Split(", "))
                softs.Add(new Soft() { SoftName = soft });
            return softs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
