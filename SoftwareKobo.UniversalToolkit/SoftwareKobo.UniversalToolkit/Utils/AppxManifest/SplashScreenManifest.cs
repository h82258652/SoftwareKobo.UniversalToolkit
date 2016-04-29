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
                var value = this["BackgroundColor"];
                return ColorExtensions.TryParse(value);
            }
        }

        public string Image => this["Image"];
    }
}