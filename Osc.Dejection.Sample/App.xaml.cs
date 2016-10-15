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
        /// <summary>
        /// Required entry point
        /// Uses an extension on StartupEventArgs to start the app
        /// </summary>
        /// <param name="startupEventArgs"></param>
        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            startupEventArgs
                .Register(assembly => assembly.FullName.StartsWith("Osc"))
                .Container(container =>
                {
                    // NOTE: Dejection dependencies are all already registered

                    // Here you should register your dependencies which will automatically be resolved via creation of ViewModels

                    // Setup listeners
                    var commandService = container.Resolve<ICommandService>();

                    // Captures all exceptions that occur
                    commandService.Listener.OnException = (exception, data) =>
                    {
                        Console.WriteLine(exception.ToString());
                    };

                    // Captures all actions executed
                    commandService.Listener.Execute = data =>
                    {
                        Console.WriteLine($"Class: {data.ClassName}");
                        Console.WriteLine($"Method: {data.MethodName}");
                        Console.WriteLine($"Line Number: {data.LineNumber}");
                    };
                })
                .Start<ApplicationViewModel>();

        }
    }
}
