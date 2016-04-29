using SoftwareKobo.UniversalToolkit.Helpers;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Triggers
{
    public class DeviceFamilyTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty DeviceFamilyProperty =
            DependencyProperty.Register(nameof(DeviceFamily), typeof(DeviceFamily), typeof(DeviceFamilyTrigger), new PropertyMetadata(DeviceFamily.Unknown, DeviceFamilyChanged));

        public DeviceFamily DeviceFamily
        {
            get
            {
                return (DeviceFamily)GetValue(DeviceFamilyProperty);
            }
            set
            {
                SetValue(DeviceFamilyProperty, value);
            }
        }

        private static void DeviceFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DeviceFamilyTrigger)d;
            var value = (DeviceFamily)e.NewValue;
            obj.SetActive(value == DeviceFamilyHelper.DeviceFamily);
        }
    }
}