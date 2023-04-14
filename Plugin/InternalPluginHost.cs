using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Plugin
{
    public class InternalPluginHost : PluginHost
    {
        private Type _Type;

        public InternalPluginHost(Env env, Type type) : base(env)
        {
            _Type = type;
        }

        public override string FileName => "Internal";

        public override void Load()
        {
            InitialisePluginFromType(_Type);
        }
    }
}
