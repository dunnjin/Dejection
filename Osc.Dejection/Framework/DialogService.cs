using Osc.Dejection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Osc.Dejection.Framework
{    
    public class DialogService : IDialogService
    {  
        #region Fields

        private readonly IDataTemplateService _dataTemplateService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IInitializationService _initializationService;

        private List<Window> _windowCollection = new List<Window>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the top most active dialog
        /// </summary>
        public Window ActiveDialog
        {
            get
            {
                return _windowCollection.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all active dialogs
        /// </summary>
        public IEnumerable<Window> ActiveDialogs
        {
            get
            {
                return _windowCollection;
            }
        }

        #endregion

        #region Initialize

        public DialogService(IDataTemplateService dataTemplateService,
            IViewModelFactory viewModelFactory,
            IInitializationService initializationService)
        {
            _dataTemplateService = dataTemplateService;
            _viewModelFactory = viewModelFactory;
            _initializationService = initializationService;

            _dataTemplateService.ThrowIfNull(nameof(dataTemplateService));
            _viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            _initializationService.ThrowIfNull(nameof(initializationService));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a window and returns without waiting for the newly opened window to close
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <typeparam name="TViewModel"></typeparam>
        public void Show<TViewModel>()
            where TViewModel : ViewModelBase
        {
            CreateWindow<TViewModel>()
                .Show();
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public bool? ShowDialog<TViewModel>()
            where TViewModel : ViewModelBase
        {
            return CreateWindow<TViewModel>()
                .ShowDialog();
        }


        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed... but asynchronously
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public Task<bool?> ShowDialogAsync<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var window = CreateWindow<TViewModel>();

            var taskCompletionSource = new TaskCompletionSource<bool?>();

            window.Dispatcher.BeginInvoke(new Action(() => taskCompletionSource.SetResult(window.ShowDialog())));

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Opens a window with the given view model by reference
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
        {
            var window = CreateWindow<TViewModel>();
            ((ContentControl)window.Content).Content = viewModel;

            return window.ShowDialog();
        }


        /// <summary>
        /// Opens a window with the given view model by reference, asynchronously
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel) 
            where TViewModel : ViewModelBase
        {
            var window = CreateWindow<TViewModel>();
            ((ContentControl)window.Content).Content = viewModel;

            var taskCompletionSource = new TaskCompletionSource<bool?>();

            window.Dispatcher.BeginInvoke(new Action(() => taskCompletionSource.SetResult(window.ShowDialog())));

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Closes the active dialog specified
        /// </summary>
        /// <exception cref="DialogNotActiveException">Closing something that isn't open</exception>
        /// <typeparam name="TViewModel"></typeparam>
        public void Close<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var window = _windowCollection.FirstOrDefault(obj => obj.Tag as Type == typeof(TViewModel));

            if (window.IsNull())
                return;

            var viewModel = (window.Content as ContentControl)?.Content as ViewModelBase;

            if (!viewModel.IsNull())
                _initializationService.UnInitialize(viewModel);

            window.Closing -= ClosingHandler;

            window.Close();

            _windowCollection.Remove(window);
        }

        private Window CreateWindow<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var viewModel = _viewModelFactory.CreateViewModel<TViewModel>();
            viewModel.ThrowIfNull(nameof(viewModel));

            var parentWindow = ActiveDialog;

            Window newWindow;

            if (parentWindow.IsNull())
                newWindow = CreateMainWindow(viewModel);
            else
                newWindow = CreateChildWindow(viewModel, parentWindow);

            _dataTemplateService.InjectDataTemplates(newWindow);

            _windowCollection.Insert(0, newWindow);

            _initializationService.Initialize(viewModel);

            return newWindow;
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        
        private Window CreateChildWindow(ViewModelBase viewModel, Window parentWindow)
        {
            var window = new Window()
            {
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.WidthAndHeight,
                Width = parentWindow.ActualWidth,
                Height = parentWindow.ActualHeight,
                Tag = viewModel.GetType(),
                Owner = parentWindow,
                Content = new ContentControl()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Content = viewModel,
                },
            };

            window.Closing += ClosingHandler;

            return window;
        }

        private Window CreateMainWindow(ViewModelBase viewModel)
        {
            var window = new Window()
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.SingleBorderWindow,
                ResizeMode = ResizeMode.CanResize,
                AllowsTransparency = false,
                Tag = viewModel.GetType(),
                Content = new ContentControl()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Content = viewModel,
                },
            };
            
            return window;
        }



        #endregion
    }
}
