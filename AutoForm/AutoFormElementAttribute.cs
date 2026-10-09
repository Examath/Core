using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Resources.ResXFileRef;

namespace Examath.Core.AutoForm
{
    /// <summary>
    /// Marks properties for use in the <see cref="AutoForm"/> dialog
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class AutoFormElementAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        /// <summary>
        /// <inheritdoc cref="AutoFormElementAttribute"/>
        /// </summary>
        public AutoFormElementAttribute()
        {
            _Control = CreateControl();
        }

        /// <summary>
        /// Gets or sets the primary label of this element
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the help text of this element
        /// </summary>
        public string? HelpText { get;set; }

        /// <summary>
        /// Gets or sets whether this element should be focused
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Gets or sets the binding's <see cref="System.Windows.Data.UpdateSourceTrigger"/>
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        /// <summary>
        /// The dependency property that the default initialiser uses
        /// </summary>
        protected internal abstract DependencyProperty? DependencyProperty();

        /// <summary>
        /// Creates the control associated with this <see cref="AutoFormElementAttribute"/>
        /// </summary>
        /// <returns>The created control</returns>
        protected abstract Control CreateControl();

        internal Control _Control;
    }
}
