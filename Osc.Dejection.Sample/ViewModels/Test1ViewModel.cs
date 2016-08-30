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
        private readonly ICommandService commandService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public ICommand ShowDialogCommand
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<ApplicationViewModel, Test2ViewModel>();
                        dialogService.ShowDialog<Test4ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test1ViewModel(ICommandService commandService, INavigationService navigationService, IDialogService dialogService)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;


        }
    }
}
