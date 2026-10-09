using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Resources.ResXFileRef;

namespace Examath.Core.AutoForm
{
    /// <summary>
    /// Represents a dialog that generates its content based of the class
    /// </summary>
    public partial class AutoForm : Window
    {
        private object _Target;

        #region Constructors

        /// <summary>
        /// Creates a new AutoForm dialog with the specified target
        /// </summary>
        public AutoForm(object target)
        {
            InitializeComponent();
            _Target = target;

            // Populate
            foreach (PropertyInfo property in _Target.GetType().GetProperties())
            {
                if (property.GetCustomAttribute<AutoFormElementAttribute>() is AutoFormElementAttribute element)
                {
                    // Add to the master list
                    _Elements.Add(element);

                    // Create Binding
                    System.Windows.Data.Binding binding = new()
                    {
                        Source = _Target,
                        Path = new PropertyPath(property.Name),
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = element.UpdateSourceTrigger,
                    };

                    //if (Converter != null)
                    //{
                    //    binding.Converter = Converter;
                    //    binding.ConverterParameter = ConverterParameter;
                    //}

                    // Create control and apply binding
                    System.Windows.Controls.Control control = element._Control;

                    BindingOperations.SetBinding(control, element.DependencyProperty(), binding);
                    if (!string.IsNullOrWhiteSpace(element.Label)) control.Tag = element.Label;
                    if (!string.IsNullOrWhiteSpace(element.HelpText)) control.ToolTip = element.HelpText;

                    // Other
                    if (element.IsFocused) FocusControl = control;

                    // Add control
                    DisplayListBox.Items.Add(control);
                }
            }
        }

        #endregion

        #region Activated and Focus

        private List<AutoFormElementAttribute> _Elements = new();

        private System.Windows.Controls.Control? FocusControl;

        /// <summary>
        /// Initialises the dialog, and loads all the controls
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            // Attempt to set focus element to the first element
            FocusControl ??= _Elements.FirstOrDefault()?._Control;

            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = _Target.GetType().Name;
            }

            if (FocusControl != null)
            {
                if (FocusControl is TextBox textBox)
                {
                    textBox.SelectAll();
                }
                FocusControl.Focus();
            }

            base.OnActivated(e);
        }

        #endregion

        #region Static Shower

        /// <summary>
        /// Creates and shows an <see cref="AutoForm"/> window
        /// </summary>
        /// <param name="target">The object to generate the form from</param>
        /// <param name="title">The title of the window</param>
        /// <returns>True if 'ok' is pressed, otherwise false</returns>
        public static bool ShowDialog(object target, string title = "")
        {
            AutoForm autoForm = new(target)
            {
                Title = title,
            };
            bool? result = autoForm.ShowDialog();
            return result ?? false;
        }

        #endregion

        #region Ok and Cancel Buttons

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion
    }
}
