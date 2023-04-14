using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Examath.Core.Plugin
{
    public interface IPlugin
    {
        public Color Color { get; }

        public void Setup(Env e);
    }

    public abstract class Plugin : IPlugin
    {
        public virtual Color Color { get; set; } = Color.FromRgb(0, 255, 0);

        public virtual void Setup(Env e)
        {

        }
    }
}
