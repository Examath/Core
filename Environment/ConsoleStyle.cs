using Examath.Core.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Styling for a block in the <see cref="Env.Output"/> console
    /// </summary>
    public enum ConsoleStyle
    {
        Unset,
        H1BlockStyle,
        NewBlockStyle,
        TimeBlockStyle,
        FormatBlockStyle,
        WarningBlockStyle,
        ErrorBlockStyle
    }
}
