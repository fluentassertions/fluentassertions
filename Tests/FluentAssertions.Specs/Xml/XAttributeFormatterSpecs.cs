using System.Xml.Linq;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Xml;

public class XAttributeFormatterSpecs
{
    [Fact]
    public void When_formatting_an_attribute_it_should_return_the_name_and_value()
    {
        // Act
        var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
        XAttribute attribute = element.Attribute("name");
        string result = Formatter.ToString(attribute);

        // Assert
        result.Should().Be(@"name=""Martin""");
    }
}
