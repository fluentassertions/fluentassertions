using System;
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

        public string ToString(object value, int nestedPropertyLevel = 0)
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