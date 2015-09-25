using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.Services.LauncherServices
{
    // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
    public class StoreService
    {
        public async Task OpenAppDetailPageByPFNAsync(string packageFamilyName)
        {
            if (packageFamilyName == null)
            {
                throw new ArgumentNullException(nameof(packageFamilyName));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?PFN=" + packageFamilyName));
        }

        public async Task OpenAppDetailPageByProductIdAsync(string productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?ProductId=" + productId));
        }

        public async Task OpenAppReviewPageByPFNAsync(string packageFamilyName)
        {
            if (packageFamilyName == null)
            {
                throw new ArgumentNullException(nameof(packageFamilyName));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?PFN=" + packageFamilyName));
        }

        public async Task OpenAppReviewPageByProductIdAsync(string productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=" + productId));
        }

        public async Task OpenCurrentAppDetailPageAsync()
        {
            await OpenAppDetailPageByPFNAsync(Package.Current.Id.FamilyName);
        }

        public async Task OpenCurrentAppReviewPageAsync()
        {
            await OpenAppReviewPageByPFNAsync(Package.Current.Id.FamilyName);
        }

        public async Task OpenDownloadAndUpdatePageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://downloadsandupdates"));
        }

        public async Task OpenSettingsPageAsync()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://settings"));
        }

        public async Task OpenStorePageAsync(StorePage storePage)
        {
            if (Enum.IsDefined(typeof(StorePage), storePage) == false)
            {
                throw new ArgumentException("store page is not defined.", nameof(storePage));
            }

            switch (storePage)
            {
                case StorePage.Home:
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store://home"));
                    break;

                case StorePage.Apps:
                case StorePage.Games:
                case StorePage.Music:
                case StorePage.Video:
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store://navigatetopage/?Id=" + storePage.ToString()));
                    break;

                default:
                    break;
            }
        }
    }
}