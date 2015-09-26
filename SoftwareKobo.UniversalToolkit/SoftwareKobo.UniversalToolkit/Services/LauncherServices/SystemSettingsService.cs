using SoftwareKobo.UniversalToolkit.Helpers;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.Services.LauncherServices
{
    /// <summary>
    /// 系统设置服务。
    /// </summary>
    /// <remarks>
    /// see https://msdn.microsoft.com/en-us/library/windows/apps/mt228342.aspx
    /// </remarks>
    public class SystemSettingsService
    {
        public async Task OpenHomePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:"));
        }

        public async Task OpenDisplayPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:display"));
        }

        public async Task OpenNotificationsPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:notifications"));
        }

        public async Task OpenStoragePageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:storagesense"));
        }

        public async Task OpenBatterySaverPageAsync()
        {
            // ms-settings:batterysaver
            // ms-settings:batterysaver-settings
            // ms-settings:batterysaver-usagedetails
            await Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver"));
        }

        public async Task OpenOfflineMapsPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:maps"));
        }

        public async Task OpenBluetoothPageAsync()
        {
            if (DeviceFamilyHelper.IsMobile)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings:bluetooth"));
            }
        }

        public async Task OpenConnectedDevicesPageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:connecteddevices"));
        }

        public async Task OpenMouseAndTouchpadPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:mousetouchpad"));
        }

        public async Task OpenWiFiPageAsync()
        {
            if (DeviceFamilyHelper.IsMobile)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi"));
            }
        }

        public async Task OpenAirplaneModePageAsync()
        {
            if (DeviceFamilyHelper.IsMobile)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings-airplanemode:"));
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings:network-airplanemode"));
            }
        }

        public async Task OpenCellularPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:network-cellular"));
        }

        public async Task OpenDialupPageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:network-dialup"));
        }

        public async Task OpenEthernetPageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:network-ethernet"));
        }

        public async Task OpenProxyPageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:network-proxy"));
        }

        public async Task OpenDataSensePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:datausage"));
        }

        public async Task OpenMobileHotspotPageAsync()
        {
            if (DeviceFamilyHelper.IsMobile)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings-mobilehotspot:"));
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings:network-mobilehotspot"));
            }
        }

        public async Task OpenLockScreenPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:lockscreen"));
        }

        public async Task OpenPersonalizationPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:personalization"));
        }

        public async Task OpenYourAccountPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:emailandaccounts"));
        }

        public async Task OpenYourWorkplacePageAsync()
        {
            if (DeviceFamilyHelper.IsMobile)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings-workplace:"));
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri("ms-settings:workplace"));
            }
        }

        public async Task OpenDateAndTimePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:dateandtime"));
        }

        public async Task OpenRegionAndLanguagePageAsync()
        {
            // Desktop only
            await Launcher.LaunchUriAsync(new Uri("ms-settings:regionlanguage"));
        }

        public async Task OpenSpeechPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:speech"));
        }

        public async Task OpenCalendarPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-calendar"));
        }

        public async Task OpenContactsPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-contacts"));
        }

        public async Task OpenFeedbackAndDiagnosticsPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-feedback"));
        }

        public async Task OpenLocationPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
        }

        public async Task OpenMessagingPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-messaging"));
        }

        public async Task OpenMicrophonePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-microphone"));
        }

        public async Task OpenOtherPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-customdevices"));
        }

        public async Task OpenRadioPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-radios"));
        }

        public async Task OpenSpeechTypingPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-speechtyping"));
        }

        public async Task OpenWebcamPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-webcam"));
        }

        public async Task OpenWindowsUpdatePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:windowsupdate"));
        }
    }
}