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
    /// <summary>
    /// Class to support animation of the ToggleButton
    /// </summary>
    public class ToggleButtonIndicatorPositionConverter : IMultiValueConverter
    {
        private const double _WIDTH = 2.0;

        /// <summary>
        /// Takes in desired position and height of element, 
        /// and returns the thickness required to drive the margin of the
        /// contrast style toggle button
        /// </summary>
        /// <param name="values">First value should be the double indicating position. 
        /// Second value should be a double indicating actual height of the control.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is double pos && values[1] is double height)
            {
                return new Thickness(0, (height - _WIDTH) * (1 - pos), 0, _WIDTH * pos);
            }
            else return new Thickness(3);
        }
        /// <summary>
        /// Not implemented.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
