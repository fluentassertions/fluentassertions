using System;
using System.Xml;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Xml;

public class XmlElementAssertionSpecs
{
    public class BeEquivalent
    {
        [Fact]
        public void When_asserting_xml_element_is_equivalent_to_another_xml_element_with_same_contents_it_should_succeed()
        {
            // This test is basically just a check that the BeEquivalent method
            // is available on XmlElementAssertions, which it should be if
            // XmlElementAssertions inherits XmlNodeAssertions.

            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<user>grega</user>");
            var element = xmlDoc.DocumentElement;
            var expectedDoc = new XmlDocument();
            expectedDoc.LoadXml("<user>grega</user>");
            var expected = expectedDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class HaveInnerText
    {
        [Fact]
        public void When_asserting_xml_element_has_a_specific_inner_text_and_it_does_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<user>grega</user>");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveInnerText("grega");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_xml_element_has_a_specific_inner_text_but_it_has_a_different_inner_text_it_should_throw()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<user>grega</user>");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveInnerText("stamac");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_a_specific_inner_text_but_it_has_a_different_inner_text_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();
            document.LoadXml("<user>grega</user>");
            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveInnerText("stamac", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theElement to have value \"stamac\" because we want to test the failure message, but found \"grega\".");
        }
    }

    public class HaveAttribute
    {
        [Fact]
        public void When_asserting_xml_element_has_attribute_with_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttribute("name", "martin");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttribute("age", "36");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();
            document.LoadXml("""<user name="martin" />""");
            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveAttribute("age", "36", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected theElement to have attribute \"age\" with value \"36\"" +
                    " because we want to test the failure message" +
                    ", but found no such attribute in <user name=\"martin\"*");
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttribute("name", "dennis");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();
            document.LoadXml("""<user name="martin" />""");
            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveAttribute("name", "dennis", "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected attribute \"name\" in theElement to have value \"dennis\"" +
                    " because we want to test the failure message" +
                    ", but found \"martin\".");
        }
    }

    public class HaveAttributeWithNamespace
    {
        [Fact]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_and_it_does_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user xmlns:a="http://www.example.com/2012/test" a:name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "martin");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user xmlns:a="http://www.example.com/2012/test" a:name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("age", "http://www.example.com/2012/test", "36");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();
            document.LoadXml("""<user xmlns:a="http://www.example.com/2012/test" a:name="martin" />""");
            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                theElement.Should().HaveAttributeWithNamespace("age", "http://www.example.com/2012/test", "36",
                    "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected theElement to have attribute \"{http://www.example.com/2012/test}age\" with value \"36\"" +
                    " because we want to test the failure message" +
                    ", but found no such attribute in <user xmlns:a=\"http:…");
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("""<user xmlns:a="http://www.example.com/2012/test" a:name="martin" />""");
            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "dennis");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();
            document.LoadXml("""<user xmlns:a="http://www.example.com/2012/test" a:name="martin" />""");
            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "dennis",
                    "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected attribute \"{http://www.example.com/2012/test}name\" in theElement to have value \"dennis\"" +
                    " because we want to test the failure message" +
                    ", but found \"martin\".");
        }
    }

    public class HaveElement
    {
        [Fact]
        public void When_asserting_xml_element_has_child_element_and_it_does_it_should_succeed()
        {
            // Arrange
            var xml = new XmlDocument();

            xml.LoadXml(
                """
                <parent>
                    <child />
                </parent>
                """);

            var element = xml.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElement("child");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent>
                    <child />
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElement("unknown");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();

            document.LoadXml(
                """
                <parent>
                    <child />
                </parent>
                """);

            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveElement("unknown", "because we want to test the failure message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theElement to have child element \"unknown\""
                + " because we want to test the failure message"
                + ", but no such child element was found.");
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_it_should_return_the_matched_element_in_the_which_property()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent>
                    <child attr='1' />
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            var matchedElement = element.Should().HaveElement("child").Subject;

            // Assert
            matchedElement.Should().BeOfType<XmlElement>()
                .And.HaveAttribute("attr", "1");

            matchedElement.Name.Should().Be("child");
        }

        [Fact]
        public void When_asserting_xml_element_with_ns_has_child_element_and_it_does_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent xmlns="test">
                    <child>value</child>
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElement("child");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_and_it_does_with_ns_it_should_succeed2()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent>
                    <child xmlns="test">value</child>
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElement("child");

            // Assert
            act.Should().NotThrow();
        }
    }

    public class HaveElementWithNamespace
    {
        [Fact]
        public void When_asserting_xml_element_has_child_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent xmlns:c='http://www.example.com/2012/test'>
                    <c:child />
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElementWithNamespace("child", "http://www.example.com/2012/test");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_with_ns_but_it_does_not_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(
                """
                <parent>
                    <child />
                </parent>
                """);

            var element = xmlDoc.DocumentElement;

            // Act
            Action act = () =>
                element.Should().HaveElementWithNamespace("unknown", "http://www.example.com/2012/test");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_xml_element_has_child_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = new XmlDocument();

            document.LoadXml(
                """
                <parent>
                    <child />
                </parent>
                """);

            var theElement = document.DocumentElement;

            // Act
            Action act = () =>
                theElement.Should().HaveElementWithNamespace("unknown", "http://www.example.com/2012/test",
                    "because we want to test the failure message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theElement to have child element \"{{http://www.example.com/2012/test}}unknown\""
                + " because we want to test the failure message"
                + ", but no such child element was found.");
        }
    }
}
