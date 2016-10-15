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
        private readonly ICommandService commandService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public ICommand NavigateTest1Command
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<Test4ViewModel, Test1ViewModel>();
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
                        navigationService.Navigate<Test4ViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }

        public ICommand CloseDialogCommand
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        dialogService.Close<Test4ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test4ViewModel(ICommandService commandService, INavigationService navigationService, IDialogService dialogService)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public override void Initialize()
        {
            base.Initialize();

            dialogService.ActiveDialog.WindowState = System.Windows.WindowState.Maximized;
        }


    }
    
}
