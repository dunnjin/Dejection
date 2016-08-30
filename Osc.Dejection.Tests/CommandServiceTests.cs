using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Windows.Input;
using Osc.Dejection.Commands;
using Microsoft.Practices.Unity;

namespace Osc.Dejection.Tests
{
    public class CommandServiceTests
    {
        [Fact]
        public void CommandTriggersOnExecute()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;
            int expected = 1;

            ICommand command = commandService
                .Execute(() => actual = expected)
                .Relay();

            Assert.Equal(actual, 0);

            command.Execute(null);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void UndoExecutesUnExecuteAction()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;
            int expected = 1;

            ICommand command = commandService
                .ExecuteToStack(() => actual = 2)
                .UnExecute(() => actual = expected)
                .Relay();

            Assert.Equal(actual, 0);

            command.Execute(null);

            Assert.Equal(actual, 2);

            commandService.Undo();

            Assert.Equal(actual, expected);            
        }

        [Fact]
        public void UndoExecutesUnExecuteActionNPlusOne()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;

            commandService.ExecuteToStack(() => actual = 1)
                .UnExecute(() => actual = 0)
                .Relay()
                .Execute(null);

            commandService.ExecuteToStack(() => actual = 2)
                .UnExecute(() => actual = 1)
                .Relay()
                .Execute(null);

            Assert.Equal(actual, 2);

            commandService.Undo();

            Assert.Equal(actual, 1);

            commandService.Undo();

            Assert.Equal(actual, 0);
        }

        [Fact]
        public void RedoInvertsUnExecute()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;

            commandService.ExecuteToStack(() => actual = 1)
                .UnExecute(() => actual = 0)
                .Relay()
                .Execute(null);

            Assert.Equal(actual, 1);

            commandService.Undo();

            Assert.Equal(actual, 0);

            commandService.Redo();

            Assert.Equal(actual, 1);
        }

        [Fact]
        public void RedoInvertsUnExecuteNPlusOne()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;

            commandService.ExecuteToStack(() => actual = 1)
                .UnExecute(() => actual = 0)
                .Relay()
                .Execute(null);

            commandService.ExecuteToStack(() => actual = 2)
             .UnExecute(() => actual = 1)
             .Relay()
             .Execute(null);

            Assert.Equal(actual, 2);

            commandService.Undo();
            commandService.Undo();

            Assert.Equal(actual, 0);

            commandService.Redo();

            Assert.Equal(actual, 1);

            commandService.Redo();

            Assert.Equal(actual, 2);
        }

        [Fact]
        public void OnExceptionIgnoresDifferentTypes()
        {
            ICommandService commandService = new CommandService(); 

            int actual = 0;
            int expected = 1;

            commandService
                .Execute(() =>
                {
                    actual = expected;
                    throw new Exception();
                })
                .OnException<ArgumentException>(obj => actual = 2)
                .Relay()
                .Execute(null);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void OnExceptionCatchesExpected()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;
            int expected = 1;

            commandService
                .Execute(() => { throw new ArgumentException(); })
                .OnException<ArgumentException>(obj => actual = expected)
                .Relay()
                .Execute(null);

            Assert.Equal(actual, expected);
        }        

        [Fact]
        public void UndoWithEmptyUnExecuteGetsSkippedToNext()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;

            commandService
                .ExecuteToStack(() => actual = 1)
                .UnExecute(() => actual = 0)
                .Relay()
                .Execute(null);

            commandService
                .Execute(() => actual = 2)
                .Relay()
                .Execute(null);

            commandService.Undo();

            Assert.Equal(actual, 0);
        }

        [Fact]
        public void UndoWithEmptyUnExecuteGetsSkipped()
        {
            ICommandService commandService = new CommandService();

            int actual = 0;

            commandService
                .Execute(() => actual = 1)
                .Relay()
                .Execute(null);

            commandService.Undo();

            Assert.Equal(actual, 1);
        }
    }
}
