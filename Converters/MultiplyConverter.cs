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
    /// Multibinding converter for multiplying several values.
    /// </summary>
    /// <remarks>
    /// Adapted from code by Jason Frank, 2013 https://stackoverflow.com/questions/2186933/wpf-animation-binding-to-the-to-attribute-of-storyboard-animation/14164245#14164245
    /// </remarks>
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;

            foreach (object value in values)
            {
                if (value is double num) result *= num;
            }

            if (parameter is double pnum) result *= pnum;

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
