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
        private readonly ICommandService _commandService;
        private readonly IDialogService _dialogService;

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                _text = value;

                NotifyPropertyChanged();
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        _dialogService.Close<Test5ViewModel>();
                    })
                    .Relay();
            }
        }

        public Test5ViewModel(ICommandService commandService, IDialogService dialogService)
        {
            _commandService = commandService;
            _dialogService = dialogService;
        }
    }
}
