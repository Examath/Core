using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Interaction logic for Asker.xaml
    /// </summary>
    public partial class Asker : Window
    {
        private IAskerBlock[] _AskerBlocks;

        /// <summary>
        /// Creates a new dialog
        /// </summary>
        /// <param name="askerOptions">An <see cref="AskerOptions"/> object containing custom properties. 
        /// Set to null to use default settings</param>
        /// <param name="askerBlocks">A list of blocks for this dialog</param>
        public Asker(AskerOptions? askerOptions, params IAskerBlock[] askerBlocks)
        {
            InitializeComponent();

            if (askerOptions != null)
            {
                Title = askerOptions.Title;
                if (askerOptions.CanCancel)
                {
                    CancelButton.Visibility = Visibility.Visible;
                }
            }
            _AskerBlocks = askerBlocks;
            AskerGroup.PopulateBlocks(DisplayListBox, _AskerBlocks);
        }

        /// <summary>
        /// Initialises the dialog, and loads all the controls
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            bool firstFocus = false;
            for (int i = 0; i < DisplayListBox.Items.Count; i++)
            {
                if (_AskerBlocks[i] is IQuestionBlock questionBlock)
                {
                    if (questionBlock.IsFocused)
                    {
                        Focus(i);
                        break;
                    }
                    else if (!firstFocus)
                    {
                        Focus(i);
                        firstFocus = true;
                    }
                }
            }

            base.OnActivated(e);

            void Focus(int i)
            {
                Control control = (Control)DisplayListBox.Items.GetItemAt(i);
                if (control is TextBox textBox)
                {
                    textBox.SelectAll();
                }
                control.Focus();
            }
        }

        /// <summary>
        /// Allows for the removal of bindings
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            AskerGroup.FinishBlocks(DisplayListBox, _AskerBlocks);
            base.OnClosing(e);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Creates and shows a new <see cref="Asker"/> dialog
        /// </summary>
        /// <param name="askerOptions">The <see cref="AskerOptions"/> for the dialog. Use null for default.</param>
        /// <param name="askerBlocks"><see cref="IAskerBlock"/>s to display in this dialog</param>
        /// <returns>The result of the dialog. True if true and false if (false or null).</returns>
        public static bool Show(AskerOptions? askerOptions, params IAskerBlock[] askerBlocks)
        {
            Asker asker = new(askerOptions, askerBlocks);
            bool result = asker.ShowDialog() ?? false;
            return result;
        }
    }

    /// <summary>
    /// Representing additional options for this dialog
    /// </summary>
    public class AskerOptions
    {
        /// <summary>
        /// Sets the title of the dialog window
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Whether or not the cancel button is visible
        /// </summary>
        public bool CanCancel { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AskerOptions"/> instance
        /// </summary>
        /// <param name="title">Sets the title of the dialog window</param>
        /// <param name="canCancel">Whether or not the cancel button is visible</param>
        public AskerOptions(string title = "Input needed", bool canCancel = false)
        {
            Title = title;
            CanCancel = canCancel;
        }
    }
}
