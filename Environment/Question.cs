using System.Windows.Controls;
using System.Windows.Data;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents a generic question and stores the answer
    /// </summary>
    /// <typeparam name="T">The type of the answer</typeparam>
    /// <typeparam name="B">The <see cref="IQuestionBlock"/> control to display the question</typeparam>
    public class Question<T, B> : IQuestionBlock where B : QuestionBlock, new()
    {
        internal B _Block;

        /// <summary>
        /// Gets or sets the always visible label for the question
        /// </summary>
        public string Label { get => _Block.Label; set => _Block.Label = value; }

        /// <summary>
        /// Gets or sets the tooltip of the question
        /// </summary>
        public string HelpText { get => _Block.HelpText; set => _Block.HelpText = value; }

        /// <summary>
        /// Gets or sets whether this question should be focused first
        /// </summary>
        public bool IsFocused { get => _Block.IsFocused; set => _Block.IsFocused = value; }

        /// <summary>
        /// Gets or sets the <see cref="UpdateSourceTrigger"/> of the binding between the answer and control
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger { get => _Block.UpdateSourceTrigger; set => _Block.UpdateSourceTrigger = value; }

        /// <summary>
        /// Gets or sets the answer of this question
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Creates a new question
        /// </summary>
        /// <param name="defaultValue">The default answer</param>
        /// <param name="label">The label for this question</param>
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

        /// <summary>
        /// Returns the string representation of the answer <see cref="Value"/> to this question
        /// </summary>
        public override string? ToString()
        {
            return Value?.ToString();
        }
    }

    /// <summary>
    /// Represents a question that accepts a <see cref="string"/> using a <see cref="TextBox"/>
    /// </summary>
    public class StringQ : Question<string, TextBoxInput>
    {
        /// <summary>
        /// Creates a new question that accepts a <see cref="string"/> using a <see cref="TextBox"/>
        /// </summary>
        public StringQ(string defaultValue = "", string label = "") : base(defaultValue, label)
        {
        }
    }

    /// <summary>
    /// Represents a question that accepts a <see cref="bool"/> using a <see cref="CheckBox"/>
    /// </summary>
    public class BoolQ : Question<bool, CheckBoxInput>
    {
        /// <summary>
        /// Creates a new question that accepts a <see cref="bool"/> using a <see cref="CheckBox"/>
        /// </summary>
        public BoolQ(bool defaultValue = false, object? content = null) : base(defaultValue)
        {
            _Block.Content = content;
        }
    }

    /// <summary>
    /// Represents a question that accepts a <see cref="bool"/> using a <see cref="CheckBox"/>
    /// </summary>
    public class IntQ : Question<int, TextBoxInput>
    {
        /// <summary>
        /// Creates a new question that accepts a <see cref="bool"/> using a <see cref="CheckBox"/>
        /// </summary>
        public IntQ(int defaultValue = 0, string label = "") : base(defaultValue)
        {
        }
    }
}
