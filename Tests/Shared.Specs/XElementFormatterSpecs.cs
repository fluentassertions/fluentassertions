using System.Xml.Linq;

using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    public class XElementFormatterSpecs
    {
        [Fact]
        public void When_element_has_attributes_it_should_include_them_in_the_output()
        {
            // Arrange
            var formatter = new XElementValueFormatter();

            // Act
            var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
            string result = formatter.Format(element, new FormattingContext(), null);

            // Assert
            result.Should().Be(@"<person name=""Martin"" age=""36"" />");
        }

        [Fact]
        public void When_element_has_child_element_it_should_not_include_them_in_the_output()
        {
            // Arrange
            var formatter = new XElementValueFormatter();

            // Act
            var element = XElement.Parse(
                @"<person name=""Martin"" age=""36"">
                      <child name=""Laura"" />
                  </person>");

            string result = formatter.Format(element, new FormattingContext(), null);

            // Assert
            result.Should().Be(@"<person name=""Martin"" age=""36"">…</person>");
        }
    }
}
