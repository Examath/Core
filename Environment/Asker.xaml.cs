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
    }

    public class AskerOptions
    {
        public string Title { get; private set; }
        public bool CanCancel { get; private set; }

        public AskerOptions(string title = "Input needed", bool canCancel = false)
        {
            Title = title;
            CanCancel = canCancel;
        }
    }
}
