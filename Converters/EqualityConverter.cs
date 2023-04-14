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
    /// Checks whether the bound value is equal to the converter parameter.
    /// </summary>
    /// <remarks>
    /// Returns <c>Equals(value, parameter)</c>
    /// </remarks>
    public class EqualityConverter : IValueConverter
    {
        /// <param name="value">The bound value to check</param>
        /// <param name="targetType">Whether to return a <see cref="bool"/> or <see cref="Visibility"/></param>
        /// <param name="parameter">What to check against</param>
        /// <param name="culture"></param>
        /// <returns>True if <paramref name="value"/>.ToString() equals to <paramref name="parameter"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
