using System;
using System.Collections.Generic;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public static class NavigationBackHelper
    {
        private static readonly Dictionary<Frame, EventHandler<BackRequestedEventArgs>> _backRequestedHandlers = new Dictionary<Frame, EventHandler<BackRequestedEventArgs>>();
        private static readonly Dictionary<Frame, EventHandler<BackPressedEventArgs>> _backPressedHandlers = new Dictionary<Frame, EventHandler<BackPressedEventArgs>>();

        public static void RegisterNavigateBack(this Frame frame)
        {
            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            if (_backRequestedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackRequestedEventArgs> backRequestedHandler = (sender, e) =>
                {
                    if (frame.CanGoBack)
                    {
                        e.Handled = true;
                        frame.GoBack();
                    }
                };
                _backRequestedHandlers.Add(frame, backRequestedHandler);
                systemNavigationManager.BackRequested += backRequestedHandler;
            }

            if (HardwareButtonsHelper.IsUseable && _backPressedHandlers.ContainsKey(frame) == false)
            {
                EventHandler<BackPressedEventArgs> backPressedHandler = (sender, e) =>
                {
                    if (frame.CanGoBack)
                    {
                        e.Handled = true;
                        frame.GoBack();
                    }
                };
                _backPressedHandlers.Add(frame, backPressedHandler);
                HardwareButtons.BackPressed += backPressedHandler;
            }
        }

        public static void UnRegisterNavigateBack(this Frame frame)
        {
            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            if (_backRequestedHandlers.ContainsKey(frame))
            {
                EventHandler<BackRequestedEventArgs> backRequestedHandler = _backRequestedHandlers[frame];
                systemNavigationManager.BackRequested -= backRequestedHandler;
                _backRequestedHandlers.Remove(frame);
            }

            if (HardwareButtonsHelper.IsUseable && _backPressedHandlers.ContainsKey(frame))
            {
                EventHandler<BackPressedEventArgs> backPressedHandler = _backPressedHandlers[frame];
                HardwareButtons.BackPressed -= backPressedHandler;
                _backPressedHandlers.Remove(frame);
            }
        }
    }
}