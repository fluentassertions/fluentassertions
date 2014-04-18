using System.Xml.Linq;

using FluentAssertions.Formatting;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
namespace FluentAssertions.Specs
{
    [TestClass]
    public class XElementFormatterSpecs
    {
        [TestMethod]
        public void When_element_has_attributes_it_should_include_them_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new XElementValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var element = XElement.Parse(@"<person name=""Martin"" age=""36"" />");
            string result = formatter.ToString(element, false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(@"<person name=\""Martin\"" age=\""36\"" />");
        }

        [TestMethod]
        public void When_element_has_child_element_it_should_not_include_them_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new XElementValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var element = XElement.Parse(
                @"<person name=""Martin"" age=""36"">
                      <child name=""Laura"" />
                  </person>");

            string result = formatter.ToString(element, false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(@"<person name=\""Martin\"" age=\""36\"">...</person>");
        }
    }
}