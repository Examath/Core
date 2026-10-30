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
using System.Windows.Shapes;

namespace Examath.Core.Environment
{
    /// <summary>
    /// A window that displays several progress bars
    /// </summary>
    public partial class ProgressWindow : Window
    {
        /// <summary>
        /// Creates a new window with the specified <see cref="ProgressWindowTask"/>
        /// </summary>
        public ProgressWindow(params ProgressWindowTask[] progressWindowTasks)
        {
            InitializeComponent();
            DataContext = progressWindowTasks;
        }
    }

    /// <summary>
    /// Represents the vale of a progress bar in the <see cref="ProgressWindow"/>
    /// </summary>
    public class ProgressWindowTask : ObservableObject, IProgress<double>
    {
        /// <summary>
        /// Gets the header of the progress bar
        /// </summary>
        public string Header { get; private set; }

        private double _Maximum;
        /// <summary>
        /// Gets or sets the maximum value of the progress bar
        /// </summary>
        public double Maximum
        {
            get => _Maximum;
            set => SetProperty(ref _Maximum, value);
        }

        private double _Value = 0;
        /// <summary>
        /// Gets or sets the current value of the progress bar
        /// </summary>
        public double Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }

        /// <summary>
        /// Creates a new <see cref="ProgressWindowTask"/> with the specified <paramref name="header"/> and <see cref="Maximum"/> with the value set to zero
        /// </summary>
        /// <param name="header"></param>
        /// <param name="maximum"></param>
        public ProgressWindowTask(string header = "", double maximum = 1.0)
        {
            Header = header;
            Maximum = maximum;
        }

        /// <summary>
        /// Increase value by 1
        /// </summary>
        public void Increment()
        {
            Value++;
        }

        /// <summary>
        /// Reports a value, safely marshaling the property change to the WPF dispatcher.
        /// </summary>
        /// <param name="value">The new progress value.</param>
        public void Report(double value)
        {
            System.Windows.Threading.Dispatcher? dispatcher = Application.Current?.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                Value = value;
                return;
            }

            dispatcher.BeginInvoke(new Action(() => Value = value));
        }
    }
}
