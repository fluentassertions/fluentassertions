using System.Xml.Linq;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class XDocumentValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is XDocument);
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="uniqueObjectTracker">
        /// An object that is passed through recursive calls and which should be used to detect circular references
        /// in the object graph that is being converted to a string representation.</param>
        /// <param name="nestedPropertyLevel">
        ///     The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        ///     no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, UniqueObjectTracker uniqueObjectTracker = null, int nestedPropertyLevel = 0)
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