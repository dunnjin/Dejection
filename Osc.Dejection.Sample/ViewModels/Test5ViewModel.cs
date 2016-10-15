using Osc.Dejection.Commands;
using Osc.Dejection.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Sample.ViewModels
{
    public class Test5ViewModel : ViewModelBase
    {
        private readonly ICommandService commandService;
        private readonly IDialogService dialogService;

        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                    return;

                text = value;

                NotifyPropertyChanged();
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return commandService
                    .Execute(() =>
                    {
                        dialogService.Close<Test5ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test5ViewModel(ICommandService commandService, IDialogService dialogService)
        {
            this.commandService = commandService;
            this.dialogService = dialogService;
        }
    }
}
