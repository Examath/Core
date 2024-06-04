using System;
using System.Windows;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents a simple dialog box, with optional yes/no/cancel.
    /// You may Messager.Out to show the dialog.
    /// </summary>
    public partial class Messager : Window
    {
        #region Content and Style Properties

        /// <summary>
        /// Gets or sets the message shown in this dialog
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
        public DialogResult Result { get; private set; } = System.Windows.Forms.DialogResult.Cancel;

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
                    Output.Background = (System.Windows.Media.SolidColorBrush)Resources["ErrorColourKey"];
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

        /// <summary>
        /// Creates a new <see cref="Messager"/> dialog and shows it to the user.
        /// </summary>
        /// <remarks>
        /// Note that this returns <see cref="DialogResult.Yes"/> if either the Ok or Yes button is clicked.
        /// </remarks>
        /// <param name="text">The content of the dialog</param>
        /// <param name="title">The title of the dialog window</param>
        /// <param name="messageStyle">The style of the message</param>
        /// <param name="isCancelButtonVisible">When true the cancel button is visible</param>
        /// <param name="isNoButtonVisible">When true the no button is visible</param>
        /// <param name="yesButtonText">Overrides the yes button text. By default this is 'Ok' or 'Yes' if the no button is visible</param>
        /// <param name="noButtonText">Sets the text of the no button, and ensures it is visible</param>
        /// <returns><see cref="DialogResult.Yes"/> if the Ok or Yes button is clicked, 
        /// <see cref="DialogResult.No"/> if the No button is clicked and 
        /// <see cref="DialogResult.Cancel"/> if the cancel button is clicked or the dialog is closed</returns>
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
