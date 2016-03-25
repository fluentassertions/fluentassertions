using FluentAssertions.Common;
using FluentAssertions.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FluentAssertions.Xml
{
    public class XmlNodeFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is XmlNode;
        }

        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            string outerXml = ((XmlNode)value).OuterXml;

            int maxLength = 20;

            if(outerXml.Length > maxLength)
            {
                outerXml = outerXml.Substring(0, maxLength).TrimEnd() + "...";
            }

            return outerXml.Escape(escapePlaceholders: true);
        }
    }
}
