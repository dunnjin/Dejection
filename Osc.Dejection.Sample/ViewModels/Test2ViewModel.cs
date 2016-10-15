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
    public class Test2ViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IViewModelFactory _viewModelFactory;

        public ICommand NavigateCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _navigationService.Navigate<ApplicationViewModel, Test1ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand MessageDialogCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        // Use ViewModel factory to create the specified ViewModel
                        var viewModelTest = _viewModelFactory.CreateViewModel<Test5ViewModel>();
                        viewModelTest.Text = @" Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec adipiscing
                                                nulla quis libero egestas lobortis. Duis blandit imperdiet ornare. Nulla
                                                ac arcu ut purus placerat congue. Integer pretium fermentum gravida.";
                        // Use the overload to pass a reference to a ViewModel
                        // This could be used as Confirm/Cancel dialogs to have dialog service display the dialog and have a reference to the ViewModel to gain its user specified data
                        _dialogService.ShowDialog(viewModelTest);                                                
                    })
                    .Relay();
            }
        }

        public Test2ViewModel(ICommandService commandService,
            INavigationService navigationService,
            IDialogService dialogService,
            IViewModelFactory viewModelFactory)
        {
            _commandService = commandService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _viewModelFactory = viewModelFactory;
        }

        
    }
}
