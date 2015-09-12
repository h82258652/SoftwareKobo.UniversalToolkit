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
                string value = this["BackgroundColor"];
                if (value.StartsWith("#"))
                {
                    return ColorExtensions.FromHex(value);
                }
                else
                {
                    return ColorExtensions.FromName(value).Value;
                }
            }
        }

        public DefaultTileManifest DefaultTileElement
        {
            get
            {
                XElement defaultTileElement = this.GetChildElement("uap:DefaultTileElement");
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

        public string Description
        {
            get
            {
                return this["Description"];
            }
        }

        public string DisplayName
        {
            get
            {
                return this["DisplayName"];
            }
        }

        public SplashScreenManifest SplashScreen
        {
            get
            {
                XElement splashScreen = this.GetChildElement("uap:SplashScreen");
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

        public string Square150x150Logo
        {
            get
            {
                return this["Square150x150Logo"];
            }
        }

        public string Square44x44Logo
        {
            get
            {
                return this["Square44x44Logo"];
            }
        }
    }
}