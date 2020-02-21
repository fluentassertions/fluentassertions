using System;
using System.Xml.Linq;

using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class XElementAssertionSpecs
    {
        #region Be / NotBe

        [Fact]
        public void When_asserting_an_xml_element_is_equal_to_the_same_xml_element_it_should_succeed()
        {
            // Arrange
            var element = new XElement("element");
            var sameElement = new XElement("element");

            // Act
            Action act = () =>
                element.Should().Be(sameElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equal_to_a_different_xml_element_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = new XElement("element");
            var otherElement = new XElement("other");

            // Act
            Action act = () =>
                element.Should().Be(otherElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element to be*other*because we want to test the failure message, but found *element*");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equal_to_an_xml_element_with_a_deep_difference_it_should_fail()
        {
            // Arrange
            var expected =
                new XElement("parent"
                    , new XElement("child"
                        , new XElement("grandChild")));
            var actual =
                new XElement("parent"
                    , new XElement("child"
                        , new XElement("grandChild2")));

            // Act
            Action act = () => actual.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_the_equality_of_an_xml_element_but_is_null_it_should_throw_appropriately()
        {
            // Arrange
            XElement actual = null;
            var expected = new XElement("other");

            // Act
            Action act = () => actual.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element to be*other*, but found <null>.");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_not_equal_to_a_different_xml_element_it_should_succeed()
        {
            // Arrange
            var element = new XElement("element");
            var otherElement = new XElement("other");

            // Act
            Action act = () =>
                element.Should().NotBe(otherElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_deep_xml_element_is_not_equal_to_a_different_xml_element_it_should_succeed()
        {
            // Arrange
            var differentElement =
                new XElement("parent"
                    , new XElement("child"
                        , new XElement("grandChild")));
            var element =
                new XElement("parent"
                    , new XElement("child"
                        , new XElement("grandChild2")));

            // Act
            Action act = () => element.Should().NotBe(differentElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_not_equal_to_the_same_xml_element_it_should_fail()
        {
            // Arrange
            var element = new XElement("element");
            var sameElement = element;

            // Act
            Action act = () =>
                element.Should().NotBe(sameElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_not_equal_to_the_same_xml_element_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = new XElement("element");
            var sameElement = element;

            // Act
            Action act = () =>
                element.Should().NotBe(sameElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element not to be <element />" +
                    " because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_the_inequality_of_an_xml_element_but_it_is_null_it_should_succeed()
        {
            // Arrange
            XElement actual = null;
            var expected = new XElement("other");

            // Act
            Action act = () => actual.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region BeNull / NotBeNull

        [Fact]
        public void When_asserting_an_xml_element_is_null_and_it_is_it_should_succeed()
        {
            // Arrange
            XElement element = null;

            // Act
            Action act = () =>
                element.Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_null_but_it_is_not_it_should_fail()
        {
            // Arrange
            var element = new XElement("element");

            // Act
            Action act = () =>
                element.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_null_but_it_is_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = new XElement("element");

            // Act
            Action act = () =>
                element.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected element to be <null> because we want to test the failure message, but found <element />.");
        }

        [Fact]
        public void When_asserting_a_non_null_xml_element_is_not_null_it_should_succeed()
        {
            // Arrange
            var element = new XElement("element");

            // Act
            Action act = () =>
                element.Should().NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_null_xml_element_is_not_null_it_should_fail()
        {
            // Arrange
            XElement element = null;

            // Act
            Action act = () =>
                element.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_element_is_not_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            XElement element = null;

            // Act
            Action act = () =>
                element.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected element not to be <null> because we want to test the failure message.");
        }

        #endregion

        #region BeEquivalentTo / NotBeEquivalentTo

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_the_same_xml_element_it_should_succeed()
        {
            // Arrange
            var element = new XElement("element");
            var sameXElement = element;

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(sameXElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_a_different_xml_element_with_same_structure_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_empty_xml_element_is_equivalent_to_a_different_selfclosing_xml_element_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var element = XElement.Parse("<parent><child></child></parent>");
            var otherElement = XElement.Parse("<parent><child /></parent>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().BeEquivalentTo(otherElement);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_selfclosing_xml_element_is_equivalent_to_a_different_empty_xml_element_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var element = XElement.Parse("<parent><child /></parent>");
            var otherElement = XElement.Parse("<parent><child></child></parent>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().BeEquivalentTo(otherElement);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_a_xml_element_with_elements_missing_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /><child2 /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_a_different_xml_element_with_extra_elements_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_a_different_xml_element_elements_missing_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /><child2 /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected node of type EndElement at \"/parent\" because we want to test the failure message, but found Element.");
        }

        [Fact]
        public void When_asserting_a_xml_element_is_equivalent_to_a_different_xml_element_with_extra_elements_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected node of type Element at \"/parent\" because we want to test the failure message, but found EndElement.");
        }

        [Fact]
        public void When_asserting_an_empty_xml_element_is_equivalent_to_a_different_xml_element_with_text_content_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child>text</child></parent>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().BeEquivalentTo(otherXElement, "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_elements_missing_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /><child2 /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_extra_elements_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_same_structure_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_same_contents_but_different_ns_prefixes_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<parent xmlns:ns1=""a""><ns1:child /></parent>");
            var otherXElement = XElement.Parse(@"<parent xmlns:ns2=""a""><ns2:child /></parent>");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_same_contents_but_extra_unused_xmlns_declaration_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<xml xmlns:ns1=""a"" />");
            var otherXElement = XElement.Parse("<xml />");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_the_same_xml_element_it_should_fail()
        {
            // Arrange
            var element = new XElement("element");
            var sameXElement = element;

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(sameXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_a_different_xml_element_with_same_structure_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var otherXElement = XElement.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect Xml to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_asserting_a_xml_element_is_not_equivalent_to_the_same_xml_element_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse("<parent><child /></parent>");
            var sameXElement = element;

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(sameXElement, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect Xml to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_a_different_xml_element_with_different_namespace_prefix_it_should_succeed()
        {
            // Arrange
            var subject = XElement.Parse("<xml xmlns=\"urn:a\"/>");
            var expected = XElement.Parse("<a:xml xmlns:a=\"urn:a\"/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_a_different_xml_element_which_differs_only_on_unused_namespace_declaration_it_should_succeed()
        {
            // Arrange
            var subject = XElement.Parse("<xml xmlns:a=\"urn:a\"/>");
            var expected = XElement.Parse("<xml/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_which_lacks_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XElement.Parse("<xml><element b=\"1\"/></xml>");
            var expected = XElement.Parse("<xml><element a=\"b\" b=\"1\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" because we want to test the failure message, but found none.");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_which_has_extra_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XElement.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XElement.Parse("<xml><element/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_which_has_different_attribute_values_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XElement.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XElement.Parse("<xml><element a=\"c\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" to have value \"c\" because we want to test the failure message, but found \"b\".");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_which_has_attribute_with_different_namespace_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XElement.Parse("<xml><element xmlns:ns=\"urn:a\" ns:a=\"b\"/></xml>");
            var expected = XElement.Parse("<xml><element a=\"b\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"ns:a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_which_has_different_text_contents_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XElement.Parse("<xml>a</xml>");
            var expected = XElement.Parse("<xml>b</xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected content to be \"b\" at \"/xml\" because we want to test the failure message, but found \"a\".");
        }

        [Fact]
        public void When_asserting_an_xml_element_is_equivalent_to_different_xml_element_with_different_comments_it_should_succeed()
        {
            // Arrange
            var subject = XElement.Parse("<xml><!--Comment--><a/></xml>");
            var expected = XElement.Parse("<xml><a/><!--Comment--></xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region HaveValue

        [Fact]
        public void When_asserting_element_has_a_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse("<user>grega</user>");

            // Act
            Action act = () =>
                element.Should().HaveValue("grega");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_element_has_a_specific_value_but_it_has_a_different_value_it_should_throw()
        {
            // Arrange
            var element = XElement.Parse("<user>grega</user>");

            // Act
            Action act = () =>
                element.Should().HaveValue("stamac");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_a_specific_value_but_it_has_a_different_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse("<user>grega</user>");

            // Act
            Action act = () =>
                element.Should().HaveValue("stamac", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element 'user' to have value \"stamac\"" +
                    " because we want to test the failure message" +
                        ", but found \"grega\".");
        }

        #endregion

        #region HaveAttribute

        [Fact]
        public void When_asserting_element_has_attribute_with_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse(@"<user name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute("name", "martin");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_ns_and_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute(XName.Get("name", "http://www.example.com/2012/test"), "martin");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<user name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute("age", "36");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute(XName.Get("age", "http://www.example.com/2012/test"), "36");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(@"<user name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute("age", "36", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element to have attribute \"age\" with value \"36\"" +
                    " because we want to test the failure message" +
                        ", but found no such attribute in <user name=\"martin\" />");
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute(XName.Get("age", "http://www.example.com/2012/test"), "36", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML element to have attribute \"{http://www.example.com/2012/test}age\" with value \"36\"" +
                    " because we want to test the failure message" +
                        ", but found no such attribute in <user xmlns:a=\"http://www.example.com/2012/test\" a:name=\"martin\" />");
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<user name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute("name", "dennis");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute(XName.Get("name", "http://www.example.com/2012/test"), "dennis");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(@"<user name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute("name", "dennis", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML attribute \"name\" to have value \"dennis\"" +
                    " because we want to test the failure message" +
                        ", but found \"martin\".");
        }

        [Fact]
        public void When_asserting_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");

            // Act
            Action act = () =>
                element.Should().HaveAttribute(XName.Get("name", "http://www.example.com/2012/test"), "dennis", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML attribute \"{http://www.example.com/2012/test}name\" to have value \"dennis\"" +
                    " because we want to test the failure message" +
                        ", but found \"martin\".");
        }

        #endregion

        #region HaveElement

        [Fact]
        public void When_asserting_element_has_child_element_and_it_does_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement("child");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_element_has_child_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent xmlns:c='http://www.example.com/2012/test'>
                    <c:child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement(XName.Get("child", "http://www.example.com/2012/test"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_element_has_child_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement("unknown");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_child_element_with_ns_but_it_does_not_it_should_fail()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement(XName.Get("unknown", "http://www.example.com/2012/test"));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_element_has_child_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement("unknown", "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML element {0} to have child element \"unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(element));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_element_has_child_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                element.Should().HaveElement(XName.Get("unknown", "http://www.example.com/2012/test"), "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML element {0} to have child element \"{{http://www.example.com/2012/test}}unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(element));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_element_has_child_element_it_should_return_the_matched_element_in_the_which_property()
        {
            // Arrange
            var element = XElement.Parse(
                @"<parent>
                    <child attr='1' />
                  </parent>");

            // Act
            var matchedElement = element.Should().HaveElement("child").Subject;

            // Assert
            matchedElement.Should().BeOfType<XElement>()
                .And.HaveAttribute("attr", "1");
            matchedElement.Name.Should().Be(XName.Get("child"));
        }

        #endregion
    }
}
