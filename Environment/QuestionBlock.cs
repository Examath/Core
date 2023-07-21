using Examath.Core.Controls;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Examath.Core.Environment
{
    public interface IQuestionBlock : IAskerBlock
    {
        public string Label { get; set; }

        public string HelpText { get; set; }

        public bool IsFocused { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
    }

    public abstract class QuestionBlock : IQuestionBlock
    {
        public object? Source { get; set; }

        public string Property { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public DependencyProperty? DisplayDependencyProperty { get; set; }

        public string HelpText { get; set; } = string.Empty;

        public bool IsFocused { get; set; } = false;

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; } = UpdateSourceTrigger.Default;

        public IValueConverter? Converter { get; set; }

        /// <summary>
        /// Gets or sets the parameter to be used on the converter
        /// </summary>
        public object ConverterParameter { get; set; }

        public QuestionBlock()
        {

        }

        public QuestionBlock(object source, string property, string label, DependencyProperty displayDependencyProperty)
        {
            Label = label;
            Source = source;
            Property = property;
            DisplayDependencyProperty = displayDependencyProperty;
        }

        /// <summary>
        /// Creates and returns a bound control for display in the <see cref="Asker"/> dialog
        /// </summary>
        /// <param name="control">The control to apply the binding to</param>
        /// <returns>The control for display in the <see cref="Asker"/> dialog</returns>
        protected virtual Control Initialise(Control control)
        {
            Binding binding = new()
            {
                Source = Source,
                Path = new PropertyPath(Property),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger,
            };

            if (Converter != null)
            {
                binding.Converter = Converter;
                binding.ConverterParameter = ConverterParameter;
            }

            BindingOperations.SetBinding(control, DisplayDependencyProperty, binding);
            if (!string.IsNullOrWhiteSpace(Label)) control.Tag = Label;
            if (!string.IsNullOrWhiteSpace(HelpText)) control.ToolTip = HelpText;
            return control;
        }

        public abstract Control GetControl();

        public virtual void Finish(Control control)
        {
            BindingExpression bindingExpression = control.GetBindingExpression(DisplayDependencyProperty);
            bindingExpression?.UpdateSource();
            BindingOperations.ClearAllBindings(control);
        }
    }

    /// <summary>
    /// Represents a question that uses a <see cref="TextBox"/> in the <see cref="Asker"/> dialog
    /// </summary>
    public class TextBoxInput : QuestionBlock
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TextBoxInput() : base()
        {
            DisplayDependencyProperty = TextBox.TextProperty;
        }

        /// <summary>
        /// Creates a question meant for a a <see cref="TextBox"/>
        /// </summary>
        /// <param name="source">Source of binding</param>
        /// <param name="property">Property to bind</param>
        /// <param name="label">Optional label</param>
        /// <param name="helpText">Optional help text</param>
        public TextBoxInput(object source, string property, string label = "")
            : base(source, property, label, TextBox.TextProperty)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override Control GetControl()
        {
            return Initialise(new TextBox());
        }
    }

    /// <summary>
    /// Represents a question that uses a <see cref="TextBox"/> in the <see cref="Asker"/> dialog to edit a list
    /// </summary>
    public class ListTextBoxInput : QuestionBlock
    {
        /// <summary>
        /// Gets or sets the separator to use
        /// </summary>
        public string Separator = "\n";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ListTextBoxInput() : base()
        {
            DisplayDependencyProperty = TextBox.TextProperty;
            Converter = new Converters.ListToStringConverter();
        }

        /// <summary>
        /// Creates a question meant for a a <see cref="TextBox"/>
        /// </summary>
        /// <param name="source">Source of binding</param>
        /// <param name="property">Property to bind</param>
        /// <param name="label">Optional label</param>
        /// <param name="helpText">Optional help text</param>
        public ListTextBoxInput(object source, string property, string label = "")
            : base(source, property, label, TextBox.TextProperty)
        {
            Converter = new Converters.ListToStringConverter();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override Control GetControl()
        {
            TextBox textBox = new TextBox();
            if (Separator.Contains('\n')) textBox.AcceptsReturn = true;
            if (Separator.Contains('\t')) textBox.AcceptsTab = true;
            ConverterParameter = Separator;
            return Initialise(textBox);
        }
    }

    /// <summary>
    /// Represents a question that uses a <see cref="CheckBox"/> in the <see cref="Asker"/> dialog
    /// </summary>
    public class CheckBoxInput : QuestionBlock
    {
        public object? Content { get; set; }

        public CheckBoxInput() : base()
        {
            DisplayDependencyProperty = System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty;
        }

        /// <summary>
        /// Creates a question meant for a a <see cref="CheckBox"/>
        /// </summary>
        /// <param name="source">Source of binding</param>
        /// <param name="property">Property to bind</param>
        /// <param name="content">Content of the CheckBox</param>
        /// <param name="helpText">Optional help text</param>
        public CheckBoxInput(object source, string property, object? content = null)
            : base(source, property, "", System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty)
        {
            Content = content;
        }

        public override Control GetControl()
        {
            CheckBox checkBox = new()
            {
                Content = Content,
            };
            return Initialise(checkBox);
        }
    }

    public class FilePickerInput : QuestionBlock
    {
        public string Directory { get; set; } = "";

        public string ExtensionFilter { get; set; } = "All files|*.*";

        public bool UseSaveFileDialog { get; set; } = false;

        public FilePickerInput() : base()
        {
            DisplayDependencyProperty = Controls.FilePicker.FileNameProperty;
        }

        public FilePickerInput(object source, string property, string label = "")
            : base(source, property, label, FilePicker.FileNameProperty)
        {
        }

        public override Control GetControl()
        {
            FilePicker filePicker = new();
            filePicker.ExtensionFilter = ExtensionFilter;
            filePicker.Directory = Directory;
            filePicker.UseSaveFileDialog = UseSaveFileDialog;
            return Initialise(filePicker);
        }
    }

    public class ComboBoxInput : QuestionBlock
    {
        public IEnumerable? ItemsSource { get; set; }

        public ComboBoxInput() : base()
        {
            DisplayDependencyProperty = System.Windows.Controls.Primitives.Selector.SelectedItemProperty;
        }

        public ComboBoxInput(object source, string property, IEnumerable itemsSource, string label = "")
            : base(source, property, label, System.Windows.Controls.Primitives.Selector.SelectedItemProperty)
        {
            ItemsSource = itemsSource;
        }

        public override Control GetControl()
        {
            ComboBox comboBox = new() { ItemsSource = ItemsSource };
            return Initialise(comboBox);
        }
    }
}
