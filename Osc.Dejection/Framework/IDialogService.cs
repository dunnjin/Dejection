using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Osc.Dejection.Framework
{
    public interface IDialogService
    {
        Window ActiveDialog { get; }

        IEnumerable<Window> ActiveDialogs { get; }

        void Show<TViewModel>()
            where TViewModel : ViewModelBase;

        bool? ShowDialog<TViewModel>()
            where TViewModel : ViewModelBase;

        Task<bool?> ShowDialogAsync<TViewModel>()
            where TViewModel : ViewModelBase;

        bool? ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        void Close<TViewModel>()
            where TViewModel : ViewModelBase;        
    }
}
