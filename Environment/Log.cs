using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents the method of outputting to a section inside an <see cref="Env"/> console.
    /// </summary>
    public class Log : OutputBase
    {
        #region Init

        public Log(FlowDocument parent, string title = "")
        {
            _Parent = parent;
            if (title != string.Empty)
            {
                Out(title, ConsoleStyle.H1BlockStyle);
            }
        }

        #endregion

        #region Console Proeprties

        /// <summary>
        /// The section containing the displayed output of this class
        /// </summary>
        public Section Output { get; set; } = new();

        private readonly FlowDocument _Parent;

        #endregion

        #region OutBlock

        /// <summary>
        /// Outputs a Paragraph to this section. For custom-formatted messages
        /// </summary>
        /// <remarks>
        /// Use <see cref="OutputBase.Out(string, ConsoleStyle)"/> if formatting is not needed.
        /// </remarks>
        /// <param name="block">The paragraph to output</param>
        /// <param name="type">The type of message, similar to Out()</param>
        public override void OutBlock(Block block, ConsoleStyle type = ConsoleStyle.Unset, string tooltip = "")
        {
            // Standard Tooltip
            base.OutBlock(block, type, tooltip);

            // Format Paragraph to required type
            if (type != ConsoleStyle.Unset)
            {
                block.Style = (Style)_Parent.Resources[type.ToString()];
            }

            Output.Blocks.Add(block);
            Output.Dispatcher.InvokeAsync(() => block.BringIntoView(), System.Windows.Threading.DispatcherPriority.Background);
        }

        public override void Out(object message, ConsoleStyle type = ConsoleStyle.Unset)
        {
            Paragraph paragraph = new(new Run(message.ToString()));
            OutBlock(paragraph, type);
        }

        #endregion

        #region Time Logger

        private string _Label = String.Empty;

        private Run _SubOutput = new();

        private readonly Stopwatch _Stopwatch = new();

        /// <summary>
        /// Starts the stopwatch, then creates a new paragraph inside the output <see cref="Section"/> to display the progress.
        /// </summary>
        /// <remarks>
        /// If an interval is already running, <see cref="EndTiming"/> is automatically called.
        /// </remarks>
        /// <param name="label">The label used for this interval, as displayed</param>
        public void StartTiming(string label)
        {
            if (_Stopwatch.IsRunning)
            {
                EndTiming();
            }
            _Stopwatch.Start();
            _Label = label;
            _SubOutput = new($"{_Label} ...");
            OutBlock(new Paragraph(_SubOutput), ConsoleStyle.TimeBlockStyle);
        }

        /// <summary>
        /// Stops the stopwatch if it's still running and updates the label for this interval.
        /// </summary>
        /// <param name="postScript">A message appended to the end of the timing label</param>
        public void EndTiming(string postScript = "")
        {
            if (_Stopwatch.IsRunning)
            {
                _SubOutput.Text = $"{_Label}\t{_Stopwatch.ElapsedMilliseconds,6} ms";
                _Stopwatch.Reset();
            }
            if (!string.IsNullOrWhiteSpace(postScript)) _SubOutput.Text += $"\t{postScript}";
        }

        #endregion
    }
}
