using System;
using System.Collections.Generic;
using System.Xml.Linq;

using FluentAssertions.Common;

using System.Linq;

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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            var element = (XElement) value;

            return element.HasElements
                ? FormatElementWithChildren(element)
                : FormatElementWithoutChildren(element);
        }

        private static string FormatElementWithoutChildren(XElement element)
        {
            return element.ToString().Escape(escapePlaceholders: true);
        }

        private static string FormatElementWithChildren(XElement element)
        {
            string [] lines = SplitIntoSeparateLines(element);

            // Can't use env.newline because the input doc may have unix or windows style
            // line-breaks
            string firstLine = lines.First().RemoveNewLines();
            string lastLine = lines.Last().RemoveNewLines();

            string formattedElement = firstLine + "..." + lastLine;
            return formattedElement.Escape(escapePlaceholders: true);
        }

        private static string [] SplitIntoSeparateLines(XElement element)
        {
            string formattedXml = element.ToString();
            return formattedXml.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}