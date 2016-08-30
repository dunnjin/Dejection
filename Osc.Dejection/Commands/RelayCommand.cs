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
    public class RelayCommand : ICommand, ICommandTree, ICommandTreeUnExecuteStart, ICommandTreeCanExecuteStart, ICommandTreeUnExecuteEnd, ICommandTreeOnExceptionEnd, ICommandTreeCanExecuteEnd, ICommandTreeToStack
    {
        #region Fields

        private Action execute;
        private Action unExecute;

        private Func<bool> canExecute;

        private Dictionary<Type, Action<Exception>> exceptions = new Dictionary<Type, Action<Exception>>();      

        #endregion

        #region Properties

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Gets or sets the caller member information given by compiler services
        /// </summary>
        public ICommandData CommandData { get; set; }

        /// <summary>
        /// Gets or sets the actions that can be intercepted before the command is executed
        /// </summary>
        public ICommandListener Listener { get; set; } = new CommandListener();

        #endregion

        /// <summary>
        /// Determines whether ICommand execute can be performed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (canExecute.IsNull())
                return true;

            if (!Listener.CanExecute.IsNull())
                Listener.CanExecute(canExecute, CommandData);

            return canExecute();
        }

        /// <summary>
        /// The action to be performed
        /// </summary>
        /// <param name="parameter">null in command service</param>
        public void Execute(object parameter)
        {
            try
            {
                if (!Listener.Execute.IsNull())
                    Listener.Execute(CommandData);

                execute();
            }
            catch (Exception exception)
            {
                if (!Listener.OnException.IsNull())
                    Listener.OnException(exception, CommandData);

                Type exceptionType = exception.GetType();
                
                if (!exceptions.ContainsKey(exceptionType))
                    return;

                Action<Exception> commandException = exceptions[exceptionType];
                commandException(exception);
            }
        }

        /// <summary>
        /// Returns ICommand for WPF 
        /// </summary>
        /// <returns></returns>
        public ICommand Relay()
        {
            return this;
        }

        public void Run()
        {
            Execute(new object());            
        }

        /// <summary>
        /// Executes the UnExecute action which should be the inverse of the Execute action
        /// </summary>
        /// <exception cref="ArgumentNullException">Inverse action cannot be null</exception>
        public void Undo()
        {
            unExecute.ThrowIfNull(nameof(unExecute));

            try
            {
                unExecute();
            }
            catch (Exception exception)
            {
                Type exceptionType = exception.GetType();
                
                if (!exceptions.ContainsKey(exceptionType))
                    return;

                Action<Exception> commandException = exceptions[exceptionType];
                commandException(exception);
            }

        }

        /// <summary>
        /// Executes the Execute action that was undoed
        /// </summary>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        public void Redo()
        {
            execute.ThrowIfNull(nameof(execute));

            try
            {
                execute();
            }
            catch (Exception exception)
            {
                Type exceptionType = exception.GetType();

                if (!exceptions.ContainsKey(exceptionType))
                    return;

                Action<Exception> commandException = exceptions[exceptionType];
                commandException(exception);
            }
        }
  
        public ICommandTreeUnExecuteStart UnExecute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            unExecute = action;

            return this;
        }

        public ICommandTree OnException<TException>(Action<Exception> action) where TException : Exception
        {
            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        public ICommandTreeCanExecuteStart CanExecute(Func<bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            canExecute = predicate;

            return this;
        }

        ICommandTreeUnExecuteStart ICommandTreeUnExecuteStart.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeCanExecuteEnd ICommandTreeUnExecuteStart.CanExecute(Func<bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            canExecute = predicate;

            return this;
        }

        ICommandTreeCanExecuteStart ICommandTreeCanExecuteStart.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeCanExecuteEnd ICommandTreeCanExecuteStart.UnExecute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            unExecute = action;

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeUnExecuteEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeOnExceptionEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeCanExecuteEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            exceptions.Add(typeof(TException), action);

            return this;
        }

        public ICommandTree Execute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            execute = action;

            return this;
        }

        public ICommandTreeToStack ExecuteToStack(Action action)
        {
            action.ThrowIfNull(nameof(action));

            execute = action;

            return this;
        }
    }
}
