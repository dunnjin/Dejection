using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Commands
{
    public interface ICommandTreeToStack
    {
        ICommandTreeUnExecuteStart UnExecute(Action action);
    }

    public interface ICommandTree
    {
        ICommandTree OnException<TSource>(Action<Exception> action)
            where TSource : Exception;

        ICommandTreeCanExecuteStart CanExecute(Func<bool> predicate);   

        ICommand Relay();

        void Run();

        void Undo();
        void Redo();
    }

    public interface ICommandTreeUnExecuteStart
    {
        ICommandTreeUnExecuteStart OnException<TSource>(Action<Exception> action)
            where TSource : Exception;

        ICommandTreeCanExecuteEnd CanExecute(Func<bool> predicate);

        ICommand Relay();

        void Run();
    }

    public interface ICommandTreeCanExecuteStart
    {
        ICommandTreeCanExecuteStart OnException<TSource>(Action<Exception> action)
            where TSource : Exception;

        ICommandTreeCanExecuteEnd UnExecute(Action action);

        ICommand Relay();

        void Run();

    }
    public interface ICommandTreeUnExecuteEnd
    {
        ICommandTreeOnExceptionEnd OnException<TSource>(Action<Exception> action)
          where TSource : Exception;

        ICommand Relay();

        void Run();
    }

    public interface ICommandTreeOnExceptionEnd
    {
        ICommandTreeOnExceptionEnd OnException<TSource>(Action<Exception> action)
          where TSource : Exception;

        ICommand Relay();

        void Run();
    }

    public interface ICommandTreeCanExecuteEnd
    {
        ICommandTreeOnExceptionEnd OnException<TSource>(Action<Exception> action)
          where TSource : Exception;

        ICommand Relay();

        void Run();
    }

    /// <summary>
    /// Contains caller member information given by compiler services
    /// </summary>
    public class CommandData
    {
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public int LineNumber { get; set; }
    }

    /// <summary>
    /// Contains caller member information given by compiler services
    /// </summary>
    public interface ICommandListener
    {
        Action<CommandData> Execute { get; set; }
        Action<Func<bool>, CommandData> CanExecute { get; set; }
        Action<Exception, CommandData> OnException { get; set; }
    }

    /// <summary>
    /// Contains a somewhat intercepter as the listener methods will be called before parent
    /// </summary>
    public class CommandListener : ICommandListener
    {
        public Action<CommandData> Execute { get; set; }
        public Action<Func<bool>, CommandData> CanExecute { get; set; }
        public Action<Exception, CommandData> OnException { get; set; }
    }
}
