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
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ICommandService commandService;
        private readonly INavigationService navigationService;

        public ICommand Test1Command
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

        public ICommand Test2Command
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<ApplicationViewModel, Test2ViewModel>();
                    })
                    .Relay();
            }
        }


        public ICommand Test3Command
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        navigationService.Navigate<ApplicationViewModel, Test3ViewModel>();
                    })
                    .Relay();
            }
        }

        public ApplicationViewModel(ICommandService commandService, INavigationService navigationService)
        {
            this.commandService = commandService;
            this.navigationService = navigationService;
        }

        public override void Initialize()
        {
            base.Initialize();

            navigationService.Navigate<ApplicationViewModel, Test1ViewModel>();
        }
    }
}
