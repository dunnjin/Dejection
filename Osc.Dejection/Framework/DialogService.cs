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
    public class DialogNotActiveException : Exception
    {
        public DialogNotActiveException()
        {
        }

        public DialogNotActiveException(string message) : base(message)
        {
        }

        public DialogNotActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DialogNotActiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DialogService : IDialogService
    {
        #region Fields

        private readonly IDataTemplateService dataTemplateService;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IInitializationService initializationService;

        private List<Window> windowCollection = new List<Window>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the top most dialog active
        /// </summary>
        public Window ParentWindow
        {
            get
            {
                return windowCollection.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all active dialogs
        /// </summary>
        public IEnumerable<Window> ActiveWindows
        {
            get
            {
                return windowCollection;
            }
        }

        #endregion

        #region Initialize

        public DialogService(IDataTemplateService dataTemplateService, IViewModelFactory viewModelFactory, IInitializationService initializationService)
        {
            this.dataTemplateService = dataTemplateService;
            this.viewModelFactory = viewModelFactory;
            this.initializationService = initializationService;

            this.dataTemplateService.ThrowIfNull(nameof(dataTemplateService));
            this.viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            this.initializationService.ThrowIfNull(nameof(initializationService));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a window and returns without waiting for the newly opened window to close
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <typeparam name="TSource"></typeparam>
        public void Show<TSource>()
            where TSource : ViewModelBase
        {
            CreateWindow<TSource>()
                .Show();
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public bool? ShowDialog<TSource>()
            where TSource : ViewModelBase
        {
            return CreateWindow<TSource>()
                .ShowDialog();
        }


        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed... but asynchronously
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public Task<bool?> ShowDialogAsync<TSource>()
            where TSource : ViewModelBase
        {
            Window window = CreateWindow<TSource>();

            TaskCompletionSource<bool?> taskCompletionSource = new TaskCompletionSource<bool?>();

            window.Dispatcher.BeginInvoke(new Action(() => taskCompletionSource.SetResult(window.ShowDialog())));

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Closes the active dialog specified
        /// </summary>
        /// <exception cref="DialogNotActiveException">Closing something that isn't open</exception>
        /// <typeparam name="TSource"></typeparam>
        public void Close<TSource>()
            where TSource : ViewModelBase
        {
            Window window = windowCollection.FirstOrDefault(obj => obj.Tag as Type == typeof(TSource));

            if (window.IsNull())
                throw new DialogNotActiveException(typeof(TSource).Name);

            ViewModelBase viewModel = (window.Content as ContentControl)?.Content as ViewModelBase;

            if (!viewModel.IsNull())
                initializationService.UnInitialize(viewModel);

            window.Closing -= ClosingHandler;

            window.Close();

            windowCollection.Remove(window);
        }

        private Window CreateWindow<TSource>()
            where TSource : ViewModelBase
        {
            ViewModelBase viewModel = viewModelFactory.CreateViewModel<TSource>();
            viewModel.ThrowIfNull(nameof(viewModel));

            Window parentWindow = ParentWindow;

            Window newWindow;

            if (parentWindow.IsNull())
                newWindow = CreateMainWindow(viewModel);
            else
                newWindow = CreateChildWindow(viewModel, parentWindow);

            dataTemplateService.InjectDataTemplates(newWindow);

            windowCollection.Insert(0, newWindow);

            initializationService.Initialize(viewModel);

            return newWindow;
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        
        private Window CreateChildWindow(ViewModelBase viewModel, Window parentWindow)
        {
            Window window = new Window()
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
            Window window = new Window()
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
