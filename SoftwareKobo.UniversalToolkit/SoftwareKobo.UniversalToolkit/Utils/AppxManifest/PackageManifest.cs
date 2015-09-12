using SoftwareKobo.UniversalToolkit.Extensions;
using System.Collections.Generic;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class PackageManifest : ManifestBase
    {
        internal PackageManifest(XElement packageElement) : base(packageElement)
        {
        }

        public static PackageManifest Current
        {
            get
            {
                return Package.Current.Manifest();
            }
        }

        public string IgnorableNamespaces
        {
            get
            {
                return this["IgnorableNamespaces"];
            }
        }

        public IEnumerable<ApplicationManifest> Applications
        {
            get
            {
                XElement applications = this.GetChildElement("Applications");
                if (applications != null)
                {
                    foreach (XElement application in applications.Elements())
                    {
                        yield return new ApplicationManifest(application);
                    }
                }
            }
        }
    }
}