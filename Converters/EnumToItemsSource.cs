using System;
using System.Linq;
using System.Windows.Markup;

namespace Examath.Core.Converters
{
    /// <summary>
    /// Markup extension that populates itemcontrols with the values of an enum
    /// </summary>
    /// <remarks>
    /// By https://stackoverflow.com/a/17405771/10701111
    /// </remarks>
    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new EnumValue(e, e.ToString() ?? "null"));
        }
    }

    public class EnumValue
    {
        public object Value { get; private set; }

        public string DisplayName { get; private set; }

        public EnumValue(object value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
