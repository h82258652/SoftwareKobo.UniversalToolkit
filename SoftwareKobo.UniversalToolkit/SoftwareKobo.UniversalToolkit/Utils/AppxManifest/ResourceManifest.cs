using System;
using System.Xml.Linq;
using Windows.Graphics.Display;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class ResourceManifest : ManifestBase
    {
        internal ResourceManifest(XElement resourceElement) : base(resourceElement)
        {
        }

        public string Language => this["Language"];

        public string DXFeatureLevel => this["uap:DXFeatureLevel"];

        public ResolutionScale Scale
        {
            get
            {
                var value = this["uap:Scale"];
                return (ResolutionScale)Enum.Parse(typeof(ResolutionScale), value);
            }
        }
    }
}