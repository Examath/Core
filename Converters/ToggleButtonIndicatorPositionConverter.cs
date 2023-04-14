using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Examath.Core.Converters
{
    public class ToggleButtonIndicatorPositionConverter : IMultiValueConverter
    {
        private const double _WIDTH = 2.0;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is double pos && values[1] is double height)
            {
                return new Thickness(0, (height - _WIDTH) * (1 - pos), 0, _WIDTH * pos);
            }
            else return new Thickness(3);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
