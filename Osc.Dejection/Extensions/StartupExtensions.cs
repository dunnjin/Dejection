using Microsoft.Practices.Unity;
using Osc.Dejection.Commands;
using Osc.Dejection.Framework;
using Osc.Dejection.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Osc.Dejection.Extensions
{
    public interface IDataTemplateInjector
    {
        /// <summary>
        /// Creates main window given the specified viewmodel and its generated view
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        void Start<TViewModel>()
         where TViewModel : ViewModelBase;
    }
    public interface IDataTemplateInjectorStart
    {
        /// <summary>
        /// Exposes unity container to register dependencies
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        IDataTemplateInjector Container(Action<IUnityContainer> container);

        /// <summary>
        /// Creates main window given the specified viewmodel and its generated view
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        void Start<TViewModel>()
            where TViewModel : ViewModelBase;
    }

    public class DataTemplateInjector : IDataTemplateInjectorStart, IDataTemplateInjector
    {
        private readonly IDialogService dialogService;
        private readonly IUnityContainer unityContainer;

        public DataTemplateInjector(IDialogService dialogService, IUnityContainer unityContainer)
        {
            this.dialogService = dialogService;
            this.unityContainer = unityContainer;

            this.dialogService.ThrowIfNull(nameof(dialogService));
            this.unityContainer.ThrowIfNull(nameof(unityContainer));
        }

        /// <summary>
        /// Exposes unity container to register dependencies
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public IDataTemplateInjector Container(Action<IUnityContainer> container)
        {
            container(unityContainer);

            return this;
        }

        /// <summary>
        /// Creates main window given the specified viewmodel and its generated view
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void Start<TViewModel>()
            where TViewModel : ViewModelBase
        {
            dialogService.ShowDialog<TViewModel>();
        }
    }

    /// <summary>
    /// A little extension to mimic IAppBuilder in mvc, where we setup our initial program
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Specifies the pattern of which AppDomain dlls to look for ViewModel / View associations to generate the dynamic datatemplates
        /// </summary>
        /// <param name="source">OnStartup</param>
        /// <param name="predicate">Pattern for dll match</param>
        /// <returns></returns>
        public static IDataTemplateInjectorStart Register(this StartupEventArgs source, Func<Assembly, bool> predicate)
        {
            // Contruction of dependecies and application
            IUnityContainer container = new UnityContainer();
            
            container.RegisterType<IDataTemplateService, DataTemplateService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICommandService, CommandService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IViewModelFactory, ViewModelFactory>();
            container.RegisterType<IDataTemplateInjector, DataTemplateInjector>();
            container.RegisterType<INavigationService, NavigationService>();
            container.RegisterType<IInitializationService, InitializationService>();

            // Load datatemplates into memory and attach to application view
            IDataTemplateService dataTemplateService = container.Resolve<IDataTemplateService>();
       
            dataTemplateService.GenerateDataTemplates(predicate);

            return container.Resolve<DataTemplateInjector>();
        }
    }
}
