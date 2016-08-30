using Microsoft.Practices.Unity;
using Osc.Dejection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Framework
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IUnityContainer unityContainer;

        public ViewModelFactory(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;

            this.unityContainer.ThrowIfNull(nameof(unityContainer));
        }

        /// <summary>
        /// Resolves viewmodel from unity IoC
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public TSource CreateViewModel<TSource>() where TSource : ViewModelBase
        {
            return unityContainer.Resolve<TSource>();
        }
    }
}
