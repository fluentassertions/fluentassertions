using System;

namespace FluentAssertions.Xml.Equivalency
{
    internal class AttributeData
    {
        public AttributeData(string namespaceUri, string localName, string value, string prefix)
        {
            NamespaceUri = namespaceUri;
            LocalName = localName;
            Value = value;
            Prefix = prefix;
        }

        public string NamespaceUri { get; }

        public string LocalName { get; }

        public string Value { get; }

        private string Prefix { get; }

        public string QualifiedName
        {
            get
            {
                if (string.IsNullOrEmpty(Prefix))
                {
                    return LocalName;
                }

                return Prefix + ":" + LocalName;
            }
        }
    }
}
