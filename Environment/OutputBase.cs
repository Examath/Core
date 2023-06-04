using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents a method of displaying messages in a console-like rich text document
    /// </summary>
    public abstract class OutputBase
    {
        #region OutBlock

        /// <summary>
        /// Adds a tooltip containing either a timestamp or <paramref name="tooltip"/> to the <paramref name="block"/>.
        /// This must be oveidden to style and add the block.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Out(string, ConsoleStyle)"/> if formatting is not needed.
        /// </remarks>
        /// <param name="block">The paragraph to output</param>
        /// <param name="type">The type of message, similar to Out()</param>
        public virtual void OutBlock(Block block, ConsoleStyle type = ConsoleStyle.Unset, string tooltip = "")
        {
            // Tooltip / DateStamp
            ToolTip toolTipElement = new();
            toolTipElement.Content = (tooltip != "") ? tooltip : DateTime.Now.ToString();
            block.ToolTip = toolTipElement;
        }

        #endregion

        #region Preformated Outputs

        /// <summary>
        /// Displays a standalone uniformly-formatted message to the console.
        /// </summary>
        /// <remarks>
        /// Use <see cref="OutBlock(Block, ConsoleStyle, string)"/> if custom or dynamic content is needed.
        /// </remarks>
        /// <param name="message">The message to output</param>
        /// <param name="type">The type of message</param>
        public abstract void Out(object message, ConsoleStyle type = ConsoleStyle.Unset);

        /// <summary>
        /// Outputs a ASO standard exception occurred message.
        /// </summary>
        /// <remarks>
        /// Use this to notify the user that an exception has been caught by a try-catch statement.
        /// Outputs the provided <see cref="Exception.Message"/>,
        /// <see cref="Exception.Source"/> and <see cref="Exception.TargetSite"/>.
        /// </remarks>
        /// <param name="e">The exception that was caught</param>
        /// <param name="context">Provide a simplified description of where this error was caught.</param>
        public void OutException(Exception e, string context = "Unknown exception")
        {
            Paragraph Message = new(new Run(DateTime.Now.ToString() + ": "));
            Message.Inlines.Add(new Bold(new Run(context + '\n')));
            Message.Inlines.Add(new Run(e.Message));
            Message.Inlines.Add(new Italic(new Run($"\n@ {e.Source} > {e.TargetSite}")));
            OutBlock(Message, ConsoleStyle.ErrorBlockStyle, e.StackTrace ?? "StackTrace null");
        }

        #endregion
    }
}
