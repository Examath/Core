using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examath.Core.Controls
{
    /// <summary>
    /// A label that is coloured when true
    /// </summary>
    public class BoolOutput : Control
    {
        #region IsChecked

        /// <summary>
        /// Gets or sets whether the output is 'on'
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Backing property for <see cref="IsChecked"/>: <inheritdoc cref="IsChecked"/>
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(BoolOutput), new PropertyMetadata(false));


        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text displayed on the control
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Backing property for <see cref="Text"/>: <inheritdoc cref="Text"/>
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BoolOutput), new PropertyMetadata("X"));

        #endregion

        #region TrueText
        /// <summary>
        /// Gets or sets the text displayed when <see cref="IsChecked"/> is false.
        /// </summary>
        /// <remarks>
        /// If <see cref="TrueText"/> is null then <see cref="Text"/> is displayed always.
        /// </remarks>
        public string? TrueText
        {
            get { return (string?)GetValue(TrueTextProperty) ?? Text; }
            set { SetValue(TrueTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...Text
        /// <summary>
        /// Backing property for <see cref="TrueText"/>: <inheritdoc cref="TrueText"/>
        /// </summary>
        public static readonly DependencyProperty TrueTextProperty =
            DependencyProperty.Register("TrueText", typeof(string), typeof(BoolOutput), new PropertyMetadata(null));

        #endregion

        static BoolOutput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BoolOutput), new FrameworkPropertyMetadata(typeof(BoolOutput)));
        }
    }
}
