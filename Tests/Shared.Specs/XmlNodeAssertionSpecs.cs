using System;
using System.Xml;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class XmlNodeAssertionSpecs
    {
        #region BeSameAs / NotBeSameAs

        [Fact]
        public void When_asserting_an_xml_node_is_the_same_as_the_same_xml_node_it_should_succeed()
        {
            // Arrange
            var doc = new XmlDocument();

            // Act
            Action act = () =>
                doc.Should().BeSameAs(doc);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_same_as_a_different_xml_node_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<doc/>");
            var otherNode = new XmlDocument();
            otherNode.LoadXml("<otherDoc/>");

            // Act
            Action act = () =>
                doc.Should().BeSameAs(otherNode, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected doc to refer to <otherDoc /> because we want to test the failure message, but found <doc />.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_same_as_a_different_xml_node_it_should_fail_with_descriptive_message_and_truncate_xml()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<doc>Some very long text that should be truncated.</doc>");
            var otherNode = new XmlDocument();
            otherNode.LoadXml("<otherDoc/>");

            // Act
            Action act = () =>
                doc.Should().BeSameAs(otherNode, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected doc to refer to <otherDoc /> because we want to test the failure message, but found <doc>Some very long….");
        }

        [Fact]
        public void When_asserting_the_equality_of_an_xml_node_but_is_null_it_should_throw_appropriately()
        {
            // Arrange
            XmlDocument actual = null;
            var expected = new XmlDocument();
            expected.LoadXml("<xml/>");

            // Act
            Action act = () => actual.Should().BeSameAs(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to refer to *xml*, but found <null>.");
        }
        #endregion

        #region BeNull / NotBeNull

        [Fact]
        public void When_asserting_an_xml_node_is_null_and_it_is_it_should_succeed()
        {
            // Arrange
            XmlNode node = null;

            // Act
            Action act = () =>
                node.Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_null_but_it_is_not_it_should_fail()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml/>");

            // Act
            Action act = () =>
                xmlDoc.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_null_but_it_is_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml/>");

            // Act
            Action act = () =>
                xmlDoc.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected xmlDoc to be <null> because we want to test the failure message," +
                    " but found <xml />.");
        }

        [Fact]
        public void When_asserting_a_non_null_xml_node_is_not_null_it_should_succeed()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml/>");

            // Act
            Action act = () =>
                xmlDoc.Should().NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_null_xml_node_is_not_null_it_should_fail()
        {
            // Arrange
            XmlDocument xmlDoc = null;

            // Act
            Action act = () =>
                xmlDoc.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_node_is_not_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            XmlDocument xmlDoc = null;

            // Act
            Action act = () =>
                xmlDoc.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected xmlDoc not to be <null> because we want to test the failure message.");
        }

        #endregion

        #region BeEquivalentTo / NotBeEquivalentTo

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_the_same_xml_node_it_should_succeed()
        {
            // Arrange
            var doc = new XmlDocument();

            // Act
            Action act = () =>
                doc.Should().BeEquivalentTo(doc);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_not_equivalent_to_som_other_xml_node_it_should_succeed()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");
            var unexpected = new XmlDocument();
            unexpected.LoadXml("<xml>b</xml>");

            // Act
            Action act = () =>
                subject.Should().NotBeEquivalentTo(unexpected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_not_equivalent_to_same_xml_node_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");

            // Act
            Action act = () =>
                subject.Should().NotBeEquivalentTo(subject, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect Xml to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_a_different_xml_node_with_other_contents_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<subject/>");
            var expected = new XmlDocument();
            expected.LoadXml("<expected/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected local name of element at \"/\" to be \"expected\" because we want to test the failure message, but found \"subject\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_a_different_xml_node_with_same_contents_it_should_succeed()
        {
            // Arrange
            var xml = "<root><a xmlns=\"urn:a\"><data>data</data></a><ns:b xmlns:ns=\"urn:b\"><data>data</data></ns:b></root>";

            var subject = new XmlDocument();
            subject.LoadXml(xml);
            var expected = new XmlDocument();
            expected.LoadXml(xml);

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_assertion_an_xml_node_is_equivalent_to_a_different_xml_node_with_different_namespace_prefix_it_should_succeed()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml xmlns=\"urn:a\"/>");
            var expected = new XmlDocument();
            expected.LoadXml("<a:xml xmlns:a=\"urn:a\"/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_a_different_xml_node_which_differs_only_on_unused_namespace_declaration_it_should_succeed()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml xmlns:a=\"urn:a\"/>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_a_different_XmlDocument_which_differs_on_a_child_element_name_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><child><subject/></child></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><child><expected/></child></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected local name of element at \"/xml/child\" to be \"expected\" because we want to test the failure message, but found \"subject\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_a_different_xml_node_which_differs_on_a_child_element_namespace_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<a:xml xmlns:a=\"urn:a\"><a:child><a:data/></a:child></a:xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml xmlns=\"urn:a\"><child><data xmlns=\"urn:b\"/></child></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected namespace of element \"data\" at \"/xml/child\" to be \"urn:b\" because we want to test the failure message, but found \"urn:a\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_contains_an_unexpected_node_type_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml>data</xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><data/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected node of type Element at \"/xml\" because we want to test the failure message, but found Text.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_contains_extra_elements_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><data/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected end of document because we want to test the failure message, but found \"data\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_lacks_elements_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml/>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><data/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected \"data\" because we want to test the failure message, but found end of document.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_lacks_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element b=\"1\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"b\" b=\"1\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" because we want to test the failure message, but found none.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_has_extra_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_has_different_attribute_values_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"c\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" to have value \"c\" because we want to test the failure message, but found \"b\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_has_attribute_with_different_namespace_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element xmlns:ns=\"urn:a\" ns:a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"b\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"ns:a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_which_has_different_text_contents_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml>b</xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected content to be \"b\" at \"/xml\" because we want to test the failure message, but found \"a\".");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_with_different_comments_it_should_succeed()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><!--Comment--><a/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><a/><!--Comment--></xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_to_different_xml_node_with_different_insignificant_whitespace_it_should_succeed()
        {
            // Arrange
            var subject = new XmlDocument() { PreserveWhitespace = true };
            subject.LoadXml("<xml><a><b/></a></xml>");
            var expected = new XmlDocument() { PreserveWhitespace = true };
            expected.LoadXml("<xml>\n<a>   \n   <b/></a>\r\n</xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_that_contains_an_unsupported_node_it_should_throw_a_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><![CDATA[Text]]></xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(subject);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("CDATA found at /xml is not supported for equivalency comparison.");
        }

        [Fact]
        public void When_asserting_an_xml_node_is_equivalent_that_isnt_it_should_include_the_right_location_in_the_descriptive_message()
        {
            // Arrange
            var subject = new XmlDocument();
            subject.LoadXml("<xml><a/><b c=\"d\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><a/><b c=\"e\"/></xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected attribute \"c\" at \"/xml/b\" to have value \"e\", but found \"d\".");
        }

        #endregion

    }
}
