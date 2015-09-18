using SoftwareKobo.UniversalToolkit.Extensions;
using System.Xml.Linq;
using Windows.UI;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class SplashScreenManifest : ManifestBase
    {
        internal SplashScreenManifest(XElement splashScreenElement) : base(splashScreenElement)
        {
        }

        public Color? BackgroundColor
        {
            get
            {
                string value = this["BackgroundColor"];
                return ColorExtensions.TryParse(value);
            }
        }

        public string Image
        {
            get
            {
                return this["Image"];
            }
        }
    }
}