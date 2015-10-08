using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public static class Messenger
    {
        private static List<WeakReference<ViewModelBase>> _viewModels = new List<WeakReference<ViewModelBase>>();

        private static Dictionary<WeakReference<FrameworkElement>, ReceiveFromViewModelHandler> _views = new Dictionary<WeakReference<FrameworkElement>, ReceiveFromViewModelHandler>();

        public static void Register<TView>(TView view) where TView : FrameworkElement, IView
        {
            Register(view, view.ReceiveFromViewModel);
        }

        public static void Register(FrameworkElement view, ReceiveFromViewModelHandler handler)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, ReceiveFromViewModelHandler> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> reference = keyValue.Key;
                FrameworkElement temp;
                if (reference.TryGetTarget(out temp))
                {
                    throw new ArgumentException("this view had registered.", nameof(view));
                }
                else
                {
                    _views.Remove(reference);
                    i--;
                }
            }
            _views.Add(new WeakReference<FrameworkElement>(view), handler);
        }

        public static void UnRegister(FrameworkElement view)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, ReceiveFromViewModelHandler> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> reference = keyValue.Key;
                FrameworkElement temp;
                if (reference.TryGetTarget(out temp))
                {
                    if (temp == view)
                    {
                        _views.Remove(reference);
                        return;
                    }
                }
                else
                {
                    _views.Remove(reference);
                    i--;
                }
            }
        }

        internal static void Process(ViewModelBase viewModel, object parameter)
        {
            string viewModelName = viewModel.GetName();
            if (viewModelName.EndsWith("Model") == false)
            {
                throw new ArgumentException("view model name should ends with model");
            }
            string targetViewName = viewModelName.Substring(0, viewModelName.LastIndexOf("Model"));

            for (int i = 0; i < _views.Count; i++)
            {
                KeyValuePair<WeakReference<FrameworkElement>, ReceiveFromViewModelHandler> keyValue = _views.ElementAt(i);
                WeakReference<FrameworkElement> reference = keyValue.Key;
                FrameworkElement view;
                if (reference.TryGetTarget(out view))
                {
                    if (view.GetType().Name == targetViewName)
                    {
                        var handler = keyValue.Value;
                        if (handler != null)
                        {
                            keyValue.Value.Invoke(viewModel, parameter);
                        }
                    }
                }
                else
                {
                    _views.Remove(reference);
                    i--;
                }
            }
        }

        internal static void Process(FrameworkElement view, object parameter)
        {
            string viewName = null;
            string targetViewModelName = null;

            for (int i = 0; i < _viewModels.Count; i++)
            {
                WeakReference<ViewModelBase> reference = _viewModels[i];
                ViewModelBase viewModel;
                if (reference.TryGetTarget(out viewModel))
                {
                    if (viewName == null)
                    {
                        viewName = view.GetType().Name;
                        targetViewModelName = viewName + "Model";
                    }

                    if (viewModel.GetName() == targetViewModelName)
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

        internal static void Register(ViewModelBase viewModel)
        {
            WeakReference<ViewModelBase> reference = new WeakReference<ViewModelBase>(viewModel);
            _viewModels.Add(reference);
        }
    }
}