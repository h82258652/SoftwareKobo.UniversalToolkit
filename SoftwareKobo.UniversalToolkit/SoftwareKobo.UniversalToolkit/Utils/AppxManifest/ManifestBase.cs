using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public abstract class ManifestBase
    {
        private XElement _manifestElement;

        protected internal ManifestBase(XElement manifestElement)
        {
            _manifestElement = manifestElement;
        }

        public XElement Element => _manifestElement;

        public string this[string attribute]
        {
            get
            {
                XName attributeName;
                var splitIndex = attribute.IndexOf(':');
                if (splitIndex >= 0)
                {
                    var prefix = attribute.Substring(0, splitIndex);
                    var localName = attribute.Substring(splitIndex + 1);
                    attributeName = XName.Get(localName, _manifestElement.GetNamespaceOfPrefix(prefix).NamespaceName);
                }
                else
                {
                    attributeName = XName.Get(attribute);
                }

                var xAttribute = _manifestElement.Attribute(attributeName);
                return xAttribute?.Value;
            }
        }

        public override string ToString()
        {
            return _manifestElement.ToString();
        }

        protected internal XElement GetChildElement(string element)
        {
            XName elementName;
            var splitIndex = element.IndexOf(':');
            if (splitIndex >= 0)
            {
                var prefix = element.Substring(0, splitIndex);
                var localName = element.Substring(splitIndex + 1);
                elementName = XName.Get(localName, _manifestElement.GetNamespaceOfPrefix(prefix).NamespaceName);
            }
            else
            {
                elementName = XName.Get(element, _manifestElement.GetDefaultNamespace().NamespaceName);
            }

            var childElement = _manifestElement.Element(elementName);
            return childElement;
        }
    }
}