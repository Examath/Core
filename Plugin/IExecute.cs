using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examath.Core.Environment;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// Represents a plugin that can be executed by user command
    /// </summary>
    public interface IExecute
    {
        void Execute(Env e);
    }

    /// <summary>
    /// Represents a plugin that has asynchronous code that can be executed by user command
    /// </summary>
    public interface IExecuteAsync
    {
        Task Execute(Env e);
    }
}
