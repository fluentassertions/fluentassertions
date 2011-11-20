using System.Xml.Linq;

namespace FluentAssertions.Formatting
{
    internal class XAttributeValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is XAttribute);
        }

        public string ToString(object value, int nestedPropertyLevel = 0)
        {
            var attribute = (XAttribute)value;
            return attribute.ToString();
        }
    }
}