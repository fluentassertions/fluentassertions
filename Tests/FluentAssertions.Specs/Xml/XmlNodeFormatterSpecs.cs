using System;
using System.Xml;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Xml;

public class XmlNodeFormatterSpecs
{
    [Fact]
    public void When_a_node_is_20_chars_long_it_should_not_be_trimmed()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(@"<xml attr=""01234"" />");

        // Act
        string result = Formatter.ToString(xmlDoc);

        // Assert
        result.Should().Be(@"<xml attr=""01234"" />" + Environment.NewLine);
    }

    [Fact]
    public void When_a_node_is_longer_then_20_chars_it_should_be_trimmed()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(@"<xml attr=""012345"" />");

        // Act
        string result = Formatter.ToString(xmlDoc);

        // Assert
        result.Should().Be(@"<xml attr=""012345"" /…" + Environment.NewLine);
    }
}
