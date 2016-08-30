using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Framework
{
    public interface IInitializationService
    {
        void Initialize(ViewModelBase viewModel);

        void UnInitialize(ViewModelBase viewModel);
    }
}
