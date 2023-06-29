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
        public string Tooltip { get; }

        /// <summary>
        /// Method to call after this plugin is initialised.
        /// </summary>
        /// <remarks>
        /// Constructors should not be used, hence this method is provided for overriding.
        /// </remarks>
        /// <param name="e">The <see cref="Env"/> to run in</param>
        public void Setup(Env e);
    }

    /// <summary>
    /// Basic implementation of <see cref="IPlugin"/>
    /// </summary>
    public abstract class Plugin : IPlugin
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual Color Colour { get; set; } = Color.FromRgb(0, 255, 0);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Tooltip { get; set; } = "";

        /// <inheritdoc/>
        public virtual void Setup(Env e)
        {

        }
    }
}
