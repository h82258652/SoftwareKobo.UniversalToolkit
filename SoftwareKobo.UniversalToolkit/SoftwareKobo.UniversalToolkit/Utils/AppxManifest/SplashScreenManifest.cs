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
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                if (value.StartsWith("#"))
                {
                    return ColorExtensions.FromHex(value);
                }
                else
                {
                    return ColorExtensions.FromName(value);
                }
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