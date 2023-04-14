using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Examath.Core.Utils
{
    /// <summary>
    /// Represents a class that supports the performance timing functionality of <see cref="Stopwatch"/>,
    /// and outputs to a provided <see cref="Section"/>.
    /// </summary>
    public class TimeLogger
    {
        private Section _Output;

        private string _Label = String.Empty;

        private Run _SubOutput = new();

        private Stopwatch _Stopwatch = new();

        public TimeLogger(Section output)
        {
            _Output = output;
        }

        /// <summary>
        /// Starts the stopwatch, then creates a new paragraph inside the output <see cref="Section"/> to display the progress.
        /// </summary>
        /// <remarks>
        /// If an interval is already running, <see cref="End"/> is automatically called.
        /// </remarks>
        /// <param name="label">The label used for this interval, as displayed</param>
        public void Start(string label)
        {
            if (_Stopwatch.IsRunning)
            {
                End();
            }
            _Stopwatch.Start();
            _Label = label;
            _SubOutput = new($"{_Label} ...");
            _Output.Blocks.Add(new Paragraph(_SubOutput));
        }

        /// <summary>
        /// Stops the stopwatch and updates the label for this interval.
        /// </summary>
        public void End()
        {
            _SubOutput.Text = $"{_Label}\t{_Stopwatch.ElapsedMilliseconds,6} ms";
            _Stopwatch.Reset();
        }
    }
}
