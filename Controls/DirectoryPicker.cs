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
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;

namespace Examath.Core.Controls
{
    /*
    / follow steps 1a or 1b and then 2 to use this custom control in a xaml file.
    /
    / step 1a) using this custom control in a xaml file that exists in the current project.
    / add this xmlnamespace attribute to the root element of the markup file where it is 
    / to be used:
    /
    /     xmlns:mynamespace= "clr-namespace:asowpf.controls"
    /
    /
    / step 1b) using this custom control in a xaml file that exists in a different project.
    / add this xmlnamespace attribute to the root element of the markup file where it is 
    / to be used:
    /
    /     xmlns:mynamespace= "clr-namespace:asowpf.controls;assembly=asowpf.controls"
    /
    / you will also need to add a project reference from the project where the xaml file lives
    / to this project and rebuild to avoid compilation errors:
    /
    /     right click on the target project in the solution explorer and
    /     "add reference"->"projects"->[browse to and select this project]
    /
    /
    / step 2)
    / go ahead and use your control in the xaml file.
    /
    /     <mynamespace:directorypicker/>
    /
    */

    /// <summary>
    /// Represents a control for picking directories
    /// </summary>
    public class DirectoryPicker : Control
    {
        #region UI

        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";

        internal TextBox? _TextBox;
        private Button? _Button;

        #endregion

        #region Directory


        /// <summary>
        /// Gets or sets the location of the selected directory
        /// </summary>
        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        /// <summary>
        /// Backing property for <see cref="Directory"/>
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register(
                "Directory",
                typeof(string),
                typeof(DirectoryPicker),
                new FrameworkPropertyMetadata(
                    String.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnDirectoryChanged),
                    null,
                    true,
                    UpdateSourceTrigger.PropertyChanged
                    )
                );

        /// <summary>
        /// When <see cref="Directory"/> is changed
        /// </summary>
        public static readonly RoutedEvent DirectoryChangedEvent =
            EventManager.RegisterRoutedEvent(
                "DirectoryChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>),
                typeof(DirectoryPicker));

        /// <summary>
        /// triggered when <see cref="Directory"/> changes
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> DirectoryChanged
        {
            add { base.AddHandler(DirectoryChangedEvent, value); }
            remove { base.RemoveHandler(DirectoryChangedEvent, value); }
        }

        /// <summary>
        /// Raises <see cref="DirectoryChangedEvent"/>
        /// </summary>
        protected virtual void OnDirectoryChanged(string oldDirectory, string newDirectory)
        {
            RoutedPropertyChangedEventArgs<string> e = new(oldDirectory, newDirectory)
            {
                RoutedEvent = DirectoryChangedEvent
            };
            base.RaiseEvent(e);
        }

        private static void OnDirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectoryPicker directoryPicker = (DirectoryPicker)d;
            directoryPicker.OnDirectoryChanged((string)e.OldValue, (string)e.NewValue);
        }

        #endregion

        static DirectoryPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryPicker), new FrameworkPropertyMetadata(typeof(DirectoryPicker)));
        }

        #region Methods

        /// <summary>
        /// Finds template parts
        /// </summary>
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

        internal void OnClick(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog vistaFolderBrowserDialog = new();
            if (Tag is string tag)
            {
                vistaFolderBrowserDialog.Description = $"Select {tag}";
            }
            if (vistaFolderBrowserDialog.ShowDialog().GetValueOrDefault())
            {
                SetValue(DirectoryProperty, vistaFolderBrowserDialog.SelectedPath);
            }
        }

        #endregion
    }
}
