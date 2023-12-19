using System.Xml.Linq;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Xml;

public class XElementFormatterSpecs
{
    [Fact]
    public void When_element_has_attributes_it_should_include_them_in_the_output()
    {
        // Act
        var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
        string result = Formatter.ToString(element);

        // Assert
        result.Should().Be(@"<person name=""Martin"" age=""36"" />");
    }

    [Fact]
    public void When_element_has_child_element_it_should_not_include_them_in_the_output()
    {
        // Act
        var element = XElement.Parse(
            @"<person name=""Martin"" age=""36"">
                      <child name=""Laura"" />
                  </person>");

        string result = Formatter.ToString(element);

        // Assert
        result.Should().Be(@"<person name=""Martin"" age=""36"">…</person>");
    }
}
