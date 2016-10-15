using Osc.Dejection.Commands;
using Osc.Dejection.Framework;
using Osc.Dejection.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Sample.ViewModels
{
    public class Test3ViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly INavigationService _navigationService;

        public ICommand NavigateTest1Command
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<Test3ViewModel, Test1ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand NavigateTest2Command
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<Test3ViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand ThrowExceptionCommand
        {
            get
            {
                return _commandService                                        
                    .Execute(() =>
                    {
                        throw new ArgumentException("Exception was thrown");
                    })
                    .OnException<ArgumentException>(obj =>
                    {
                        // ArgumentException will be routed here
                    })
                    .OnException<Exception>(obj =>
                    {
                        // Acts like Catch inside the Try/Catch
                    })
                    .Relay();
            }
        }

        public Test3ViewModel(ICommandService commandService,
            INavigationService navigationService)
        {
            _commandService = commandService;
            _navigationService = navigationService;
        }
    }
}
