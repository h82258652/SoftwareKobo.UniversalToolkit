using Windows.System.Profile;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public static class DeviceFamilyHelper
    {
        public static DeviceFamily DeviceFamily
        {
            get
            {
                switch (AnalyticsInfo.VersionInfo.DeviceFamily)
                {
                    case "Windows.Desktop":
                        return DeviceFamily.Desktop;

                    case "Windows.Mobile":
                        return DeviceFamily.Mobile;

                    case "Windows.Team":
                        return DeviceFamily.Team;

                    case "Windows.IoT":
                        return DeviceFamily.IoT;

                    case "Windows.Xbox":
                        return DeviceFamily.Xbox;

                    default:
                        return DeviceFamily.Unknown;
                }
            }
        }

        public static bool IsDesktop => DeviceFamily == DeviceFamily.Desktop;

        public static bool IsIoT => DeviceFamily == DeviceFamily.IoT;

        public static bool IsMobile => DeviceFamily == DeviceFamily.Mobile;

        public static bool IsTeam => DeviceFamily == DeviceFamily.Team;

        public static bool IsUnknown => DeviceFamily == DeviceFamily.Unknown;

        public static bool IsXbox => DeviceFamily == DeviceFamily.Xbox;
    }
}