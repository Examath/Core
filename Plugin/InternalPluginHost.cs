using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// Hosts an <see cref="IPlugin"/> defined at compile-time
    /// </summary>
    public class InternalPluginHost : PluginHost
    {
        /// <summary>
        /// Creates a new <see cref="PluginHost"/> to host a single plugin
        /// </summary>
        /// <param name="env">The <see cref="Env"/> context the <see cref="IPlugin"/> will run in</param>
        /// <param name="type">The class of this internal plugin. This class should implement <see cref="IPlugin"/></param>
        /// <remarks>
        /// This does not actually load the plugin. <seealso cref="Load"/>
        /// </remarks>
        public InternalPluginHost(Env env, Type type) : base(env)
        {
            _Type = type;
        }

        /// <summary>
        /// <inheritdoc/> (in this case, internal)
        /// </summary>
        public override string FileName => "Internal";

        /// <summary>
        /// Initialises and loads the actual <see cref="Plugin"/> from provided type
        /// </summary>
        public override void Load()
        {
            if(_Type != null) CreateInstanceOfPluginType();
        }
    }
}
