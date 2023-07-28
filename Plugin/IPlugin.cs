using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// Represents an interface for a plugin
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// The colour of the plugin, generally meant to be the foreground of any UI linked to this plugin instance
        /// </summary>
        public Color Colour { get; }

        /// <summary>
        /// A string that appears as the plugin's tooltip
        /// </summary>
        public object Tooltip { get; }

        /// <summary>
        /// Method to call after this plugin is initialised.
        /// </summary>
        /// <remarks>
        /// Constructors should not be used, hence this method is provided for overriding.
        /// </remarks>
        /// <param name="e">The <see cref="Env"/> to run in</param>
        public void Setup(Env e);
    }
}
