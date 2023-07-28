using Examath.Core.Environment;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Examath.Core.Plugin
{
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
        public virtual object Tooltip { get => ""; }

        /// <inheritdoc/>
        public virtual void Setup(Env e)
        {

        }
    }

    /// <summary>
    /// Basic implementation of <see cref="IPlugin"/> that also implements property changes and validation
    /// </summary>
    public abstract class ObservableValidatorPlugin : ObservableValidator, IPlugin
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual Color Colour { get; set; } = Color.FromRgb(0, 255, 0);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual object Tooltip { get => ""; }

        /// <inheritdoc/>
        public virtual void Setup(Env e)
        {

        }
    }
}