using System.Xml.Linq;

using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class XAttributeFormatterSpecs
    {
        [TestMethod]
        public void When_formatting_an_attribute_it_should_return_the_name_and_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new XAttributeValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
            XAttribute attribute = element.Attribute("name");
            string result = formatter.ToString(attribute);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(@"name=""Martin""");
        }
    }
}