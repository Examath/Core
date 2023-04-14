using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examath.Core.Controls
{

    //http://www.nullskull.com/a/1401/creating-a-wpf-custom-control.aspx
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Line.Interface"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Line.Interface;assembly=Line.Interface"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TextBoxButton/>
    ///
    /// </summary>
    public class TextBoxButton : ContentControl
    {
        #region Constants

        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";

        #endregion

        static TextBoxButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxButton), new FrameworkPropertyMetadata(typeof(TextBoxButton)));
        }

        #region Public Properties

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(TextBoxButton),
                new FrameworkPropertyMetadata(
                    String.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTextChanged),
                    null,
                    true,
                    UpdateSourceTrigger.PropertyChanged
                    )
                );

        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
                "TextChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>),
                typeof(TextBoxButton));

        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add { base.AddHandler(TextChangedEvent, value); }
            remove { base.RemoveHandler(TextChangedEvent, value); }
        }

        protected virtual void OnTextChanged(string oldText, string newText)
        {
            RoutedPropertyChangedEventArgs<string> e = new(oldText, newText)
            {
                RoutedEvent = TextChangedEvent
            };
            base.RaiseEvent(e);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxButton textBoxButton = (TextBoxButton)d;
            textBoxButton.OnTextChanged((string)e.OldValue, (string)e.NewValue);
        }

        #endregion

        #region Click

        /// <summary>
        /// Event correspond to left mouse button click on button
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxButton));

        /// <summary>
        /// Add / Remove ClickEvent handler
        /// </summary>
        public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }

        internal virtual void OnClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new(ClickEvent, this);
            base.RaiseEvent(routedEventArgs);
        }

        #endregion

        #region ButtonStyle

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(TextBoxButton), new PropertyMetadata(null));


        #endregion

        #endregion

        #region Data

        internal TextBox? _TextBox;
        private Button? _Button;

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _TextBox = (TextBox)GetTemplateChild(ElementTextBox);

            _Button = (Button)GetTemplateChild(ElementButton);
            if (_Button != null)
            {
                _Button.Click += OnClick;
            }
        }

        #endregion
    }
}
