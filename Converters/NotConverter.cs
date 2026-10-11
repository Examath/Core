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
    /// Performs a not operation on bound value
    /// </summary>
    public class NotConverter : IValueConverter
    {
        /// <summary>
        /// Inverts the boolean value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool param) return !param;
            else return false;
        }

		/// <summary>
		/// Inverts the boolean value
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool param) return !param;
            else return false;
        }
    }
}
