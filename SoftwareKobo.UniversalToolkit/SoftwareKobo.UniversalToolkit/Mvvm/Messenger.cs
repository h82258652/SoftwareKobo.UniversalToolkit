using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public static class Messenger
    {
        private static readonly List<WeakReference<IView>> _registeredViews = new List<WeakReference<IView>>();

        /// <summary>
        /// 将 View 注册到通信管理器中。
        /// </summary>
        /// <param name="view">需要注册到通信管理器的 View。</param>
        public static void Register(IView view)
        {
            for (var i = 0; i < _registeredViews.Count; i++)
            {
                var registeredViewReference = _registeredViews[i];
                IView registeredView;
                if (registeredViewReference.TryGetTarget(out registeredView))
                {
                    if (registeredView == view)
                    {
                        return;
                    }
                }
                else
                {
                    if (_registeredViews.Remove(registeredViewReference))
                    {
                        i--;
                    }
                }
            }

            _registeredViews.Add(new WeakReference<IView>(view));
        }

        /// <summary>
        /// 将 View 从通信管理器中注销。
        /// </summary>
        /// <param name="view">需要注销的 View。</param>
        public static void Unregister(IView view)
        {
            for (var i = 0; i < _registeredViews.Count; i++)
            {
                var registeredViewReference = _registeredViews[i];
                IView registeredView;
                if (registeredViewReference.TryGetTarget(out registeredView))
                {
                    if (registeredView == view)
                    {
                        _registeredViews.Remove(registeredViewReference);
                    }
                }
                else
                {
                    if (_registeredViews.Remove(registeredViewReference))
                    {
                        i--;
                    }
                }
            }
        }

        internal static void Process(IView view, object parameter)
        {
            var viewModel = view.DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.ReceiveFromView(parameter);
            }
        }

        internal static async void Process(ViewModelBase viewModel, object parameter)
        {
            for (var i = 0; i < _registeredViews.Count; i++)
            {
                var registeredViewReference = _registeredViews[i];
                IView registeredView;
                if (registeredViewReference.TryGetTarget(out registeredView))
                {
                    var element = registeredView as DependencyObject;
                    if (element != null)
                    {
                        try
                        {
                            if (element.Dispatcher.HasThreadAccess)
                            {
                                if (registeredView.DataContext == viewModel)
                                {
                                    registeredView.ReceiveFromViewModel(parameter);
                                }
                            }
                            else
                            {
                                await element.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    if (registeredView.DataContext == viewModel)
                                    {
                                        registeredView.ReceiveFromViewModel(parameter);
                                    }
                                });
                            }
                        }
                        catch (InvalidComObjectException ex)
                        {
                        }
                    }
                    else
                    {
                        if (registeredView.DataContext == viewModel)
                        {
                            registeredView.ReceiveFromViewModel(parameter);
                        }
                    }
                }
                else
                {
                    if (_registeredViews.Remove(registeredViewReference))
                    {
                        i--;
                    }
                }
            }
        }
    }
}