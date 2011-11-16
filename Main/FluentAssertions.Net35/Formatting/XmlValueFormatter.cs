using System;
using System.Xml.Linq;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class XmlValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is XElement);
        }

        public string ToString(object value, int nestedPropertyLevel = 0)
        {
            return Environment.NewLine + value.ToString().Escape() + Environment.NewLine;
        }
    }
}