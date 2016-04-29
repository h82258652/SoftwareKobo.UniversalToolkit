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

        public static PackageManifest Current => Package.Current.Manifest();

        public IEnumerable<ApplicationManifest> Applications
        {
            get
            {
                var applications = GetChildElement("Applications");
                if (applications != null)
                {
                    foreach (var application in applications.Elements())
                    {
                        yield return new ApplicationManifest(application);
                    }
                }
            }
        }

        public string IgnorableNamespaces => this["IgnorableNamespaces"];

        public IEnumerable<ResourceManifest> Resources
        {
            get
            {
                var resources = GetChildElement("Resources");
                foreach (var resource in resources.Elements())
                {
                    yield return new ResourceManifest(resource);
                }
            }
        }

        private static XDocument LoadDocument(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var manifestPath = Path.Combine(package.InstalledLocation.Path, "AppxManifest.xml");
            var document = XDocument.Load(manifestPath);
            return document;
        }
    }
}