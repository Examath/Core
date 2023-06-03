using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Documents;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents an environment in which code and users can interact.
    /// </summary>
    /// <remarks>
    /// This includes:
    /// <list type="bullet">
    ///     <item>A console <see cref="Output"/>, a rich text document</item>
    ///     <item>A command to <see cref="Clear"/> the console</item>
    ///     <item><see cref="object"/> <see cref="Model"/> for data</item>
    ///     <item>
    ///         Input methods <see cref="Ask(AskerOptions?, IAskerBlock[])"/> and
    ///         <see cref="In(string)"/>
    ///     </item>
    ///     <item>
    ///         Output methods <see cref="OutBlock(Block, ConsoleStyle, string)"/>,
    ///         <see cref="Out(object, ConsoleStyle)"/>,
    ///         <see cref="OutputBase.OutException(Exception, string)"/> and
    ///         <see cref="StartLog(string)"/>
    ///     </item>
    /// </list>
    /// Example usage:
    /// <code>
    /// XAML FlowDocumentScrollViewer                
    ///     x:Name="OutputContainer"                
    ///     Foreground="{StaticResource ForegroundColourKey}"                
    ///     Grid.Row="4"                
    ///     Margin="2"                
    ///     VerticalScrollBarVisibility="Auto" Grid.Column="2"                
    ///     DataContext="{Binding Env}"                
    ///     Document="{Binding Output}"
    /// C#:
    ///     private Env _Env = new();
    ///     ...        
    ///         Env.Default = Env;
    ///         Env.Model = _XModel;
    /// </code>
    /// </remarks>
    public partial class Env : OutputBase
    {

        #region Init

        /// <summary>
        /// Creates a new <see cref="Env"/> set to the default console style resource.
        /// </summary>
        public Env()
        {
            Output.Resources.Source = new Uri("/Examath.Core;component/Parts/Console.xaml", UriKind.Relative);
        }

        /// <summary>
        /// Static variable that may be used to access an <see cref="Env"/> indirectly.
        /// </summary>
        /// <remarks>
        /// This must be set to a new instance at application start before using.
        /// </remarks>
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

        #region Input

        /// <summary>
        /// Generic object for storing any kind of data
        /// </summary>
        public object? Model { get; set; }

        /// <summary>
        /// Use <see cref="Asker.Show(AskerOptions?, IAskerBlock[])"/> instead.
        /// </summary>
        /// <param name="askerOptions">The <see cref="AskerOptions"/> for the dialog. Use null for default.</param>
        /// <param name="askerBlocks"><see cref="IAskerBlock"/>s to display in this dialog</param>
        /// <returns>The result of the dialog. True if true and false if (false or null).</returns>
        public bool Ask(AskerOptions? askerOptions, params IAskerBlock[] askerBlocks)
#pragma warning restore CA1822 // Mark members as static
        {
            Asker asker = new(askerOptions, askerBlocks);
            bool result = asker.ShowDialog() ?? false;
            return result;
        }

        /// <summary>
        /// Creates and shows a new <see cref="Asker"/> dialog
        /// with a single <see cref="StringQ"/> (<see cref="string"/>, <see cref="TextBoxInput"/>) question
        /// </summary>
        /// <param name="question">Title of dialog</param>
        /// <returns>A string that may contain input from the user. This may be blank.</returns>
        public string In(string question = "Enter input")
        {
            StringQ stringQ = new();
            Asker.Show(new(title: question), new[] { stringQ });
            return stringQ.Value;
        }

        #endregion

        #region Output

        /// <summary>
        /// Outputs a <see cref="Block"/> to this enviorment console. For custom-formatted messages
        /// </summary>
        /// <remarks>
        /// Use <see cref="Out(object, ConsoleStyle)"/> if formatting is not needed.
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

        public override void Out(object message, ConsoleStyle type = ConsoleStyle.Unset)
        {
            Section section = new(new Paragraph(new Run(message.ToString())));
            OutBlock(section, type);
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

        #endregion
    }
}
