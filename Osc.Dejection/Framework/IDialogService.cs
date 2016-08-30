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
        Window ParentWindow { get; }

        IEnumerable<Window> ActiveWindows { get; }

        void Show<TSource>()
            where TSource : ViewModelBase;

        bool? ShowDialog<TSource>()
            where TSource : ViewModelBase;

        Task<bool?> ShowDialogAsync<TSource>()
            where TSource : ViewModelBase;

        void Close<TSource>()
            where TSource : ViewModelBase; 
    }
}
