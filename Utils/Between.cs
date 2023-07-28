using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Utils
{
    /// <summary>
    /// Conditional operator that checks if a value is within a range
    /// </summary>
    public static class Between
    {        /// <summary>
             /// Determines whether <paramref name="target"/> is between <paramref name="min"/> and <paramref name="max"/> exclusivley
             /// </summary>
             /// <typeparam name="T">The <see cref="IComparable"/> object to compare</typeparam>
             /// <param name="target">The target of the conditional operation</param>
             /// <param name="min">The minimum value <paramref name="target"/> may have</param>
             /// <param name="max">The maximum value <paramref name="target"/> may have</param>
             /// <returns>True if <paramref name="min"/> &lt; <paramref name="target"/> &lt; <paramref name="max"/>, false otherwise</returns>
        public static bool Exclusive<T>(T min, T target, T max) where T : IComparable<T>
        {
            return (min.CompareTo(target) < 0) && (max.CompareTo(target) > 0);
        }

        /// <summary>
        /// Determines whether <paramref name="target"/> is between <paramref name="min"/> and <paramref name="max"/> inclusivley
        /// </summary>
        /// <typeparam name="T">The <see cref="IComparable"/> object to compare</typeparam>
        /// <param name="target">The target of the conditional operation</param>
        /// <param name="min">The minimum value <paramref name="target"/> may have</param>
        /// <param name="max">The maximum value <paramref name="target"/> may have</param>
        /// <returns>True if <paramref name="min"/> &lt;= <paramref name="target"/> &lt;= <paramref name="max"/>, false otherwise</returns>
        public static bool Inclusive<T>(T min, T target, T max) where T : IComparable<T>
        {
            return (min.CompareTo(target) <= 0) && (max.CompareTo(target) >= 0);
        }
    }
}
