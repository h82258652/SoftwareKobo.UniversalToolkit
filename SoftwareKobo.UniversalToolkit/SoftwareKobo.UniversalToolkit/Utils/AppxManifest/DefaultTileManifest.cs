using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class DefaultTileManifest : ManifestBase
    {
        internal DefaultTileManifest(XElement defaultTileElement) : base(defaultTileElement)
        {
        }

        public string Square310x310Logo
        {
            get
            {
                return this["Square310x310Logo"];
            }
        }

        public string Square71x71Logo
        {
            get
            {
                return this["Square71x71Logo"];
            }
        }

        public string Wide310x150Logo
        {
            get
            {
                return this["Wide310x150Logo"];
            }
        }
    }
}