using System.Xml.Linq;

namespace FluentAssertions.Formatting
{
    public class XDocumentValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is XDocument;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var document = (XDocument)value;

            return (document.Root is not null)
                ? formatChild("root", document.Root)
                : FormatDocumentWithoutRoot();
        }

        private static string FormatDocumentWithoutRoot()
        {
            return "[XML document without root element]";
        }
    }
}
