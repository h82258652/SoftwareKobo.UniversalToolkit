using Windows.System.Profile;
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
            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Desktop":
                    obj.SetActive(value == DeviceFamily.Desktop);
                    break;

                case "Windows.Mobile":
                    obj.SetActive(value == DeviceFamily.Mobile);
                    break;

                case "Windows.Team":
                    obj.SetActive(value == DeviceFamily.Team);
                    break;

                case "Windows.IoT":
                    obj.SetActive(value == DeviceFamily.IoT);
                    break;

                case "Windows.Xbox":
                    obj.SetActive(value == DeviceFamily.Xbox);
                    break;

                default:
                    obj.SetActive(value == DeviceFamily.Unknown);
                    break;
            }
        }
    }
}