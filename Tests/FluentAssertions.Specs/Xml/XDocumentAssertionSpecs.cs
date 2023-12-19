using System;
using System.Xml.Linq;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Xml;

public class XDocumentAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void When_asserting_a_xml_document_is_equal_to_the_same_xml_document_it_should_succeed()
        {
            // Arrange
            var document = new XDocument();
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().Be(sameXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equal_to_a_different_xml_document_it_should_fail()
        {
            // Arrange
            var theDocument = new XDocument();
            var otherXDocument = new XDocument();

            // Act
            Action act = () =>
                theDocument.Should().Be(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be [XML document without root element], but found [XML document without root element].");
        }

        [Fact]
        public void When_the_expected_element_is_null_it_fails()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().Be(new XDocument(), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be [XML document without root element] *failure message*, but found <null>.");
        }

        [Fact]
        public void When_both_subject_and_expected_documents_are_null_it_succeeds()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().Be(null);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_document_is_expected_to_equal_null_it_fails()
        {
            // Arrange
            XDocument theDocument = new();

            // Act
            Action act = () => theDocument.Should().Be(null, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be <null> *failure message*, but found [XML document without root element].");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equal_to_a_different_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<configuration></configuration>");
            var otherXDocument = XDocument.Parse("<data></data>");

            // Act
            Action act = () =>
                theDocument.Should().Be(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be <data*> because we want to test the failure message, but found <configuration*>.");
        }
    }

    public class NotBe
    {
        [Fact]
        public void When_asserting_a_xml_document_is_not_equal_to_a_different_xml_document_it_should_succeed()
        {
            // Arrange
            var document = new XDocument();
            var otherXDocument = new XDocument();

            // Act
            Action act = () =>
                document.Should().NotBe(otherXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equal_to_the_same_xml_document_it_should_fail()
        {
            // Arrange
            var theDocument = new XDocument();
            var sameXDocument = theDocument;

            // Act
            Action act = () =>
                theDocument.Should().NotBe(sameXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be [XML document without root element].");
        }

        [Fact]
        public void When_a_null_document_is_not_supposed_to_be_a_document_it_succeeds()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().NotBe(new XDocument());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_document_is_not_supposed_to_be_null_it_succeeds()
        {
            // Arrange
            XDocument theDocument = new();

            // Act
            Action act = () => theDocument.Should().NotBe(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_document_is_not_supposed_to_be_equal_to_null_it_fails()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().NotBe(null, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect theDocument to be <null> *failure message*.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equal_to_the_same_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<configuration></configuration>");
            var sameXDocument = theDocument;

            // Act
            Action act = () =>
                theDocument.Should().NotBe(sameXDocument, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be <configuration*> because we want to test the failure message.");
        }
    }

    public class BeEquivalentTo
    {
        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_the_same_xml_document_it_should_succeed()
        {
            // Arrange
            var document = new XDocument();
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(sameXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_a_xml_selfclosing_document_is_equivalent_to_a_different_xml_document_with_same_structure_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_same_structure_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child></child></parent>");
            var otherXDocument = XDocument.Parse("<parent><child></child></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_xml_document_with_elements_missing_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /><child2 /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected EndElement \"parent\" in theDocument at \"/parent\", but found Element \"child2\".");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_extra_elements_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var expected = XDocument.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Element \"child2\" in theDocument at \"/parent\", but found EndElement \"parent\".");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_with_selfclosing_child_is_equivalent_to_a_different_xml_document_with_subchild_child_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child><child /></child></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Element \"child\" in theDocument at \"/parent/child\", but found EndElement \"parent\".");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_elements_missing_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /><child2 /></parent>");
            var expected = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected EndElement \"parent\" in theDocument at \"/parent\" because we want to test the failure message,"
                + " but found Element \"child2\".");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_extra_elements_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var expected = XDocument.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Element \"child2\" in theDocument at \"/parent\" because we want to test the failure message,"
                + " but found EndElement \"parent\".");
        }

        [Fact]
        public void When_a_document_is_null_then_be_equivalent_to_null_succeeds()
        {
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().BeEquivalentTo(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_document_is_null_then_be_equivalent_to_a_document_fails()
        {
            XDocument theDocument = null;

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(
                    XDocument.Parse("<parent><child /></parent>"), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected theDocument to be equivalent to <null> *failure message*" +
                    ", but found \"<parent><child /></parent>\".");
        }

        [Fact]
        public void When_a_document_is_equivalent_to_null_it_fails()
        {
            XDocument theDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(null, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected theDocument to be equivalent to \"<parent><child /></parent>\" *failure message*" +
                    ", but found <null>.");
        }

        [Fact]
        public void
            When_assertion_an_xml_document_is_equivalent_to_a_different_xml_document_with_different_namespace_prefix_it_should_succeed()
        {
            // Arrange
            var subject = XDocument.Parse("<xml xmlns=\"urn:a\"/>");
            var expected = XDocument.Parse("<a:xml xmlns:a=\"urn:a\"/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_a_different_xml_document_which_differs_only_on_unused_namespace_declaration_it_should_succeed()
        {
            // Arrange
            var subject = XDocument.Parse("<xml xmlns:a=\"urn:a\"/>");
            var expected = XDocument.Parse("<xml/>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_lacks_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<xml><element b=\"1\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"b\" b=\"1\"/></xml>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected attribute \"a\" in theDocument at \"/xml/element\" because we want to test the failure message, but found none.");
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_extra_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element/></xml>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect to find attribute \"a\" in theDocument at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_different_attribute_values_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"c\"/></xml>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected attribute \"a\" in theDocument at \"/xml/element\" to have value \"c\" because we want to test the failure message, but found \"b\".");
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_attribute_with_different_namespace_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<xml><element xmlns:ns=\"urn:a\" ns:a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"b\"/></xml>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect to find attribute \"ns:a\" in theDocument at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_different_text_contents_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<xml>a</xml>");
            var expected = XDocument.Parse("<xml>b</xml>");

            // Act
            Action act = () =>
                theDocument.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected content to be \"b\" in theDocument at \"/xml\" because we want to test the failure message, but found \"a\".");
        }

        [Fact]
        public void
            When_asserting_an_xml_document_is_equivalent_to_different_xml_document_with_different_comments_it_should_succeed()
        {
            // Arrange
            var subject = XDocument.Parse("<xml><!--Comment--><a/></xml>");
            var expected = XDocument.Parse("<xml><a/><!--Comment--></xml>");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_equivalence_of_an_xml_document_but_has_different_attribute_value_it_should_fail_with_xpath_to_difference()
        {
            // Arrange
            XDocument actual = XDocument.Parse("<xml><a attr=\"x\"/><b id=\"foo\"/></xml>");
            XDocument expected = XDocument.Parse("<xml><a attr=\"x\"/><b id=\"bar\"/></xml>");

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"/xml/b\"*");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_document_with_repeating_element_names_but_differs_it_should_fail_with_index_xpath_to_difference()
        {
            // Arrange
            XDocument actual = XDocument.Parse(
                "<xml><xml2 /><xml2 /><xml2><a x=\"y\"/><b><sub /></b><a x=\"y\"/></xml2></xml>");

            XDocument expected = XDocument.Parse(
                "<xml><xml2 /><xml2 /><xml2><a x=\"y\"/><b><sub /></b><a x=\"z\"/></xml2></xml>");

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"/xml/xml2[3]/a[2]\"*");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_document_with_repeating_element_names_on_different_levels_but_differs_it_should_fail_with_index_xpath_to_difference()
        {
            // Arrange
            XDocument actual = XDocument.Parse(
                "<xml><xml /><xml /><xml><xml x=\"y\"/><xml2><xml /></xml2><xml x=\"y\"/></xml></xml>");

            XDocument expected = XDocument.Parse(
                "<xml><xml /><xml /><xml><xml x=\"y\"/><xml2><xml /></xml2><xml x=\"z\"/></xml></xml>");

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"/xml/xml[3]/xml[2]\"*");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_document_with_repeating_element_names_with_different_parents_but_differs_it_should_fail_with_index_xpath_to_difference()
        {
            // Arrange
            XDocument actual = XDocument.Parse(
                "<root><xml1 /><xml1><xml2 /><xml2 a=\"x\" /></xml1><xml1><xml2 /><xml2 a=\"x\" /></xml1></root>");

            XDocument expected = XDocument.Parse(
                "<root><xml1 /><xml1><xml2 /><xml2 a=\"x\" /></xml1><xml1><xml2 /><xml2 a=\"y\" /></xml1></root>");

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"/root/xml1[3]/xml2[2]\"*");
        }
    }

    public class NotBeEquivalentTo
    {
        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_elements_missing_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /><child2 /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(otherXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_extra_elements_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(otherXDocument);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_structure_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent, but it is.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_the_same_xml_document_it_should_fail()
        {
            // Arrange
            var theDocument = new XDocument();
            var sameXDocument = theDocument;

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(sameXDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent, but it is.");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_structure_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var otherDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(otherDocument, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_contents_but_different_ns_prefixes_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(@"<parent xmlns:ns1=""a""><ns1:child /></parent>");
            var otherXDocument = XDocument.Parse(@"<parent xmlns:ns2=""a""><ns2:child /></parent>");

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_contents_but_extra_unused_xmlns_declaration_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(@"<xml xmlns:ns1=""a"" />");
            var otherDocument = XDocument.Parse("<xml />");

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(otherDocument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent, but it is.");
        }

        [Fact]
        public void
            When_asserting_a_xml_document_is_not_equivalent_to_the_same_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<parent><child /></parent>");
            var sameXDocument = theDocument;

            // Act
            Action act = () =>
                theDocument.Should().NotBeEquivalentTo(sameXDocument, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect theDocument to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_a_null_document_is_unexpected_equivalent_to_null_it_fails()
        {
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().NotBeEquivalentTo(null, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect theDocument to be equivalent *failure message*, but it is.");
        }

        [Fact]
        public void When_a_null_document_is_not_equivalent_to_a_document_it_succeeds()
        {
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().NotBeEquivalentTo(XDocument.Parse("<parent><child /></parent>"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_document_is_not_equivalent_to_null_it_succeeds()
        {
            XDocument theDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () => theDocument.Should().NotBeEquivalentTo(null);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class BeNull
    {
        [Fact]
        public void When_asserting_a_null_xml_document_is_null_it_should_succeed()
        {
            // Arrange
            XDocument document = null;

            // Act
            Action act = () =>
                document.Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_non_null_xml_document_is_null_it_should_fail()
        {
            // Arrange
            var theDocument = new XDocument();

            // Act
            Action act = () =>
                theDocument.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be <null>, but found [XML document without root element].");
        }

        [Fact]
        public void When_asserting_a_non_null_xml_document_is_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse("<configuration></configuration>");

            // Act
            Action act = () =>
                theDocument.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to be <null> because we want to test the failure message, but found <configuration*>.");
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void When_asserting_a_non_null_xml_document_is_not_null_it_should_succeed()
        {
            // Arrange
            var document = new XDocument();

            // Act
            Action act = () =>
                document.Should().NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_not_null_it_should_fail()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () =>
                theDocument.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected theDocument not to be <null>.");
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_not_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () =>
                theDocument.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument not to be <null> because we want to test the failure message.");
        }
    }

    public class HaveRoot
    {
        [Fact]
        public void When_asserting_document_has_root_element_and_it_does_it_should_succeed_and_return_it_for_chaining()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            XElement root = document.Should().HaveRoot("parent").Subject;

            // Assert
            root.Should().BeSameAs(document.Root);
        }

        [Fact]
        public void When_asserting_document_has_root_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () => theDocument.Should().HaveRoot("unknown");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to have root element \"unknown\", but found <parent>…</parent>.");
        }

        [Fact]
        public void When_asserting_document_has_root_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveRoot("unknown", "because we want to test the failure message");

            // Assert
            string expectedMessage = "Expected theDocument to have root element \"unknown\"" +
                " because we want to test the failure message" +
                $", but found {Formatter.ToString(theDocument)}.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_a_null_document_has_root_element_it_should_fail()
        {
            // Arrange
            XDocument theDocument = null;

            // Act
            Action act = () => theDocument.Should().HaveRoot("unknown");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(
                "Cannot assert the document has a root element if the document itself is <null>.");
        }

        [Fact]
        public void When_asserting_a_document_has_a_root_element_with_a_null_name_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () => theDocument.Should().HaveRoot(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has a root element if the expected name is <null>*");
        }

        [Fact]
        public void When_asserting_a_document_has_a_root_element_with_a_null_xname_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () => theDocument.Should().HaveRoot((XName)null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has a root element if the expected name is <null>*");
        }

        [Fact]
        public void When_asserting_document_has_root_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent xmlns='http://www.example.com/2012/test'>
                    <child/>
                </parent>
                """);

            // Act
            Action act = () =>
                document.Should().HaveRoot(XName.Get("parent", "http://www.example.com/2012/test"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_document_has_root_element_with_ns_but_it_does_not_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveRoot(XName.Get("unknown", "http://www.example.com/2012/test"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to have root element \"{http://www.example.com/2012/test}unknown\", but found <parent>…</parent>.");
        }

        [Fact]
        public void When_asserting_document_has_root_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveRoot(XName.Get("unknown", "http://www.example.com/2012/test"),
                    "because we want to test the failure message");

            // Assert
            string expectedMessage =
                "Expected theDocument to have root element \"{http://www.example.com/2012/test}unknown\"" +
                " because we want to test the failure message" +
                $", but found {Formatter.ToString(theDocument)}.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }
    }

    public class HaveElement
    {
        [Fact]
        public void When_document_has_the_expected_child_element_it_should_not_throw_and_return_the_element_for_chaining()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            XElement element = document.Should().HaveElement("child").Subject;

            // Assert
            element.Should().BeSameAs(document.Element("parent").Element("child"));
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveElement("unknown");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to have root element with child \"unknown\", but no such child element was found.");
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveElement("unknown", "because we want to test the failure message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to have root element with child \"unknown\" because we want to test the failure message,"
                + " but no such child element was found.");
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent xmlns:test='http://www.example.org/2012/test'>
                    <test:child />
                </parent>
                """);

            // Act
            Action act = () =>
                document.Should().HaveElement(XName.Get("child", "http://www.example.org/2012/test"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_with_ns_but_it_does_not_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                document.Should().HaveElement(XName.Get("unknown", "http://www.example.org/2012/test"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected document to have root element with child \"{http://www.example.org/2012/test}unknown\","
                + " but no such child element was found.");
        }

        [Fact]
        public void
            When_asserting_document_has_root_with_child_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var theDocument = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () =>
                theDocument.Should().HaveElement(XName.Get("unknown", "http://www.example.org/2012/test"),
                    "because we want to test the failure message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected theDocument to have root element with child \"{http://www.example.org/2012/test}unknown\""
                + " because we want to test the failure message, but no such child element was found.");
        }

        [Fact]
        public void
            When_asserting_document_has_root_with_child_element_with_attributes_it_should_be_possible_to_use_which_to_assert_on_the_element()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child attr='1' />
                </parent>
                """);

            // Act
            XElement matchedElement = document.Should().HaveElement("child").Subject;

            // Assert
            matchedElement.Should().BeOfType<XElement>().And.HaveAttribute("attr", "1");
            matchedElement.Name.Should().Be(XName.Get("child"));
        }

        [Fact]
        public void When_asserting_a_null_document_has_an_element_it_should_fail()
        {
            // Arrange
            XDocument document = null;

            // Act
            Action act = () => document.Should().HaveElement("unknown");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(
                "Cannot assert the document has an element if the document itself is <null>.");
        }

        [Fact]
        public void When_asserting_a_document_without_root_element_has_an_element_it_should_fail()
        {
            // Arrange
            XDocument document = new();

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                document.Should().HaveElement("unknown");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected document to have root element with child \"unknown\", but it has no root element.");
        }

        [Fact]
        public void When_asserting_a_document_has_an_element_with_a_null_name_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has an element if the expected name is <null>*");
        }

        [Fact]
        public void When_asserting_a_document_has_an_element_with_a_null_xname_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement((XName)null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage(
                "Cannot assert the document has an element if the expected name is <null>*");
        }
    }

    public class HaveElementWithOccurrence
    {
        [Fact]
        public void When_asserting_document_has_two_child_elements_and_it_does_it_succeeds()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                </parent>
                """);

            // Act / Assert
            document.Should().HaveElement("child", Exactly.Twice());
        }

        [Fact]
        public void Asserting_document_null_inside_an_assertion_scope_it_checks_the_whole_assertion_scope_before_failing()
        {
            // Arrange
            XDocument document = null;

            // Act
            Action act = () =>
            {
                using (new AssertionScope())
                {
                    document.Should().HaveElement("child", Exactly.Twice());
                    document.Should().HaveElement("child", Exactly.Twice());
                }
            };

            // Assert
            act.Should().NotThrow<NullReferenceException>();
        }

        [Fact]
        public void
            Asserting_with_document_root_null_inside_an_assertion_scope_it_checks_the_whole_assertion_scope_before_failing()
        {
            // Arrange
            XDocument document = new();

            // Act
            Action act = () =>
            {
                using (new AssertionScope())
                {
                    document.Should().HaveElement("child", Exactly.Twice());
                    document.Should().HaveElement("child", Exactly.Twice());
                }
            };

            // Assert
            act.Should().NotThrow<NullReferenceException>();
        }

        [Fact]
        public void When_asserting_document_has_two_child_elements_but_it_does_have_three_it_fails()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement("child", Exactly.Twice());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected document to have a root element containing a child \"child\"*exactly*2 times, but found it 3 times*");
        }

        [Fact]
        public void Document_is_valid_and_expected_null_with_string_overload_it_fails()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement(null, Exactly.Twice());

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has an element if the expected name is <null>.*");
        }

        [Fact]
        public void Document_is_valid_and_expected_null_with_x_name_overload_it_fails()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement((XName)null, Exactly.Twice());

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has an element count if the element name is <null>.*");
        }

        [Fact]
        public void Chaining_after_a_successful_occurrence_check_does_continue_the_assertion()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                    <child />
                </parent>
                """);

            // Act / Assert
            document.Should().HaveElement("child", AtLeast.Twice())
                .Which.Should().NotBeNull();
        }

        [Fact]
        public void Chaining_after_a_non_successful_occurrence_check_does_not_continue_the_assertion()
        {
            // Arrange
            var document = XDocument.Parse(
                """
                <parent>
                    <child />
                    <child />
                    <child />
                </parent>
                """);

            // Act
            Action act = () => document.Should().HaveElement("child", Exactly.Once())
                .Which.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected document to have a root element containing a child \"child\"*exactly*1 time, but found it 3 times.");
        }

        [Fact]
        public void When_asserting_a_null_document_to_have_an_element_count_it_should_fail()
        {
            // Arrange
            XDocument xDocument = null;

            // Act
            Action act = () => xDocument.Should().HaveElement("child", AtLeast.Once());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Cannot assert the count if the document itself is <null>.");
        }
    }
}
