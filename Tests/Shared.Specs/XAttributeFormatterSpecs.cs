using System.Xml.Linq;

using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    public class XAttributeFormatterSpecs
    {
        [Fact]
        public void When_formatting_an_attribute_it_should_return_the_name_and_value()
        {
            // Arrange
            var formatter = new XAttributeValueFormatter();

            // Act
            var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
            XAttribute attribute = element.Attribute("name");
            string result = formatter.Format(attribute, new FormattingContext(), null);

            // Assert
            result.Should().Be(@"name=""Martin""");
        }
    }
}
