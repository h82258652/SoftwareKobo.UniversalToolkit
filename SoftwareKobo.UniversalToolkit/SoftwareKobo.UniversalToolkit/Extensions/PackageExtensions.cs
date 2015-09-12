using SoftwareKobo.UniversalToolkit.Utils.AppxManifest;
using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class PackageExtensions
    {
        public static PackageManifest Manifest(this Package package)
        {
            string manifestPath = Path.Combine(package.InstalledLocation.Path, "AppxManifest.xml");
            XDocument document = XDocument.Load(manifestPath);
            return new PackageManifest(document.Root);
        }
    }
}