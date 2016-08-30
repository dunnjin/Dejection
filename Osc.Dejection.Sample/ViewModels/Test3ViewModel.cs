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
        private readonly ICommandService commandService;
        private readonly INavigationService navigationService;

        public ICommand NavigateTest1Command
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<Test3ViewModel, Test1ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand NavigateTest2Command
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<Test3ViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand ThrowExceptionCommand
        {
            get
            {
                return commandService                                        
                    .Execute(() =>
                    {
                        throw new Exception("Exception was thrown");
                    })
                    .OnException<Exception>(obj =>
                    {
                        // Log something maybe... I'm not the boss of your life
                    })
                    .Relay();
            }
        }

        public Test3ViewModel(ICommandService commandService, INavigationService navigationService)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
        }
    }
}
