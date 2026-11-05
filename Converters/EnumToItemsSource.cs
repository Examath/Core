using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Markup;

namespace Examath.Core.Converters
{
	/// <summary>
	/// A XAML markup extension that exposes the values of an enum type as an
	/// enumerable of descriptors (Value + Label). Useful for binding enum
	/// values to ItemsControl-based controls such as ComboBox.
	/// </summary>
	/// <remarks>
	/// Based on https://stackoverflow.com/a/17405771/10701111
	/// </remarks>
	public class EnumToItemsSource : MarkupExtension
	{
		private readonly Type? _Type;

		/// <summary>
		/// Initializes an empty <see cref="EnumToItemsSource"/>
		/// </summary>
		public EnumToItemsSource()
		{

		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnumToItemsSource"/> for the
		/// specified <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The enum <see cref="Type"/> to expose. Cannot be null.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is not an enum.</exception>
		public EnumToItemsSource(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}
			else
			{
				var enumType = Nullable.GetUnderlyingType(type) ?? type;
				if (enumType.IsEnum == false)
					throw new ArgumentException("Type must be an Enum.");
				else _Type = type;
			}
		}

		/// <summary>
		/// Returns an enumerable of <see cref="EnumValue"/> instances representing
		/// each value of the configured enum type. Suitable for use as an
		/// ItemsSource in XAML bindings.
		/// </summary>
		public override object? ProvideValue(IServiceProvider serviceProvider)
		{
			if (_Type == null) return null;
			return Enum.GetValues(_Type)
				.Cast<object>()
				.Select(e => new EnumValue(e, e.ToString() ?? "null"));
		}
	}

	/// <summary>
	/// Simple descriptor that pairs an enum value with a display name.
	/// </summary>
	public class EnumValue
	{
		/// <summary>
		/// The underlying enum value.
		/// </summary>
		public object Value { get; private set; }

		/// <summary>
		/// The user-facing display name for the value.
		/// </summary>
		public string Label { get; private set; }

		/// <summary>
		/// Creates a new <see cref="EnumValue"/>.
		/// </summary>
		public EnumValue(object value, string displayName)
		{
			Value = value;
			Label = displayName;
		}

		/// <summary>
		/// Returns the <see cref="Label"/>.
		/// </summary>
		public override string ToString()
		{
			return Label;
		}
	}
}
