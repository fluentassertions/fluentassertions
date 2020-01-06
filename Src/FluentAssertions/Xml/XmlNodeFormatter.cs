#if !NETSTANDARD1_3 && !NETSTANDARD1_6

using System.Xml;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

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

            const int maxLength = 20;

            if (outerXml.Length > maxLength)
            {
                outerXml = outerXml.Substring(0, maxLength).TrimEnd() + "…";
            }

            return outerXml.EscapePlaceholders();
        }
    }
}

#endif
