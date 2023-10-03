using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Model
{
    /// <summary>
    /// Represents an event argument for when critical data is changed
    /// </summary>
    public class DataChangedEventArgs
    {
        /// <summary>
        /// Gets the object that was modified
        /// </summary>
        public object? Object { get; private set; }

        /// <summary>
        /// Gets the property that was changed
        /// </summary>
        public string? Property { get; private set; }

        public DateTime Created { get; private set; } = DateTime.Now;

        /// <summary>
        /// Creates a simple <see cref="DataChangedEventArgs"/> with a target <paramref name="obj"/> and <paramref name="property"/>
        /// </summary>
        /// <param name="obj">The object that was modified</param>
        /// <param name="property">The property of <paramref name="obj"/> that was modified</param>
        public DataChangedEventArgs(object? obj, string? property)
        {
            Object = obj;
            Property = property;
        }

        /// <summary>
        /// Returns the string representation of this event
        /// </summary>
        public override string ToString()
        {
            return $"{Object}.{Property} changed @ {Created}";
        }
    }
}
