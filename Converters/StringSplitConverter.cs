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
    public class StringSplitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString() ?? "";
            if (parameter is StringSplitConverterParameter stringSplitConverterParameter)
            {
                string[] parts = input.Split(stringSplitConverterParameter.Separator);
                return parts[Math.Min(parts.Length - 1, stringSplitConverterParameter.Index)];
            }
            else return input.Split(' ')[0];
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringSplitConverterParameter
    {
        public string Separator { get; set; } = " ";
        public int Index { get; set; } = 0;

        public StringSplitConverterParameter()
        {

        }
    }
}