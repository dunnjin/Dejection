using Microsoft.Practices.Unity;
using Osc.Dejection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Commands
{
    public class CommandService : ICommandService
    {
        #region Fields

        private readonly Stack<ICommandTree> undo = new Stack<ICommandTree>();
        private readonly Stack<ICommandTree> redo = new Stack<ICommandTree>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the actions that can be intercepted before the command is executed
        /// </summary>
        public ICommandListener Listener { get; } = new CommandListener();

        #endregion

        #region Methods

        /// <summary>
        /// Action to perform for the given command
        /// </summary>
        /// <param name="action">Delegate to get executed</param>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        /// <returns>Fluent command object</returns>
        public ICommandTree Execute(Action action, [CallerFilePath] string className = "",
            [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            RelayCommand command = new RelayCommand()
            {
                Listener = Listener,
                CommandData = new CommandData()
                {
                    ClassName = className,
                    MethodName = methodName,
                    LineNumber = lineNumber,
                },
            };

            return command.Execute(action);
        }


        /// <summary>
        /// Action to perform for the given command and adds action to stack to enable undo capabilities
        /// </summary>
        /// <param name="action">Delegate to get executed</param>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        /// <returns>Fluent command object</returns>
        public ICommandTreeToStack ExecuteToStack(Action action, [CallerFilePath] string className = "",
            [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            RelayCommand command = new RelayCommand()
            {
                Listener = Listener,
                CommandData = new CommandData()
                {
                    ClassName = className,
                    MethodName = methodName,
                    LineNumber = lineNumber,
                },
            };

            undo.Push(command);

            redo.Clear();

            return command.ExecuteToStack(action);
        }

        /// <summary>
        /// Undos previous ICommand that was executed
        /// </summary>
        public void Undo()
        {
            if (undo.Count.LessThanOrEqual(0))
                return;

            ICommandTree command = undo.Pop();

            if (command.IsNull())
                return;

            command.Undo();

            redo.Push(command);
        }

        /// <summary>
        /// Redos ICommand that was undoed
        /// </summary>
        public void Redo()
        {
            if (redo.Count.LessThanOrEqual(0))
                return;

            ICommandTree command = redo.Pop();

            if (command.IsNull())
                return;

            command.Redo();

            undo.Push(command);
        }

        #endregion

    }
}
