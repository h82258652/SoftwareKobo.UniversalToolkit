using SoftwareKobo.UniversalToolkit.Extensions;
using System.Xml.Linq;
using Windows.UI;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class VisualElementsManifest : ManifestBase
    {
        internal VisualElementsManifest(XElement visualElementsElement) : base(visualElementsElement)
        {
        }

        public Color BackgroundColor
        {
            get
            {
                var value = this["BackgroundColor"];
                return ColorExtensions.Parse(value);
            }
        }

        public DefaultTileManifest DefaultTileElement
        {
            get
            {
                var defaultTileElement = GetChildElement("uap:DefaultTileElement");
                if (defaultTileElement == null)
                {
                    return null;
                }
                else
                {
                    return new DefaultTileManifest(defaultTileElement);
                }
            }
        }

        public string Description => this["Description"];

        public string DisplayName => this["DisplayName"];

        public SplashScreenManifest SplashScreen
        {
            get
            {
                var splashScreen = GetChildElement("uap:SplashScreen");
                if (splashScreen == null)
                {
                    return null;
                }
                else
                {
                    return new SplashScreenManifest(splashScreen);
                }
            }
        }

        public string Square150x150Logo => this["Square150x150Logo"];

        public string Square44x44Logo => this["Square44x44Logo"];
    }
}