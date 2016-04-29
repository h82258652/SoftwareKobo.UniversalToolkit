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
    public static class NavigationHelper
    {
        private static readonly Dictionary<Frame, CoreWindow> _frameWindows = new Dictionary<Frame, CoreWindow>();

        private static readonly Dictionary<Frame, EventHandler<BackPressedEventArgs>> _hardwareNavigateBackHandlers = new Dictionary<Frame, EventHandler<BackPressedEventArgs>>();

        private static readonly Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>> _mouseXButtonHandlers = new Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>>();

        private static readonly Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>> _pointerMovedHandlers = new Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>>();

        private static readonly Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>> _pointerPressedHandlers = new Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>>();

        private static readonly Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>> _pointerReleasedHandlers = new Dictionary<Frame, TypedEventHandler<CoreWindow, PointerEventArgs>>();

        private static readonly Dictionary<Frame, EventHandler<BackRequestedEventArgs>> _systemNavigateBackHandlers = new Dictionary<Frame, EventHandler<BackRequestedEventArgs>>();

        public static void Register(Frame frame)
        {
            Register(frame, new NavigationHelperConfig());
        }

        public static void Register(Frame frame, NavigationHelperConfig config)
        {
            Action backAction = () =>
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
            };
            Register(frame, backAction);
        }

        public static void Register(Frame frame, Func<Task> asyncBackAction)
        {
            Register(frame, asyncBackAction, new NavigationHelperConfig());
        }

        public static void Register(Frame frame, Func<Task> asyncBackAction, Func<Task> asyncForwardAction)
        {
            Register(frame, asyncBackAction, asyncForwardAction, new NavigationHelperConfig());
        }

        public static void Register(Frame frame, Func<Task> asyncBackAction, NavigationHelperConfig config)
        {
            Func<Task> asyncForwardAction = () =>
            {
                if (frame.CanGoForward)
                {
                    frame.GoForward();
                }
                return Task.FromResult<object>(null);
            };
            Register(frame, asyncBackAction, asyncForwardAction, config);
        }

        public static void Register(Frame frame, Action backAction)
        {
            Register(frame, backAction, new NavigationHelperConfig());
        }

        public static void Register(Frame frame, Action backAction, NavigationHelperConfig config)
        {
            Action forwardAction = () =>
            {
                if (frame.CanGoForward)
                {
                    frame.GoForward();
                }
            };
            Register(frame, backAction, forwardAction, config);
        }

        public static void Register(Frame frame, Action backAction, Action forwardAction)
        {
            Register(frame, backAction, forwardAction, new NavigationHelperConfig());
        }

        public static void Register(Frame frame, Action backAction, Action forwardAction, NavigationHelperConfig config)
        {
            Func<Task> asyncBackAction = () =>
            {
                if (backAction != null)
                {
                    backAction();
                }
                return Task.FromResult<object>(null);
            };
            Func<Task> asyncForwardAction = () =>
            {
                if (forwardAction != null)
                {
                    forwardAction();
                }
                return Task.FromResult<object>(null);
            };
            Register(frame, asyncBackAction, asyncForwardAction, config);
        }

        public static void Register(Frame frame, Func<Task> asyncBackAction, Func<Task> asyncForwardAction, NavigationHelperConfig config)
        {
            #region 标题栏后退键

            if (config.IsNavigateBackBySystemBackButton)
            {
                if (_systemNavigateBackHandlers.ContainsKey(frame) == false)
                {
                    var snm = SystemNavigationManager.GetForCurrentView();
                    snm.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    EventHandler<BackRequestedEventArgs> systemNavigateBackHandler = async (sender, e) =>
                    {
                        e.Handled = true;
                        if (asyncBackAction != null)
                        {
                            await asyncBackAction();
                        }
                    };
                    _systemNavigateBackHandlers.Add(frame, systemNavigateBackHandler);
                    snm.BackRequested += systemNavigateBackHandler;
                }
            }

            #endregion 标题栏后退键

            #region 设备后退键

            if (config.IsNavigateBackByHardwareBackButton)
            {
                if (HardwareButtonsHelper.IsUseable)
                {
                    if (_hardwareNavigateBackHandlers.ContainsKey(frame) == false)
                    {
                        EventHandler<BackPressedEventArgs> hardwareNavigateBackHandler = async (sender, e) =>
                        {
                            e.Handled = true;
                            if (asyncBackAction != null)
                            {
                                await asyncBackAction();
                            }
                        };
                        _hardwareNavigateBackHandlers.Add(frame, hardwareNavigateBackHandler);
                        HardwareButtons.BackPressed += hardwareNavigateBackHandler;
                    }
                }
            }

            #endregion 设备后退键

            CoreWindow window;
            if (_frameWindows.ContainsKey(frame))
            {
                window = _frameWindows[frame];
            }
            else
            {
                window = CoreWindow.GetForCurrentThread();
                _frameWindows[frame] = window;
            }

            #region 鼠标后退和前进

            if (config.IsNavigateBackByMouseXButton1 || config.IsNavigateForwardByMouseXButton2)
            {
                var isNavigateBackByMouseXButton1 = config.IsNavigateBackByMouseXButton1;
                var isNavigateForwardByMouseXButton2 = config.IsNavigateForwardByMouseXButton2;
                if (_mouseXButtonHandlers.ContainsKey(frame) == false)
                {
                    TypedEventHandler<CoreWindow, PointerEventArgs> mouseXButtonHandler = async (sender, e) =>
                    {
                        e.Handled = true;
                        switch (e.CurrentPoint.Properties.PointerUpdateKind)
                        {
                            case PointerUpdateKind.XButton1Released:
                                if (isNavigateBackByMouseXButton1)
                                {
                                    if (asyncBackAction != null)
                                    {
                                        await asyncBackAction();
                                    }
                                }
                                break;

                            case PointerUpdateKind.XButton2Released:
                                if (isNavigateForwardByMouseXButton2)
                                {
                                    if (asyncForwardAction != null)
                                    {
                                        await asyncForwardAction();
                                    }
                                }
                                break;
                        }
                    };
                    _mouseXButtonHandlers.Add(frame, mouseXButtonHandler);
                    window.PointerReleased += mouseXButtonHandler;
                }
            }

            #endregion 鼠标后退和前进

            #region 滑动后退和前进

            if (config.IsNavigateBackBySlideToRight || config.IsNavigateForwardBySlideToLeft)
            {
                var isNavigateBackBySlideToRight = config.IsNavigateBackBySlideToRight;
                var isNavigateForwardBySlideToLeft = config.IsNavigateForwardBySlideToLeft;

                var recognizer = new GestureRecognizer()
                {
                    GestureSettings = GestureSettings.CrossSlide,
                    CrossSlideHorizontally = true
                };
                PointerPoint startPoint = null;
                PointerPoint endPoint = null;
                if (_pointerPressedHandlers.ContainsKey(frame) == false)
                {
                    TypedEventHandler<CoreWindow, PointerEventArgs> pointerPressedHandler = (sender, e) =>
                    {
                        e.Handled = true;
                        switch (e.CurrentPoint.PointerDevice.PointerDeviceType)
                        {
                            case Windows.Devices.Input.PointerDeviceType.Touch:
                            case Windows.Devices.Input.PointerDeviceType.Pen:
                                var point = e.CurrentPoint;
                                startPoint = point;
                                recognizer.ProcessDownEvent(point);
                                break;
                        }
                    };
                    _pointerPressedHandlers.Add(frame, pointerPressedHandler);
                    window.PointerPressed += pointerPressedHandler;
                }
                if (_pointerMovedHandlers.ContainsKey(frame) == false)
                {
                    TypedEventHandler<CoreWindow, PointerEventArgs> pointerMovedHandler = (sender, e) =>
                    {
                        e.Handled = true;
                        switch (e.CurrentPoint.PointerDevice.PointerDeviceType)
                        {
                            case Windows.Devices.Input.PointerDeviceType.Touch:
                            case Windows.Devices.Input.PointerDeviceType.Pen:
                                recognizer.ProcessMoveEvents(e.GetIntermediatePoints());
                                break;
                        }
                    };
                    _pointerMovedHandlers.Add(frame, pointerMovedHandler);
                    window.PointerMoved += pointerMovedHandler;
                }
                if (_pointerReleasedHandlers.ContainsKey(frame) == false)
                {
                    TypedEventHandler<CoreWindow, PointerEventArgs> pointerReleasedHandler = (sender, e) =>
                    {
                        e.Handled = true;
                        switch (e.CurrentPoint.PointerDevice.PointerDeviceType)
                        {
                            case Windows.Devices.Input.PointerDeviceType.Touch:
                            case Windows.Devices.Input.PointerDeviceType.Pen:
                                var point = e.CurrentPoint;
                                endPoint = point;
                                recognizer.ProcessUpEvent(point);
                                break;
                        }
                    };
                    _pointerReleasedHandlers.Add(frame, pointerReleasedHandler);
                    window.PointerReleased += pointerReleasedHandler;
                }
                recognizer.CrossSliding += async (sender, e) =>
                {
                    if (e.CrossSlidingState == CrossSlidingState.Completed)
                    {
                        if (startPoint != null && endPoint != null)
                        {
                            var startX = startPoint.Position.X;
                            var endX = endPoint.Position.X;
                            if (startX < endX && isNavigateBackBySlideToRight)
                            {
                                if (asyncBackAction != null)
                                {
                                    await asyncBackAction();
                                }
                            }
                            if (startX > endX && isNavigateForwardBySlideToLeft)
                            {
                                if (asyncForwardAction != null)
                                {
                                    await asyncForwardAction();
                                }
                            }
                        }
                    }
                };
            }

            #endregion 滑动后退和前进
        }

        public static void Unregister(Frame frame)
        {
            #region 标题栏后退键

            if (_systemNavigateBackHandlers.ContainsKey(frame))
            {
                var snm = SystemNavigationManager.GetForCurrentView();
                snm.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                var systemNavigateBackHandler = _systemNavigateBackHandlers[frame];
                snm.BackRequested -= systemNavigateBackHandler;
                _systemNavigateBackHandlers.Remove(frame);
            }

            #endregion 标题栏后退键

            #region 设备后退键

            if (HardwareButtonsHelper.IsUseable)
            {
                if (_hardwareNavigateBackHandlers.ContainsKey(frame))
                {
                    var hardwareNavigateBackHandler = _hardwareNavigateBackHandlers[frame];
                    HardwareButtons.BackPressed -= hardwareNavigateBackHandler;
                    _hardwareNavigateBackHandlers.Remove(frame);
                }
            }

            #endregion 设备后退键

            CoreWindow window;
            if (_frameWindows.TryGetValue(frame, out window) == false)
            {
                return;
            }

            #region 鼠标后退和前进

            if (_mouseXButtonHandlers.ContainsKey(frame))
            {
                var mouseXButtonHandler = _mouseXButtonHandlers[frame];
                window.PointerReleased -= mouseXButtonHandler;
                _mouseXButtonHandlers.Remove(frame);
            }

            #endregion 鼠标后退和前进

            #region 滑动后退和前进

            if (_pointerPressedHandlers.ContainsKey(frame))
            {
                var pointerPressedHandler = _pointerPressedHandlers[frame];
                window.PointerPressed -= pointerPressedHandler;
                _pointerPressedHandlers.Remove(frame);
            }
            if (_pointerMovedHandlers.ContainsKey(frame))
            {
                var pointerMovedHandler = _pointerMovedHandlers[frame];
                window.PointerMoved -= pointerMovedHandler;
                _pointerMovedHandlers.Remove(frame);
            }
            if (_pointerReleasedHandlers.ContainsKey(frame))
            {
                var pointerReleasedHandler = _pointerReleasedHandlers[frame];
                window.PointerReleased -= pointerReleasedHandler;
                _pointerReleasedHandlers.Remove(frame);
            }

            #endregion 滑动后退和前进

            _frameWindows.Remove(frame);
        }
    }
}