using System.Xml.Linq;

namespace SoftwareKobo.UniversalToolkit.Utils.AppxManifest
{
    public abstract class ManifestBase
    {
        private XElement _manifestElement;

        protected internal ManifestBase(XElement manifestElement)
        {
            this._manifestElement = manifestElement;
        }

        public XElement Element
        {
            get
            {
                return this._manifestElement;
            }
        }

        public string this[string attribute]
        {
            get
            {
                XName attributeName;
                int splitIndex = attribute.IndexOf(':');
                if (splitIndex >= 0)
                {
                    string prefix = attribute.Substring(0, splitIndex);
                    string localName = attribute.Substring(splitIndex + 1);
                    attributeName = XName.Get(localName, this._manifestElement.GetNamespaceOfPrefix(prefix).NamespaceName);
                }
                else
                {
                    attributeName = XName.Get(attribute);
                }

                XAttribute xAttribute = this._manifestElement.Attribute(attributeName);
                return xAttribute?.Value;
            }
        }

        public override string ToString()
        {
            return this._manifestElement.ToString();
        }

        protected internal XElement GetChildElement(string element)
        {
            XName elementName;
            int splitIndex = element.IndexOf(':');
            if (splitIndex >= 0)
            {
                string prefix = element.Substring(0, splitIndex);
                string localName = element.Substring(splitIndex + 1);
                elementName = XName.Get(localName, this._manifestElement.GetNamespaceOfPrefix(prefix).NamespaceName);
            }
            else
            {
                elementName = XName.Get(element, this._manifestElement.GetDefaultNamespace().NamespaceName);
            }
            
            XElement childElement = this._manifestElement.Element(elementName);
            return childElement;
        }
    }
}