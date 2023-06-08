using System;
using System.Windows;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Messager : Window
    {
        #region Content and Style Properties

        /// <summary>
        /// gets or sets the message shown in this dialog
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the style for the text display
        /// </summary>
        public ConsoleStyle MessageStyle { get; set; } = ConsoleStyle.Unset;

        #endregion

        #region Buttons Properties

        /// <summary>
        /// Gets or sets whether the cancel button is shown
        /// </summary>
        public bool IsCancelButtonVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the no button is visible
        /// </summary>
        public bool IsNoButtonVisible { get; set; }

        /// <summary>
        /// Gets or sets the text for the 'Ok' or 'Yes' button
        /// </summary>
        public string YesButtonText { get; set; } = "";

        /// <summary>
        /// Gets or sets the text for the 'no' button
        /// </summary>
        public string NoButtonText { get; set; } = "";

        #endregion

        #region Output Properties

        /// <summary>
        /// Gets whether the 'Yes' button was pressed
        /// </summary>
        public DialogResult Result { get; private set; }

        #endregion

        /// <summary>
        /// Creates a new message dialog
        /// </summary>
        public Messager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises the dialog, and loads all the controls
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            Output.Text = Text;

            switch (MessageStyle)
            {
                case ConsoleStyle.H1BlockStyle:
                    Output.FontWeight = FontWeights.Bold;
                    break;
                case ConsoleStyle.NewBlockStyle:
                    Output.Background = (System.Windows.Media.SolidColorBrush)Resources["MetaPanelColourKey"];
                    break;
                case ConsoleStyle.FormatBlockStyle:
                    Output.Foreground = (System.Windows.Media.SolidColorBrush)Resources["FormatColourKey"];
                    break;
                case ConsoleStyle.WarningBlockStyle:
                    Output.Foreground = (System.Windows.Media.SolidColorBrush)Resources["WarningColourKey"];
                    break;
                case ConsoleStyle.ErrorBlockStyle:
                    Output.Background = (System.Windows.Media.SolidColorBrush)Resources["FormatColourKey"];
                    break;
            }

            if (IsCancelButtonVisible) CancelButton.Visibility = Visibility.Visible;

            if (IsNoButtonVisible || NoButtonText != string.Empty)
            {
                NoButton.Visibility = Visibility.Visible;
                YesButton.Content = "Yes";
                if (NoButtonText != string.Empty) NoButton.Content = NoButtonText;
            }

            if (YesButtonText != string.Empty) YesButton.Content = YesButtonText;

            base.OnActivated(e);
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.Yes;
            DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.No;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.Cancel;
            DialogResult = false;
        }

        public static DialogResult Out(
            string text,
            string title = "Message",
            ConsoleStyle messageStyle = ConsoleStyle.Unset,
            bool isCancelButtonVisible = false,
            bool isNoButtonVisible = false,
            string yesButtonText = "",
            string noButtonText = ""
            )
        {
            Messager messager = new()
            {
                Title = title,
                Text = text,
                MessageStyle = messageStyle,
                IsCancelButtonVisible = isCancelButtonVisible,
                IsNoButtonVisible = isNoButtonVisible,
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText
            };
            messager.ShowDialog();
            return messager.Result;
        }

        public static DialogResult OutException(
            Exception e,
            string context = "",
            bool isCancelButtonVisible = false,
            bool isNoButtonVisible = false,
            string yesButtonText = "",
            string noButtonText = "")
        {
            return Out(
                e.Message,"Exception",
                ConsoleStyle.ErrorBlockStyle,
                isCancelButtonVisible,
                isNoButtonVisible,
                yesButtonText,
                noButtonText
                );
        }
    }
}
