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

        public override Control GetControl()
        {
            return Initialise(new TextBox());
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
}
