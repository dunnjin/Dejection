using Osc.Dejection.Commands;
using Osc.Dejection.Framework;
using Osc.Dejection.Navigation;
using Osc.Dejection.Sample.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Sample.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public ICommand Test1Command
        {
            get
            {
                // When button is clicked use the fluent command service and navigation service to change ViewModel
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<ApplicationViewModel, Test1ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand Test2Command
        {
            get
            {
                // When button is clicked use the fluent command service and navigation service to change ViewModel
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<ApplicationViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }


        public ICommand Test3Command
        {
            get
            {
                // When button is clicked use the fluent command service and navigation service to change ViewModel
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<ApplicationViewModel, Test3ViewModel>();
                    })
                    .Relay();
            }
        }

        public ApplicationViewModel(ICommandService commandService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _commandService = commandService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Once we reach Initialize dialogs have been completed
            // Initial logic should be placed here for ViewModels while the constructor is used just for dependency injection

            // Since we do don't deal with Window's and allow the dialog service to handle it for us
            // There might be a scenario where you need to change the windows settings
            // ActiveDialog exposes the topmost current dialog, this exposes the Window object for manipulation
            _dialogService.ActiveDialog.RegisterSettings();

            // Tell ApplicationViewModel to show Test1ViewModel inside the 'SelectedViewModel' ContentControl
            _navigationService.Navigate<ApplicationViewModel, Test1ViewModel>();
        }
    }
}
