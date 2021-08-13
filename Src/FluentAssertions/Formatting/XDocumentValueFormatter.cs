using System.Xml.Linq;

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
