using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// View 和 ViewModel 通信管理器。
    /// </summary>
    public static class Messenger
    {
        private static List<WeakReference<ViewModelBase>> _viewModels = new List<WeakReference<ViewModelBase>>();

        private static Dictionary<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>> _views = new Dictionary<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>>();

        /// <summary>
        /// 将 View 注册到通信管理器中。
        /// </summary>
        /// <typeparam name="TView">实现了 IView 接口的类型。</typeparam>
        /// <param name="view">需要注册到通信管理器的 View。</param>
        /// <example>
        /// protected override void OnNavigatedTo(NavigationEventArgs e)
        /// {
        ///     Messenger.Register(this);
        /// }
        /// </example>
        public static void Register<TView>(TView view) where TView : FrameworkElement, IView
        {
            Register(view, view.ReceiveFromViewModel);
        }

        /// <summary>
        /// 将 View 注册到通信管理器中。
        /// </summary>
        /// <param name="view">需要注册到通信管理器的 View。</param>
        /// <param name="handler">处理来自 ViewModel 的消息的方法。</param>
        /// <exception cref="ArgumentNullException">view 或者 handler 为 null。</exception>
        public static void Register(FrameworkElement view, ReceiveFromViewModelHandler handler)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> viewReference = keyValue.Key;
                FrameworkElement temp;
                if (viewReference.TryGetTarget(out temp))
                {
                    throw new ArgumentException("this view had registered.", nameof(view));
                }
                else
                {
                    _views.Remove(viewReference);
                    i--;
                }
            }
            _views.Add(new WeakReference<FrameworkElement>(view), new WeakReference<ReceiveFromViewModelHandler>(handler));
        }

        /// <summary>
        /// 将 View 从通信管理器中注销。
        /// </summary>
        /// <param name="view">需要注销的 View。</param>
        public static void Unregister(FrameworkElement view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> viewReference = keyValue.Key;
                FrameworkElement temp;
                if (viewReference.TryGetTarget(out temp))
                {
                    if (temp == view)
                    {
                        _views.Remove(viewReference);
                        return;
                    }
                }
                else
                {
                    if (_views.Remove(viewReference))
                    {
                        i--;
                    }
                }
            }
        }

        internal static void Register(ViewModelBase viewModel)
        {
            _viewModels.Add(new WeakReference<ViewModelBase>(viewModel));
        }

        internal static void Unregister(ViewModelBase viewModel)
        {
            for (int i = 0; i < _viewModels.Count; i++)
            {
                WeakReference<ViewModelBase> reference = _viewModels[i];
                ViewModelBase temp;
                if (reference.TryGetTarget(out temp))
                {
                    if (temp == viewModel)
                    {
                        _viewModels.Remove(reference);
                        return;
                    }
                }
                else
                {
                    _viewModels.Remove(reference);
                    i--;
                }
            }
        }

        internal static void Process(FrameworkElement view, object parameter)
        {
            string targetViewModelName = view.GetType().Name + "Model";
            for (int i = 0; i < _viewModels.Count; i++)
            {
                WeakReference<ViewModelBase> reference = _viewModels[i];
                ViewModelBase viewModel;
                if (reference.TryGetTarget(out viewModel))
                {
                    if (viewModel.GetType().Name == targetViewModelName)
                    {
                        viewModel.ReceiveFromView(view, parameter);
                    }
                }
                else
                {
                    _viewModels.Remove(reference);
                    i--;
                }
            }
        }

        internal static void Process(ViewModelBase viewModel, object parameter)
        {
            string viewModelName = viewModel.GetType().Name;
            int index = viewModelName.LastIndexOf("Model");
            if (index < 0)
            {
                throw new ArgumentException("view model name should ends with model.", nameof(viewModel));
            }
            string targetViewName = viewModelName.Substring(0, index);

            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> viewReference = keyValue.Key;
                FrameworkElement view;
                bool isNeedToRemove = false;
                if (viewReference.TryGetTarget(out view))
                {
                    if (view.GetType().Name == targetViewName)
                    {
                        WeakReference<ReceiveFromViewModelHandler> handlerReference = keyValue.Value;
                        ReceiveFromViewModelHandler handler;
                        if (handlerReference.TryGetTarget(out handler))
                        {
                            handler(viewModel, parameter);
                        }
                        else
                        {
                            isNeedToRemove = true;
                        }
                    }
                }
                else
                {
                    isNeedToRemove = true;
                }

                if (isNeedToRemove)
                {
                    if (_views.Remove(viewReference))
                    {
                        i--;
                    }
                }
            }
        }
    }
}