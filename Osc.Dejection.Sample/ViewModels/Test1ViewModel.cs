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
    public class Test1ViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public ICommand ShowDialogCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<ApplicationViewModel, Test2ViewModel>();

                        // Create a new dialog window with the specified ViewModel 
                        // The dialog settings are based off the View settings being displayed
                        _dialogService.ShowDialog<Test4ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test1ViewModel(ICommandService commandService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _commandService = commandService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }
    }
}
