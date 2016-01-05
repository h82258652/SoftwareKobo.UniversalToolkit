using System;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Triggers
{
    public class InputDeviceTypeTrigger : StateTriggerBase
    {
        public readonly static DependencyProperty PointerTypeProperty = DependencyProperty.Register(nameof(InputDeviceType), typeof(PointerDeviceType), typeof(InputDeviceTypeTrigger), new PropertyMetadata((PointerDeviceType)(-1)));

        public InputDeviceTypeTrigger()
        {
            var coreWindow = Window.Current.CoreWindow;
            coreWindow.PointerPressed += CoreWindow_PointerEvent;
            coreWindow.PointerMoved += CoreWindow_PointerEvent;

            if (Bootstrapper.Current != null)
            {
                Bootstrapper.Current.WindowCreated += App_WindowCreated;
            }
        }

        public PointerDeviceType InputDeviceType
        {
            get
            {
                return (PointerDeviceType)GetValue(PointerTypeProperty);
            }
            set
            {
                SetValue(PointerTypeProperty, value);
            }
        }

        private void App_WindowCreated(object sender, WindowCreatedEventArgs e)
        {
            var coreWindow = e.Window.CoreWindow;
            coreWindow.PointerPressed += CoreWindow_PointerEvent;
            coreWindow.PointerMoved += CoreWindow_PointerEvent;
        }

        private void CoreWindow_PointerEvent(CoreWindow sender, PointerEventArgs args)
        {
            if (Enum.IsDefined(typeof(PointerDeviceType), InputDeviceType))
            {
                SetActive(args.CurrentPoint.PointerDevice.PointerDeviceType == InputDeviceType);
            }
        }
    }
}