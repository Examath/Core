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
    /// Splits a string by provided parameter and then returns
    /// </summary>
    public class StringMultiplexerConverter : IValueConverter
    {
        /// <summary>
        /// The separator used in the parameter
        /// </summary>
        public const char Separator = '|';
        private readonly string[] defArray = { "False", "True", "Overflow" };

        /// <summary>
        /// Multiplexes the <paramref name="value"/> with the <paramref name="parameter"/>
        /// </summary>
        /// <param name="value">The boolean or integer value to multiplex with</param>
        /// <param name="parameter">A string with multiplexer outputs separated by '|' <see cref="Separator"/></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] array = (parameter is string str) ? str.Split(Separator) : defArray;

            int index = 0;

            if (value is bool b) index = b ? 1 : 0;
            else if (value is int i) index = i;

            index = Math.Clamp(index, 0, array.Length);

            return array[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}