using Windows.Foundation.Metadata;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public static class HardwareButtonsHelper
    {
        public static bool IsUseable => ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
    }
}