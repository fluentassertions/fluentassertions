using System.Xml.Linq;

namespace FluentAssertions.Formatting
{
    internal class XDocumentValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is XDocument);
        }

        public string ToString(object value, int nestedPropertyLevel = 0)
        {
            var document = (XDocument) value;

            return (document.Root != null)
                ? FormatDocumentWithRoot(document)
                : FormatDocumentWithoutRoot();
        }

        private string FormatDocumentWithRoot(XDocument document)
        {
            return Formatter.ToString(document.Root);
        }

        private string FormatDocumentWithoutRoot()
        {
            return "[XML document without root element]";
        }
    }
}