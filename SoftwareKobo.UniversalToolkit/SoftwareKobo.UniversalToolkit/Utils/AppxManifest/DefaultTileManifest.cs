using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class DefaultTileManifest : ManifestBase
    {
        internal DefaultTileManifest(XElement defaultTileElement) : base(defaultTileElement)
        {
        }

        public string Square310x310Logo => this["Square310x310Logo"];

        public string Square71x71Logo => this["Square71x71Logo"];

        public string Wide310x150Logo => this["Wide310x150Logo"];
    }
}