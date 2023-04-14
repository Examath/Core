using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Enviorment
{
    public abstract class AskerOutput
    {
        protected abstract void Initialise(Control control);
    }

    public class MethodOutput : AskerOutput
    {

    }
}
