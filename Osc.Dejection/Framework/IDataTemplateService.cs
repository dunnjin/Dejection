using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Osc.Dejection.Framework
{
    public interface IDataTemplateService
    {
        void InjectDataTemplates(Window content);

        void GenerateDataTemplates(Func<Assembly, bool> predicate);
    }
}
