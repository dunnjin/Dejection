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

        private Action _execute;
        private Action _unExecute;

        private Func<bool> _canExecute;

        private Dictionary<Type, Action<Exception>> _exceptions = new Dictionary<Type, Action<Exception>>();      

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
        public CommandData CommandData { get; set; }

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
            if (_canExecute.IsNull())
                return true;

            if (!Listener.CanExecute.IsNull())
                Listener.CanExecute(_canExecute, CommandData);

            return _canExecute();
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

                _execute();
            }
            catch (Exception exception)
            {
                if (!Listener.OnException.IsNull())
                    Listener.OnException(exception, CommandData);

                var exceptionType = exception.GetType();
                
                if (!_exceptions.ContainsKey(exceptionType))
                    return;

                var commandException = _exceptions[exceptionType];
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
            _unExecute.ThrowIfNull(nameof(_unExecute));

            try
            {
                _unExecute();
            }
            catch (Exception exception)
            {
                var exceptionType = exception.GetType();
                
                if (!_exceptions.ContainsKey(exceptionType))
                    return;

                var commandException = _exceptions[exceptionType];
                commandException(exception);
            }

        }

        /// <summary>
        /// Executes the Execute action that was undoed
        /// </summary>
        /// <exception cref="ArgumentNullException">Action cannot be null</exception>
        public void Redo()
        {
            _execute.ThrowIfNull(nameof(_execute));

            try
            {
                _execute();
            }
            catch (Exception exception)
            {
                var exceptionType = exception.GetType();

                if (!_exceptions.ContainsKey(exceptionType))
                    return;

                var commandException = _exceptions[exceptionType];
                commandException(exception);
            }
        }
  
        public ICommandTreeUnExecuteStart UnExecute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            _unExecute = action;

            return this;
        }

        public ICommandTree OnException<TException>(Action<Exception> action) where TException : Exception
        {
            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        public ICommandTreeCanExecuteStart CanExecute(Func<bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            _canExecute = predicate;

            return this;
        }

        ICommandTreeUnExecuteStart ICommandTreeUnExecuteStart.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeCanExecuteEnd ICommandTreeUnExecuteStart.CanExecute(Func<bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            _canExecute = predicate;

            return this;
        }

        ICommandTreeCanExecuteStart ICommandTreeCanExecuteStart.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeCanExecuteEnd ICommandTreeCanExecuteStart.UnExecute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            _unExecute = action;

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeUnExecuteEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeOnExceptionEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        ICommandTreeOnExceptionEnd ICommandTreeCanExecuteEnd.OnException<TException>(Action<Exception> action)
        {
            action.ThrowIfNull(nameof(action));

            if (_exceptions.ContainsKey(typeof(TException)))
                throw new ArgumentException("Exception already expected");

            _exceptions.Add(typeof(TException), action);

            return this;
        }

        public ICommandTree Execute(Action action)
        {
            action.ThrowIfNull(nameof(action));

            _execute = action;

            return this;
        }

        public ICommandTreeToStack ExecuteToStack(Action action)
        {
            action.ThrowIfNull(nameof(action));

            _execute = action;

            return this;
        }
    }
}
