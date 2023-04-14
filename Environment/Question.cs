using System.Windows.Controls;
using System.Windows.Data;

namespace Examath.Core.Environment
{
    public class Question<T, B> : IQuestionBlock where B : QuestionBlock, new()
    {
        internal B _Block;

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

    public class BoolQ : Question<bool, CheckBoxInput>
    {
        public BoolQ(bool defaultValue = false, object? content = null) : base(defaultValue)
        {
            _Block.Content = content;
        }
    }
}
