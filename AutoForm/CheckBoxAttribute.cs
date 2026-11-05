using System.Windows;
using System.Windows.Controls;

namespace Examath.Core.AutoForm
{
    /// <summary>
    /// Visualises the property as a <see cref="CheckBox"/> in the <see cref="AutoForm"/> dialog
    /// </summary>
    public sealed class CheckBoxAttribute : AutoFormElementAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override DependencyProperty? DependencyProperty() => System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty;

        /// <summary>
        /// Gets or sets the content in the CheckBox (as opposed to <see cref="Label"/>)
        /// </summary>
        public string? Content { get;set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override Control CreateControl()
        {
            CheckBox checkBox = new();
            if (Content != null) checkBox.Content = Content;
            return checkBox;
        }
    }
}
