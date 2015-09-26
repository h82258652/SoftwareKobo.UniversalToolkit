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
                return (DeviceFamily)this.GetValue(DeviceFamilyProperty);
            }
            set
            {
                this.SetValue(DeviceFamilyProperty, value);
            }
        }

        private static void DeviceFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DeviceFamilyTrigger obj = (DeviceFamilyTrigger)d;
            DeviceFamily value = (DeviceFamily)e.NewValue;
            obj.SetActive(value == DeviceFamilyHelper.DeviceFamily);
        }
    }
}