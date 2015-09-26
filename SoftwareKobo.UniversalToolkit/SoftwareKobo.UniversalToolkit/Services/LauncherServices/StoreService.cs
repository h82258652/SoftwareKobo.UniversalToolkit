using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.Services.LauncherServices
{
    /// <summary>
    /// 商店服务。
    /// </summary>
    /// <remarks>
    /// see https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
    /// </remarks>
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

        public async Task OpenAppsPageWithCategoryAsync(string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://browse/?type=Apps&cat=" + category));
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

        public async Task OpenGamesPageWithCategoryAsync(string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://browse/?type=Games&cat=" + category));
        }

        public async Task OpenProductsPageByPublisherAsync(string publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException(nameof(publisher));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://publisher/?name=" + publisher));
        }

        public async Task OpenSearchPageByFileExtensionAsync(string fileExtension)
        {
            if (fileExtension == null)
            {
                throw new ArgumentNullException(nameof(fileExtension));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://assoc/?FileExt=" + fileExtension));
        }

        public async Task OpenSearchPageByProtocolAsync(string protocol)
        {
            if (protocol == null)
            {
                throw new ArgumentNullException(nameof(protocol));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://assoc/?Protocol=" + protocol));
        }

        public async Task OpenSearchPageByQueryAsync(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://search/?query=" + query));
        }

        public async Task OpenSearchPageByTagsAsync(params string[] tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }
            if (tags.Length == 0)
            {
                throw new ArgumentException("at least one tag!", nameof(tags));
            }

            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://assoc/?Tags=" + string.Join(",", tags)));
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