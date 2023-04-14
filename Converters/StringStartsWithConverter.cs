using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Examath.Core.Converters
{
    /// <summary>
    /// Checks whether the first string starts with the second string
    /// </summary>
    /// <remarks>
    /// Returns <c>value1.StartsWith(value2);</c>
    /// </remarks>
    internal class StringStartsWithStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is string text && values[1] is string query)
            {
                return text.StartsWith(query);
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
