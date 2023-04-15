using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents an environment in which code and users can interact.
    /// </summary>
    /// <remarks>
    /// This includes a console <see cref="Output"/>, a rich text document, and simple methods to display dynamic content on it.
    /// </remarks>
    public partial class Env : OutputBase
    {

        #region Init

        public Env()
        {
            Output.Resources.Source = new Uri("/Examath.Core;component/Parts/Console.xaml", UriKind.Relative);
        }

        public static Env? Default { get; set; }

        #endregion

        #region Console Proeprties

        /// <summary>
        /// The output of this console
        /// </summary>
        public FlowDocument Output { get; set; } = new();

        public int ConsoleEntriesMaxLength { get; set; } = 1000;

        #endregion

        #region Console Methods

        /// <summary>
        /// Clears all blocks in the <see cref="Output"/>
        /// </summary>
        [RelayCommand]
        public void Clear()
        {
            Output.Blocks.Clear();
        }

        #endregion

        #region Output

        /// <summary>
        /// Outputs a <see cref="Block"/> to this enviorment console. For custom-formatted messages
        /// </summary>
        /// <remarks>
        /// Use <see cref="OutputBase.Out(string, ConsoleStyle)"/> if formatting is not needed.
        /// If the block already exists inside <see cref="Output"/>, then it's moved to last.
        /// </remarks>
        /// <param name="block">The section to output</param>
        /// <param name="type">The type of message, similar to Out()</param>
        public override void OutBlock(Block block, ConsoleStyle type = ConsoleStyle.Unset, string tooltip = "")
        {
            // Standard Tooltip
            base.OutBlock(block, type, tooltip);

            // Format Paragraph to required type
            if (type != ConsoleStyle.Unset)
            {
                block.Style = (Style)Output.Resources[type.ToString()];
            }

            // Allow for blocks to be updated and 'pushed' back down
            if (Output.Blocks.Contains(block))
            {
                Output.Blocks.Remove(block);
            }
            else
            {
                // Check if number of entries is too long
                if (Output.Blocks.Count >= ConsoleEntriesMaxLength)
                {
                    Output.Blocks.Remove(Output.Blocks.FirstBlock);
                }
            }

            Output.Blocks.Add(block);

            // Scroll
            Output.Dispatcher.InvokeAsync(() => block.BringIntoView(), System.Windows.Threading.DispatcherPriority.Background);
        }

        /// <summary>
        /// Creates a new <see cref="Log"/> and adds it to this console as a <see cref="Section"/>
        /// </summary>
        /// <param name="title">Optional heading for this <see cref="Log"/></param>
        /// <returns>The new <see cref="Log"/></returns>
        public Log StartLog(string title = "")
        {
            Log log = new(Output, title);
            OutBlock(log.Output);
            return log;
        }

        public override void Out(object message, ConsoleStyle type = ConsoleStyle.Unset)
        {
            Section section = new(new Paragraph(new Run(message.ToString())));
            OutBlock(section, type);
        }

        #endregion

        #region Input

        /// <summary>
        /// Generic object for storing any kind of data
        /// </summary>
        public object? Model { get; set; }

        public string In(string question = "Enter input")
        {
            StringQ stringQ = new();
            Ask(new(title: question), new[] {stringQ});
            return stringQ.Value;
        }

        public bool Ask(AskerOptions? askerOptions, params IAskerBlock[] askerBlocks)
        {
            Asker asker = new(askerOptions, askerBlocks);
            bool result = asker.ShowDialog() ?? false;
            return result;
        }

        #endregion
    }
}
