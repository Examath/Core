using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Extensions
{
    /// <summary>
    /// Useful extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks whether this string is null or in the ASO format for null refrence
        /// </summary>
        /// <param name="nullable">The string to check</param>
        /// <returns>True if <paramref name="value"/> parameter is either <see cref="string.IsNullOrEmpty(string?)"/>or is the string "null"</returns>
        public static bool IsASONull(this string value)
        {
            return string.IsNullOrEmpty(value) || value == "null";
        }
    }
}
