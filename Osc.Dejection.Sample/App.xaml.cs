using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Osc.Dejection.Commands;
using Osc.Dejection.Extensions;
using Osc.Dejection.Sample.ViewModels;
using Microsoft.Practices.Unity;

namespace Osc.Dejection.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            startupEventArgs
              .Register(assembly => assembly.FullName.StartsWith("Osc"))
              .Container(container =>
              {
                  // Setup listeners
                  ICommandService commandService = container.Resolve<ICommandService>();

                  // We can use Nlog or Log4Net to log all Debug/Errors in a single class rather than then calling it in every class
                  commandService.Listener.OnException = (exception, data) =>
                  {
                        Console.WriteLine(exception.ToString());
                  };
              })
              .Start<ApplicationViewModel>();

        }
    }
}
