using System.Windows;
using System.Windows.Controls;

namespace Examath.Core.AutoForm
{
    /// <summary>
    /// Visualises the property as a <see cref="CheckBox"/> in the <see cref="AutoForm"/> dialog
    /// </summary>
    public sealed class TextBoxAttribute : AutoFormElementAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override DependencyProperty? DependencyProperty() => TextBox.TextProperty;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override Control CreateControl()
        {
            TextBox textBox = new();
            return textBox;
        }
    }
}
