using System;
using System.Collections.Generic;
using System.Xml.Linq;

using FluentAssertions.Common;

using System.Linq;

namespace FluentAssertions.Formatting
{
    internal class XElementValueFormatter : IValueFormatter
    {
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
            return element.ToString().Escape();
        }

        private static string FormatElementWithChildren(XElement element)
        {
            string [] lines = SplitIntoSeparateLines(element);

            string firstLine = lines.First().Replace(Environment.NewLine, "");
            string lastLine = lines.Last().Replace(Environment.NewLine, "");

            string formattedElement = firstLine + "..." + lastLine;
            return formattedElement.Escape();
        }

        private static string [] SplitIntoSeparateLines(XElement element)
        {
            string formattedXml = element.ToString();
            return formattedXml.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}