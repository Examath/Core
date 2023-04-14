using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class Question<T, B> : IQuestionBlock where B : QuestionBlock, new()
    {
        private B _Block;

        public string Label { get => _Block.Label; set => _Block.Label = value; }

        public string HelpText { get => _Block.HelpText; set => _Block.HelpText = value; }

        public bool IsFocused { get => _Block.IsFocused; set => _Block.IsFocused = value; }

        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => _Block.UpdateSourceTrigger;
            set => _Block.UpdateSourceTrigger = value;
        }

        public T Value { get; set; }

        public Question(T defaultValue, string label = "")
        {
            Value = defaultValue;
            _Block = new B()
            {
                Label = label,
                Source = this,
                Property = nameof(Value),
            };
        }

        void IAskerBlock.Finish(Control control)
        {
            _Block.Finish(control);
        }

        Control IAskerBlock.GetControl()
        {
            return _Block.GetControl();
        }

        public override string? ToString()
        {
            return Value?.ToString();
        }
    }

    public class StringQ : Question<string, TextBoxInput>
    {
        public StringQ(string defaultValue = "", string label = "") : base(defaultValue, label)
        {
        }
    }
}
