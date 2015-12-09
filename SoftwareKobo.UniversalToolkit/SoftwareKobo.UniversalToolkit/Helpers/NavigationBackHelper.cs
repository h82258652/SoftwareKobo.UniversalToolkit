using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    [Obsolete("请使用 NavigationHelper", false)]
    public static class NavigationBackHelper
    {
        private static readonly Dictionary<Frame, EventHandler<BackPressedEventArgs>> _backPressedHandlers = new Dictionary<Frame, EventHandler<BackPressedEventArgs>>();

        private static readonly Dictionary<Frame, EventHandler<BackRequestedEventArgs>> _backRequestedHandlers = new Dictionary<Frame, EventHandler<BackRequestedEventArgs>>();

        private static readonly Dictionary<Frame, CoreWindow> _frameWindows = new Dictionary<Frame, CoreWindow>();

        private static readonly Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>> _pointerReleasedHandlers = new Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>>();

        public static void RegisterNavigateBack(this Frame frame, Func<Task> asyncAction)
        {
            #region 标题栏后退键
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            if (_backPressedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackRequestedEventArgs> backRequestedHandler = async (sender, e) =>
                {
                    e.Handled = true;
                    if (asyncAction != null)
                    {
                        await asyncAction.Invoke();
                    }
                };
                _backRequestedHandlers.Add(frame, backRequestedHandler);
                systemNavigationManager.BackRequested += backRequestedHandler;
            }
            #endregion

            #region 设备后退键
            if (HardwareButtonsHelper.IsUseable && _backPressedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackPressedEventArgs> backPressedHandler = async (sender, e) =>
                {
                    e.Handled = true;
                    if (asyncAction != null)
                    {
                        await asyncAction.Invoke();
                    }
                };
                _backPressedHandlers.Add(frame, backPressedHandler);
                HardwareButtons.BackPressed += backPressedHandler;
            }
            #endregion

            #region 鼠标后退键
            CoreWindow window = CoreWindow.GetForCurrentThread();
            if (_frameWindows.ContainsKey(frame) == false && _pointerReleasedHandlers.ContainsKey(frame) == false)
            {
                TypedEventHandler<CoreWindow, PointerEventArgs> pointerReleasedHandler = async (sender, e) =>
                {
                    e.Handled = true;
                    if (e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Released)
                    {
                        if (asyncAction != null)
                        {
                            await asyncAction.Invoke();
                        }
                    }
                };
                _frameWindows.Add(frame, window);
                _pointerReleasedHandlers.Add(frame, pointerReleasedHandler);
                window.PointerReleased += pointerReleasedHandler;
            }
            #endregion
        }

        public static void RegisterNavigateBack(this Frame frame, Action action)
        {
            #region 标题栏后退键
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            if (_backPressedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackRequestedEventArgs> backRequestedHandler = (sender, e) =>
                {
                    e.Handled = true;
                    if (action != null)
                    {
                        action.Invoke();
                    }
                };
                _backRequestedHandlers.Add(frame, backRequestedHandler);
                systemNavigationManager.BackRequested += backRequestedHandler;
            }
            #endregion

            #region 设备后退键
            if (HardwareButtonsHelper.IsUseable && _backPressedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackPressedEventArgs> backPressedHandler = (sender, e) =>
                {
                    e.Handled = true;
                    if (action != null)
                    {
                        action.Invoke();
                    }
                };
                _backPressedHandlers.Add(frame, backPressedHandler);
                HardwareButtons.BackPressed += backPressedHandler;
            }
            #endregion

            #region 鼠标后退键
            CoreWindow window = CoreWindow.GetForCurrentThread();
            if (_frameWindows.ContainsKey(frame) == false && _pointerReleasedHandlers.ContainsKey(frame) == false)
            {
                TypedEventHandler<CoreWindow, PointerEventArgs> pointerReleasedHandler = (sender, e) =>
                {
                    e.Handled = true;
                    if (e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Released)
                    {
                        if (action != null)
                        {
                            action.Invoke();
                        }
                    }
                };
                _frameWindows.Add(frame, window);
                _pointerReleasedHandlers.Add(frame, pointerReleasedHandler);
                window.PointerReleased += pointerReleasedHandler;
            }
            #endregion
        }

        public static void RegisterNavigateBack(this Frame frame)
        {
            Action action = () =>
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
            };
            RegisterNavigateBack(frame, action);
        }

        public static void UnregisterNavigateBack(this Frame frame)
        {
            #region 标题栏后退键
            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            if (_backRequestedHandlers.ContainsKey(frame))
            {
                EventHandler<BackRequestedEventArgs> backRequestedHandler = _backRequestedHandlers[frame];
                systemNavigationManager.BackRequested -= backRequestedHandler;
                _backRequestedHandlers.Remove(frame);
            }
            #endregion

            #region 设备后退键
            if (HardwareButtonsHelper.IsUseable && _backPressedHandlers.ContainsKey(frame))
            {
                EventHandler<BackPressedEventArgs> backPressedHandler = _backPressedHandlers[frame];
                HardwareButtons.BackPressed -= backPressedHandler;
                _backPressedHandlers.Remove(frame);
            }
            #endregion

            #region 鼠标后退键
            if (_frameWindows.ContainsKey(frame) && _pointerReleasedHandlers.ContainsKey(frame))
            {
                CoreWindow window = _frameWindows[frame];
                TypedEventHandler<CoreWindow, PointerEventArgs> pointerReleasedHandler = _pointerReleasedHandlers[frame];
                window.PointerReleased -= pointerReleasedHandler;
                _frameWindows.Remove(frame);
                _pointerReleasedHandlers.Remove(frame);
            }
            #endregion
        }
    }
}