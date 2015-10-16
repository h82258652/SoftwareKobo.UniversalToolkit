using Windows.Foundation.Metadata;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public static class StatusBarHelper
    {
        public static bool IsUseable
        {
            get
            {
                return ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
            }
        }
    }
}