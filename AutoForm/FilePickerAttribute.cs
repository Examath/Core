using Examath.Core.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Examath.Core.AutoForm
{
    /// <summary>
    /// Visualises the property as a <see cref="CheckBox"/> in the <see cref="AutoForm"/> dialog
    /// </summary>
    public sealed class FilePickerAttribute : AutoFormElementAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override DependencyProperty? DependencyProperty() => FilePicker.FileNameProperty;

        /// <summary>
        /// Gets or sets the extension to use
        /// </summary>
        public string ExtensionFilter { get; set; } = "All files|*.*";

        /// <summary>
        /// Gets or sets whether to use the <see cref="SaveFileDialog"/> instead of the <see cref="OpenFileDialog"/>
        /// </summary>
        public bool UseSaveFileDialog { get; set; } = false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override Control CreateControl()
        {
            FilePicker filePicker = new()
            {
                ExtensionFilter = ExtensionFilter,
                UseSaveFileDialog = UseSaveFileDialog,
            };
            return filePicker;
        }
    }
}
