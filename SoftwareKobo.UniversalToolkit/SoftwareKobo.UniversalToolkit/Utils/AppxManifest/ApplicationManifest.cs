using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class ApplicationManifest : ManifestBase
    {
        internal ApplicationManifest(XElement applicationElement) : base(applicationElement)
        {
        }

        public string EntryPoint
        {
            get
            {
                return this["EntryPoint"];
            }
        }

        public string Executable
        {
            get
            {
                return this["Executable"];
            }
        }

        public string Id
        {
            get
            {
                return this["Id"];
            }
        }

        public string StartPage
        {
            get
            {
                return this["StartPage"];
            }
        }

        public VisualElementsManifest VisualElements
        {
            get
            {
                return new VisualElementsManifest(this.GetChildElement("uap:VisualElements"));
            }
        }
    }
}