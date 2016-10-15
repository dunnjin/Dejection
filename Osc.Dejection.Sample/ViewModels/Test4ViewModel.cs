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
   
    public class Test4ViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public ICommand NavigateTest1Command
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<Test4ViewModel, Test1ViewModel>();
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
                        _navigationService.Navigate<Test4ViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand CloseDialogCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        // Will close the topmost dialog based off the ViewModel type specified
                        // If the the ViewModel is not found then no dialog is closed
                        _dialogService.Close<Test4ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test4ViewModel(ICommandService commandService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _commandService = commandService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }
    }
}
