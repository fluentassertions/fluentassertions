#if NET45 || NETSTANDARD2_0

using FluentAssertions.Common;
using FluentAssertions.Formatting;
using System.Collections.Generic;
using System.Xml;

namespace FluentAssertions.Xml
{
    public class XmlNodeFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is XmlNode;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            string outerXml = ((XmlNode)value).OuterXml;

            int maxLength = 20;

            if (outerXml.Length > maxLength)
            {
                outerXml = outerXml.Substring(0, maxLength).TrimEnd() + "…";
            }

            return outerXml.Escape(escapePlaceholders: true);
        }
    }
}

#endif
