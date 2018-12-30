using System;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class XElementValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return (value is XElement);
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var element = (XElement)value;

            return element.HasElements
                ? FormatElementWithChildren(element)
                : FormatElementWithoutChildren(element);
        }

        private static string FormatElementWithoutChildren(XElement element)
        {
            return element.ToString().EscapePlaceholders();
        }

        private static string FormatElementWithChildren(XElement element)
        {
            string[] lines = SplitIntoSeparateLines(element);

            // Can't use env.newline because the input doc may have unix or windows style
            // line-breaks
            string firstLine = lines.First().RemoveNewLines();
            string lastLine = lines.Last().RemoveNewLines();

            string formattedElement = firstLine + "…" + lastLine;
            return formattedElement.EscapePlaceholders();
        }

        private static string[] SplitIntoSeparateLines(XElement element)
        {
            string formattedXml = element.ToString();
            return formattedXml.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
