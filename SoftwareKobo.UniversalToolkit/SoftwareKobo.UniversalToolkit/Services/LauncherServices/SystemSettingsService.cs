using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.Services.LauncherServices
{
    // https://msdn.microsoft.com/en-us/library/windows/apps/mt228342.aspx
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
    }
}
