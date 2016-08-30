using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Commands
{
    public interface ICommandService
    {
        /// <summary>
        /// Action to perform for the given command
        /// </summary>
        /// <param name="action">Delegate to get executed</param>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        /// <returns>Fluent command object</returns>
        ICommandTree Execute(Action action, [CallerFilePath]string className = "", [CallerMemberName]string methodName = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Action to perform for the given command and adds action to stack to enable undo capabilities
        /// </summary>
        /// <param name="action">Delegate to get executed</param>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        /// <returns>Fluent command object</returns>
        ICommandTreeToStack ExecuteToStack(Action action, [CallerFilePath]string className = "", [CallerMemberName]string methodName = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Undos previous ICommand that was executed
        /// </summary>
        void Undo();

        /// <summary>
        /// Redos ICommand that was undoed
        /// </summary>
        void Redo();

        /// <summary>
        /// Gets the actions that can be intercepted before the command is executed
        /// </summary>
        ICommandListener Listener { get; }
    }
}
