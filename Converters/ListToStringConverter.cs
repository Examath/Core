using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Examath.Core.Converters
{
    internal class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string separator = (parameter is string s) ? s : "\r\n";
            if (value is List<string> list)
            {
                return string.Join(separator, list);
            }
            else return string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string separator = (parameter is string s) ? s : "\r\n";
            if (value is string str)
            {
                return str.Split(separator).ToList();
            }
            else return null;
        }
    }
}
