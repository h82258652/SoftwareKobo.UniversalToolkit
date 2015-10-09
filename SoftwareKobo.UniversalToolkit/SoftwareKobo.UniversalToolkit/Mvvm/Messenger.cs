using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public static class Messenger
    {
        private static List<WeakReference<ViewModelBase>> _viewModels = new List<WeakReference<ViewModelBase>>();

        private static Dictionary<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>> _views = new Dictionary<WeakReference<FrameworkElement>, WeakReference<ReceiveFromViewModelHandler>>();

        public static void Register<TView>(TView view) where TView : FrameworkElement, IView
        {
            Register(view, view.ReceiveFromViewModel);
        }

        public static void Register(FrameworkElement view, ReceiveFromViewModelHandler handler)
        {
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

        public static void Unregister(FrameworkElement view)
        {
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