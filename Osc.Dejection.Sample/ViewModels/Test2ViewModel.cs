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
        private readonly ICommandService commandService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;
        private readonly IViewModelFactory viewModelFactory;

        public ICommand NavigateCommand
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<ApplicationViewModel, Test1ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand MessageDialogCommand
        {
            get
            {
                return commandService
                    .Execute(async () =>
                    {
                        var viewModelTest = viewModelFactory.CreateViewModel<Test5ViewModel>();
                        viewModelTest.Text = @"  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec adipiscing
    nulla quis libero egestas lobortis. Duis blandit imperdiet ornare. Nulla
    ac arcu ut purus placerat congue. Integer pretium fermentum gravida.";

                        await dialogService.ShowDialogAsync(viewModelTest);                                                
                    })
                    .Relay();
            }
        }

        public Test2ViewModel(ICommandService commandService,
            INavigationService navigationService,
            IDialogService dialogService,
            IViewModelFactory viewModelFactory)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.viewModelFactory = viewModelFactory;
        }

        
    }
}
