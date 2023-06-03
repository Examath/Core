using System.Windows;
using System.Windows.Forms;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Messager : Window
    {

        /// <summary>
        /// Gets whether the 'Yes' button was pressed
        /// </summary>
        public bool IsYes { get; private set; }

        /// <summary>
        /// Gets or sets whether the cancel button is shown
        /// </summary>
        public bool IsCancelButtonVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the no button is visible
        /// </summary>
        public bool IsNoButtonVisible { get; set; }

        private string _YesButtonText = "Unset";
        /// <summary>
        /// Gets or sets the text for the 'Ok' or 'Yes' button
        /// </summary>
        public string YesButtonText
        {
            get
            {
                if (_YesButtonText != "Unset") return _YesButtonText;
                else
                {
                    if (IsNoButtonVisible) return "Yes";
                    else return "Ok";
                }
            }
            set => _YesButtonText = value;
        }

        /// <summary>
        /// Gets or sets the text for the 'no' button
        /// </summary>
        public string NoButtonText { get; set; } = "No";

        /// <summary>
        /// gets or sets the message shown in this dialog
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Creates a new message dialog
        /// </summary>
        public Messager()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = true;
            DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public static bool Show(string title, string text, bool isNoButtonVisible = false)
        {
            Messager messager = new()
            {
                Title = title,
                Text = text,
                IsNoButtonVisible = isNoButtonVisible,
            };
            bool result = messager.ShowDialog() ?? false;
            return result;
        }
    }
}
