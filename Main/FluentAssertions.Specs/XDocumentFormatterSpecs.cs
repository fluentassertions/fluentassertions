using System.Xml.Linq;

using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class XDocumentFormatterSpecs
    {
        [TestMethod]
        public void When_element_has_root_element_it_should_include_it_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new XDocumentValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var document = XDocument.Parse(
                @"<configuration>
                     <startDate />
                     <endDate />
                  </configuration>");

            string result = formatter.ToString(document);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(@"<configuration>...</configuration>");
        }

        [TestMethod]
        public void When_element_has_no_root_element_it_should_include_it_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new XDocumentValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var document = new XDocument();

            string result = formatter.ToString(document);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(@"[XML document without root element]");
        }
    }
}