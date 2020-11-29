using System;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class XAttributeAssertionSpecs
    {
        #region Be / NotBe

        [Fact]
        public void When_asserting_an_xml_attribute_is_equal_to_the_same_xml_attribute_it_should_succeed()
        {
            // Arrange
            var attribute = new XAttribute("name", "value");
            var sameXAttribute = new XAttribute("name", "value");

            // Act
            Action act = () =>
                attribute.Should().Be(sameXAttribute);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_attribute_is_equal_to_a_different_xml_attribute_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theAttribute = new XAttribute("name", "value");
            var otherAttribute = new XAttribute("name2", "value");

            // Act
            Action act = () =>
                theAttribute.Should().Be(otherAttribute, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected theAttribute to be {otherAttribute} because we want to test the failure message, but found {theAttribute}.");
        }

        [Fact]
        public void When_asserting_an_xml_attribute_is_not_equal_to_a_different_xml_attribute_it_should_succeed()
        {
            // Arrange
            var attribute = new XAttribute("name", "value");
            var otherXAttribute = new XAttribute("name2", "value");

            // Act
            Action act = () =>
                attribute.Should().NotBe(otherXAttribute);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_attribute_is_not_equal_to_the_same_xml_attribute_it_should_throw()
        {
            // Arrange
            var theAttribute = new XAttribute("name", "value");
            var sameXAttribute = theAttribute;

            // Act
            Action act = () =>
                theAttribute.Should().NotBe(sameXAttribute);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theAttribute to be name=\"value\".");
        }

        [Fact]
        public void When_asserting_an_xml_attribute_is_not_equal_to_the_same_xml_attribute_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var theAttribute = new XAttribute("name", "value");
            var sameAttribute = theAttribute;

            // Act
            Action act = () =>
                theAttribute.Should().NotBe(sameAttribute, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Did not expect theAttribute to be {sameAttribute} because we want to test the failure message.");
        }

        #endregion

        #region BeNull / NotBeNull

        [Fact]
        public void When_asserting_a_null_xml_attribute_is_null_it_should_succeed()
        {
            // Arrange
            XAttribute attribute = null;

            // Act
            Action act = () =>
                attribute.Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_non_null_xml_attribute_is_null_it_should_fail()
        {
            // Arrange
            var theAttribute = new XAttribute("name", "value");

            // Act
            Action act = () =>
                theAttribute.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theAttribute to be <null>, but found name=\"value\".");
        }

        [Fact]
        public void When_asserting_a_non_null_xml_attribute_is_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theAttribute = new XAttribute("name", "value");

            // Act
            Action act = () =>
                theAttribute.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected theAttribute to be <null> because we want to test the failure message, but found {theAttribute}.");
        }

        [Fact]
        public void When_asserting_a_non_null_xml_attribute_is_not_null_it_should_succeed()
        {
            // Arrange
            var attribute = new XAttribute("name", "value");

            // Act
            Action act = () =>
                attribute.Should().NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_null_xml_attribute_is_not_null_it_should_fail()
        {
            // Arrange
            XAttribute theAttribute = null;

            // Act
            Action act = () =>
                theAttribute.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theAttribute not to be <null>.");
        }

        [Fact]
        public void When_asserting_a_null_xml_attribute_is_not_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            XAttribute theAttribute = null;

            // Act
            Action act = () =>
                theAttribute.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theAttribute not to be <null> because we want to test the failure message.");
        }

        #endregion

        #region HaveValue

        [Fact]
        public void When_asserting_attribute_has_a_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var attribute = new XAttribute("age", "36");

            // Act
            Action act = () =>
                attribute.Should().HaveValue("36");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_attribute_has_a_specific_value_but_it_has_a_different_value_it_should_throw()
        {
            // Arrange
            var theAttribute = new XAttribute("age", "36");

            // Act
            Action act = () =>
                theAttribute.Should().HaveValue("16");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theAttribute \"age\" to have value \"16\", but found \"36\".");
        }

        [Fact]
        public void When_asserting_attribute_has_a_specific_value_but_it_has_a_different_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var theAttribute = new XAttribute("age", "36");

            // Act
            Action act = () =>
                theAttribute.Should().HaveValue("16", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theAttribute \"age\" to have value \"16\" because we want to test the failure message, but found \"36\".");
        }

        #endregion
    }
}
