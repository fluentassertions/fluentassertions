using System.Xml.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class XDocumentValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is XDocument;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            Guard.ThrowIfArgumentIsNull(value, nameof(value));
            Guard.ThrowIfArgumentIsNull(formattedGraph, nameof(formattedGraph));
            Guard.ThrowIfArgumentIsNull(formatChild, nameof(formatChild));

            var document = (XDocument)value;

            if (document.Root is not null)
            {
                formatChild("root", document.Root, formattedGraph);
            }
            else
            {
                formattedGraph.AddFragment("[XML document without root element]");
            }
        }
    }
}
