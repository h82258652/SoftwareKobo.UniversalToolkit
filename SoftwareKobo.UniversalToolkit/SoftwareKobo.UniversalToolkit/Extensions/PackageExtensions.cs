using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using Windows.ApplicationModel;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class PackageExtensions
    {
        public static PackageManifest Manifest(this Package package)
        {
            return new PackageManifest(package);
        }
    }
}