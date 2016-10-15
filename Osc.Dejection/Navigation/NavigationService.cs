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

        private readonly IDialogService _dialogService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IInitializationService _initializationService;

        #endregion

        #region Initialize

        public NavigationService(IDialogService dialogService,
            IViewModelFactory viewModelFactory,
            IInitializationService initializationService)
        {
            _dialogService = dialogService;
            _viewModelFactory = viewModelFactory;
            _initializationService = initializationService;

            _dialogService.ThrowIfNull(nameof(dialogService));
            _viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            _initializationService.ThrowIfNull(nameof(initializationService));
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
            var owner = _dialogService.ActiveDialogs
                .Select(obj => FindOwner<Owner>((obj.Content as ContentControl)?.Content as ViewModelBase))
                .FirstOrDefault(obj => !obj.IsNull());

            if (owner.IsNull())
                return;

            var previous = owner.SelectedViewModel;

            if (!previous.IsNull())
                _initializationService.UnInitialize(previous);

            var target = _viewModelFactory.CreateViewModel<Target>();

            if (!target.IsNull())
                _initializationService.Initialize(target);

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
