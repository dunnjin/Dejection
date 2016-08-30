using Osc.Dejection.Extensions;
using Osc.Dejection.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Osc.Dejection.Navigation
{
    public class NavigationService : INavigationService
    {
        #region Fields

        private readonly IDialogService dialogService;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IInitializationService initializationService;

        #endregion

        #region Initialize

        public NavigationService(IDialogService dialogService, IViewModelFactory viewModelFactory, IInitializationService initializationService)
        {
            this.dialogService = dialogService;
            this.viewModelFactory = viewModelFactory;
            this.initializationService = initializationService;

            this.dialogService.ThrowIfNull(nameof(dialogService));
            this.viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            this.initializationService.ThrowIfNull(nameof(initializationService));
        }

        #endregion    

        /// <summary>
        /// Takes the given owners selected view model and sets it to the given target
        /// </summary>
        /// <typeparam name="Owner">The view model that wants to display the target</typeparam>
        /// <typeparam name="Target">The view model thats wants to be displayed</typeparam>
        public void Navigate<Owner, Target>()
            where Owner : ViewModelBase
            where Target : ViewModelBase
        {
            ViewModelBase owner = dialogService.ActiveWindows
                .Select(obj => FindOwner<Owner>((obj.Content as ContentControl)?.Content as ViewModelBase))
                .FirstOrDefault(obj => !obj.IsNull());

            if (owner.IsNull())
                return;

            ViewModelBase previous = owner.SelectedViewModel;

            if (!previous.IsNull())
                initializationService.UnInitialize(previous);

            ViewModelBase target = viewModelFactory.CreateViewModel<Target>();

            if (!target.IsNull())
                initializationService.Initialize(target);

            owner.SelectedViewModel = target;
        }

        private ViewModelBase FindOwner<Owner>(ViewModelBase parent)
            where Owner : ViewModelBase
        {
            if (parent.IsNull())
                return null;

            if (parent is Owner)
                return parent;

            return FindOwner<Owner>(parent.SelectedViewModel);
        }
    }
}
