using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Framework
{
    public interface IViewModelFactory
    {
        TSource CreateViewModel<TSource>()
            where TSource : ViewModelBase;
    }
}
