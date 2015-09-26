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

        public static bool IsDesktop
        {
            get
            {
                return DeviceFamily == DeviceFamily.Desktop;
            }
        }

        public static bool IsIoT
        {
            get
            {
                return DeviceFamily == DeviceFamily.IoT;
            }
        }

        public static bool IsMobile
        {
            get
            {
                return DeviceFamily == DeviceFamily.Mobile;
            }
        }

        public static bool IsTeam
        {
            get
            {
                return DeviceFamily == DeviceFamily.Team;
            }
        }

        public static bool IsUnknown
        {
            get
            {
                return DeviceFamily == DeviceFamily.Unknown;
            }
        }

        public static bool IsXbox
        {
            get
            {
                return DeviceFamily == DeviceFamily.Xbox;
            }
        }
    }
}