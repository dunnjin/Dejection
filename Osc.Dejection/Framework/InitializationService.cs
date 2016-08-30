using Osc.Dejection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Framework
{
    /// <summary>
    /// This exists in case I need to expand on what initializing can do for a view model
    /// And its repeated for a new dialog and when navigation occurs
    /// </summary>
    public class InitializationService : IInitializationService
    {
        public void Initialize(ViewModelBase viewModel)
        {
            viewModel.ThrowIfNull(nameof(viewModel));

            viewModel.Initialize();
        }

        public void UnInitialize(ViewModelBase viewModel)
        {
            viewModel.ThrowIfNull(nameof(viewModel));

            viewModel.UnInitialize();
        }
    }
}
