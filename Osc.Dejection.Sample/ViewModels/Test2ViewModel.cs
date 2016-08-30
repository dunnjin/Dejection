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

        public Test2ViewModel(ICommandService commandService, INavigationService navigationService)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
        }

        
    }
}
