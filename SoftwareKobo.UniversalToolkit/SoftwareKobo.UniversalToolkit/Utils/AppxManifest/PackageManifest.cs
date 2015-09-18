using SoftwareKobo.UniversalToolkit.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public sealed class PackageManifest : ManifestBase
    {
        public PackageManifest(Package package) : base(LoadDocument(package).Root)
        {
        }

        public static PackageManifest Current
        {
            get
            {
                return Package.Current.Manifest();
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

        public string IgnorableNamespaces
        {
            get
            {
                return this["IgnorableNamespaces"];
            }
        }

        private static XDocument LoadDocument(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            string manifestPath = Path.Combine(package.InstalledLocation.Path, "AppxManifest.xml");
            XDocument document = XDocument.Load(manifestPath);
            return document;
        }
    }
}