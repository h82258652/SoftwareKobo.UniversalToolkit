using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class ApplicationManifest : ManifestBase
    {
        internal ApplicationManifest(XElement applicationElement) : base(applicationElement)
        {
        }

        public string EntryPoint => this["EntryPoint"];

        public string Executable => this["Executable"];

        public string Id => this["Id"];

        public string StartPage => this["StartPage"];

        public VisualElementsManifest VisualElements => new VisualElementsManifest(GetChildElement("uap:VisualElements"));
    }
}