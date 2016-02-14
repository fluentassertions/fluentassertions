using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class XmlNodeAssertionSpecs
    {
        #region Be / NotBe

        [TestMethod]
        public void When_asserting_an_XmlNode_is_the_same_as_the_same_XmlNode_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var doc = new XmlDocument();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                doc.Should().BeSameAs(doc);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_same_as_a_different_XmlDocument_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var doc = new XmlDocument();
            doc.LoadXml("<doc/>");
            var otherNode = new XmlDocument();
            otherNode.LoadXml("<otherDoc/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                doc.Should().BeSameAs(otherNode, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected Xml Node to refer to <otherDoc /> because we want to test the failure message, but found <doc />.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_same_as_a_different_XmlDocument_it_should_fail_with_descriptive_message_and_truncate_xml()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var doc = new XmlDocument();
            doc.LoadXml("<doc>Some very long text that should be truncated.</doc>");
            var otherNode = new XmlDocument();
            otherNode.LoadXml("<otherDoc/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                doc.Should().BeSameAs(otherNode, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected Xml Node to refer to <otherDoc /> because we want to test the failure message, but found <doc>Some very long....");
        }
        #endregion

        #region BeEquivalentTo / NotBeEquivalentTo

        [TestMethod]
        public void When_asserting_an_XmlNode_is_equivalent_as_the_same_XmlNode_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var doc = new XmlDocument();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                doc.Should().BeEquivalentTo(doc);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlNode_is_not_equivalent_to_som_other_XmlNode_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");
            var unexpected = new XmlDocument();
            unexpected.LoadXml("<xml>b</xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().NotBeEquivalentTo(unexpected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlNode_is_not_equivalent_to_same_XmlNode_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().NotBeEquivalentTo(subject, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected Xml to not be equivalent because we want to test the failure message, but it is.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_a_different_XmlDocument_with_other_contents_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<subject/>");
            var expected = new XmlDocument();
            expected.LoadXml("<expected/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected local name of element at \"/\" to be \"expected\" because we want to test the failure message, but found \"subject\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_a_different_XmlDocument_with_same_contents_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xml = "<root><a xmlns=\"urn:a\"><data>data</data></a><ns:b xmlns:ns=\"urn:b\"><data>data</data></ns:b></root>";

            var subject = new XmlDocument();
            subject.LoadXml(xml);
            var expected = new XmlDocument();
            expected.LoadXml(xml);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_assertion_an_XmlDocument_is_equivalent_to_a_different_XmlDocument_with_different_namespace_prefix_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml xmlns=\"urn:a\"/>");
            var expected = new XmlDocument();
            expected.LoadXml("<a:xml xmlns:a=\"urn:a\"/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        #endregion

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_a_different_XmlDocument_which_differs_only_on_unused_namespace_declaration_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml xmlns:a=\"urn:a\"/>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_a_different_XmlDcoument_which_differs_on_a_child_element_name_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><child><subject/></child></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><child><expected/></child></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected local name of element at \"/xml/child\" to be \"expected\" because we want to test the failure message, but found \"subject\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_a_different_XmlDocument_which_differs_on_a_child_element_namespace_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<a:xml xmlns:a=\"urn:a\"><a:child><a:data/></a:child></a:xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml xmlns=\"urn:a\"><child><data xmlns=\"urn:b\"/></child></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected namespace of element \"data\" at \"/xml/child\" to be \"urn:b\" because we want to test the failure message, but found \"urn:a\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_contains_an_unexpected_node_type_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml>data</xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><data/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected node of type Element at \"/xml\" because we want to test the failure message, but found Text.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_contains_extra_elements_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><data/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml/>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected end of document because we want to test the failure message, but found \"data\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_lacks_elements_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml/>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><data/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected \"data\" because we want to test the failure message, but found end of document.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_lacks_attributes_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element b=\"1\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"b\" b=\"1\"/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" because we want to test the failure message, but found none.");
        }


        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_has_extra_attributes_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Didn't expect to find attribute \"a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_has_different_attribute_values_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"c\"/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" to have value \"c\" because we want to test the failure message, but found \"b\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_has_attribute_with_different_namespace_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><element xmlns:ns=\"urn:a\" ns:a=\"b\"/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><element a=\"b\"/></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Didn't expect to find attribute \"ns:a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_which_has_different_text_contents_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml>a</xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml>b</xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().
                WithMessage("Expected content to be \"b\" at \"/xml\" because we want to test the failure message, but found \"a\".");
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_with_different_comments_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument();
            subject.LoadXml("<xml><!--Comment--><a/></xml>");
            var expected = new XmlDocument();
            expected.LoadXml("<xml><a/><!--Comment--></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_to_different_XmlDocument_with_different_insignificant_whitespace_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument() { PreserveWhitespace = true };
            subject.LoadXml("<xml><a><b/></a></xml>");
            var expected = new XmlDocument() { PreserveWhitespace = true };
            expected.LoadXml("<xml>\n<a>   \n   <b/></a>\r\n</xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_XmlDocument_is_equivalent_that_contains_an_unsupported_node_it_should_throw_a_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new XmlDocument() { PreserveWhitespace = true };
            subject.LoadXml("<xml><![CDATA[Text]]></xml>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(subject);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NotSupportedException>()
                .WithMessage("CDATA found at /xml is not supported for equivalency comparison.");
        }
    }
}
