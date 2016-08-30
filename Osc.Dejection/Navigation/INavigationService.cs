using Osc.Dejection.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Navigation
{
    public interface INavigationService
    {
        void Navigate<Owner, Target>()
            where Owner : ViewModelBase
            where Target : ViewModelBase;
    }
}
