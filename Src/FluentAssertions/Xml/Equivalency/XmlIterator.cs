using System.Collections.Generic;
using System.Xml;

namespace FluentAssertions.Xml.Equivalency
{
    internal class XmlIterator
    {
        private readonly XmlReader reader;
        private bool skipOnce;

        public XmlIterator(XmlReader reader)
        {
            this.reader = reader;

            this.reader.MoveToContent();
        }

        public XmlNodeType NodeType => reader.NodeType;

        public string LocalName => reader.LocalName;

        public string NamespaceUri => reader.NamespaceURI;

        public string Value => reader.Value;

        public bool IsEmptyElement => reader.IsEmptyElement;

        public bool IsEndOfDocument => reader.EOF;

        public void Read()
        {
            if (skipOnce)
            {
                skipOnce = false;
            }
            else if (reader.Read())
            {
                reader.MoveToContent();
            }
        }

        public void MoveToEndElement()
        {
            reader.Read();
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                // advancing failed
                // skip reading on next attempt to "simulate" rewind reader
                skipOnce = true;
            }

            reader.MoveToContent();
        }

        public IList<AttributeData> GetAttributes()
        {
            var attributes = new List<AttributeData>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    if (reader.NamespaceURI != "http://www.w3.org/2000/xmlns/")
                    {
                        attributes.Add(new AttributeData(reader.NamespaceURI, reader.LocalName, reader.Value, reader.Prefix));
                    }
                }
                while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }

            return attributes;
        }
    }
}
