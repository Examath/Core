using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Examath.Core.Controls
{
    public class FilePicker : Control
    {
        #region UI

        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";

        internal TextBox? _TextBox;
        private Button? _Button;

        #endregion

        #region Properties

        #region FileName

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register(
                "FileName",
                typeof(string),
                typeof(FilePicker),
                new FrameworkPropertyMetadata(
                    String.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTextChanged),
                    null,
                    true,
                    UpdateSourceTrigger.PropertyChanged
                    )
                );

        public static readonly RoutedEvent FileNameChangedEvent =
            EventManager.RegisterRoutedEvent(
                "FileNameChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>),
                typeof(FilePicker));

        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add { base.AddHandler(FileNameChangedEvent, value); }
            remove { base.RemoveHandler(FileNameChangedEvent, value); }
        }

        protected virtual void OnFileNameChanged(string oldFileName, string newFileName)
        {
            SetValue(IsAbsoluteFileNameProeprtyKey, Path.IsPathFullyQualified(newFileName));
            RoutedPropertyChangedEventArgs<string> e = new(oldFileName, newFileName)
            {
                RoutedEvent = FileNameChangedEvent
            };
            base.RaiseEvent(e);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FilePicker filePicker = (FilePicker)d;
            filePicker.OnFileNameChanged((string)e.OldValue, (string)e.NewValue);
        }

        #endregion

        public string ExtensionFilter
        {
            get { return (string)GetValue(ExtensionFilterProperty); }
            set { SetValue(ExtensionFilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExtensionFilter.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty ExtensionFilterProperty =
            DependencyProperty.Register("ExtensionFilter", typeof(string), typeof(FilePicker), new PropertyMetadata("All files (*.*)|*.*"));

        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Directory.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("Directory", typeof(string), typeof(FilePicker), new PropertyMetadata(""));

        #region IsAbsoluteFileName

        public static readonly DependencyPropertyKey IsAbsoluteFileNameProeprtyKey =
            DependencyProperty.RegisterReadOnly(name: "IsAbsoluteFileName", propertyType: typeof(bool), ownerType: typeof(FilePicker), typeMetadata: new PropertyMetadata(false));

        public bool IsAbsoluteFileName
        {
            get => (bool) GetValue(IsAbsoluteFileNameProeprtyKey.DependencyProperty);
            set => SetValue(IsAbsoluteFileNameProeprtyKey, value);
        }
        #endregion

        #endregion

        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        #region Methods

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
            // Configure open file dialog box
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = (string)GetValue(ExtensionFilterProperty), // Filter files by extension
                InitialDirectory = (string)GetValue(DirectoryProperty),
            };

            if (GetValue(DirectoryProperty) is string directory1 && Path.Exists(directory1))
            {
                fileDialog.InitialDirectory = directory1;
            }

            if (File.Exists((string)GetValue(FileNameProperty)))
            {
                fileDialog.FileName = Path.GetFileNameWithoutExtension((string)GetValue(FileNameProperty));
                fileDialog.DefaultExt = Path.GetExtension((string)GetValue(FileNameProperty));
            }

            if (Tag is string tag)
            {
                fileDialog.Title = $"Select {tag}";
            }

            // Show open file dialog box
            bool? result = fileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                string directory = (string)GetValue(DirectoryProperty);
                if (directory != string.Empty && fileDialog.FileName.StartsWith(directory))
                {
                    SetValue(FileNameProperty, fileDialog.FileName[directory.Length..]);
                }
                else
                {
                    SetValue(FileNameProperty, fileDialog.FileName);
                }
            }
        }

        #endregion
    }
}
