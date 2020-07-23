using System;
using System.Xml.Linq;

using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class XDocumentAssertionSpecs
    {
        #region Be / NotBe

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
            var document = new XDocument();
            var otherXDocument = new XDocument();

            // Act
            Action act = () =>
                document.Should().Be(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_equal_to_another_xml_document_it_should_fail()
        {
            // Arrange
            XDocument document = null;
            var otherXDocument = new XDocument();

            // Act
            Action act = () =>
                document.Should().Be(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_null_xml_document_is_equal_to_a_null_xml_document_it_should_succeed()
        {
            // Arrange
            XDocument document = null;
            XDocument otherXDocument = null;

            // Act
            Action act = () =>
                document.Should().Be(otherXDocument);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equal_to_a_different_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<configuration></configuration>");
            var otherXDocument = XDocument.Parse("<data></data>");

            // Act
            Action act = () =>
                document.Should().Be(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected XML document to be <data></data>" +
                    " because we want to test the failure message," +
                        " but found <configuration></configuration>.");
        }

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
            var document = new XDocument();
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().NotBe(sameXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_not_equal_to_some_xml_document_it_should_succeed()
        {
            // Arrange
            XDocument document = null;
            var someXDocument = new XDocument();

            // Act
            Action act = () =>
                document.Should().NotBe(someXDocument);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_not_equal_to_a_null_xml_document_it_should_fail()
        {
            // Arrange
            XDocument document = null;
            XDocument someXDocument = null;

            // Act
            Action act = () =>
                document.Should().NotBe(someXDocument);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect XML document to be <null>.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equal_to_the_same_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<configuration></configuration>");
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().NotBe(sameXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect XML document to be <configuration></configuration>" +
                    " because we want to test the failure message.");
        }

        #endregion

        #region BeEquivalentTo / NotBeEquivalentTo

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
        public void When_asserting_a_xml_selfclosing_document_is_equivalent_to_a_different_xml_document_with_same_structure_it_should_succeed()
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
        public void When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_same_structure_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var document = XDocument.Parse("<parent><child></child></parent>");
            var otherXDocument = XDocument.Parse("<parent><child></child></parent>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_xml_document_with_elements_missing_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /><child2 /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_extra_elements_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_with_selfclosing_child_is_equivalent_to_a_different_xml_document_with_subchild_child_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child><child /></child></parent>");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("Expected node of type Element at \"/parent\", but found EndElement.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_elements_missing_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /><child2 /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected node of type EndElement at \"/parent\" because we want to test the failure message, but found Element.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_equivalent_to_a_different_xml_document_with_extra_elements_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /><child2 /></parent>");

            // Act
            Action act = () =>
                document.Should().BeEquivalentTo(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected node of type Element at \"/parent\" because we want to test the failure message, but found EndElement.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_elements_missing_it_should_succeed()
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
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_extra_elements_it_should_succeed()
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
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_structure_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(otherXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_the_same_xml_document_it_should_fail()
        {
            // Arrange
            var document = new XDocument();
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(sameXDocument);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_structure_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var otherXDocument = XDocument.Parse("<parent><child /></parent>");

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect Xml to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_contents_but_different_ns_prefixes_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(@"<parent xmlns:ns1=""a""><ns1:child /></parent>");
            var otherXDocument = XDocument.Parse(@"<parent xmlns:ns2=""a""><ns2:child /></parent>");

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(otherXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_a_different_xml_document_with_same_contents_but_extra_unused_xmlns_declaration_it_should_fail()
        {
            // Arrange
            var element = XDocument.Parse(@"<xml xmlns:ns1=""a"" />");
            var otherXElement = XDocument.Parse("<xml />");

            // Act
            Action act = () =>
                element.Should().NotBeEquivalentTo(otherXElement);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_xml_document_is_not_equivalent_to_the_same_xml_document_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<parent><child /></parent>");
            var sameXDocument = document;

            // Act
            Action act = () =>
                document.Should().NotBeEquivalentTo(sameXDocument, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect Xml to be equivalent because we want to test the failure message, but it is.");
        }

        [Fact]
        public void When_assertion_an_xml_document_is_equivalent_to_a_different_xml_document_with_different_namespace_prefix_it_should_succeed()
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
        public void When_asserting_an_xml_document_is_equivalent_to_a_different_xml_document_which_differs_only_on_unused_namespace_declaration_it_should_succeed()
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
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_lacks_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XDocument.Parse("<xml><element b=\"1\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"b\" b=\"1\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" because we want to test the failure message, but found none.");
        }

        [Fact]
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_extra_attributes_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XDocument.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_different_attribute_values_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XDocument.Parse("<xml><element a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"c\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected attribute \"a\" at \"/xml/element\" to have value \"c\" because we want to test the failure message, but found \"b\".");
        }

        [Fact]
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_attribute_with_different_namespace_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XDocument.Parse("<xml><element xmlns:ns=\"urn:a\" ns:a=\"b\"/></xml>");
            var expected = XDocument.Parse("<xml><element a=\"b\"/></xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Did not expect to find attribute \"ns:a\" at \"/xml/element\" because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_which_has_different_text_contents_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var subject = XDocument.Parse("<xml>a</xml>");
            var expected = XDocument.Parse("<xml>b</xml>");

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().
                WithMessage("Expected content to be \"b\" at \"/xml\" because we want to test the failure message, but found \"a\".");
        }

        [Fact]
        public void When_asserting_an_xml_document_is_equivalent_to_different_xml_document_with_different_comments_it_should_succeed()
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
        public void When_asserting_equivalence_of_an_xml_document_but_has_different_attribute_value_it_should_fail_with_xpath_to_difference()
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
        public void When_asserting_equivalence_of_document_with_repeating_element_names_but_differs_it_should_fail_with_index_xpath_to_difference()
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
        public void When_asserting_equivalence_of_document_with_repeating_element_names_on_different_levels_but_differs_it_should_fail_with_index_xpath_to_difference()
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
        public void When_asserting_equivalence_of_document_with_repeating_element_names_with_different_parents_but_differs_it_should_fail_with_index_xpath_to_difference()
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

        #endregion

        #region BeNull / NotBeNull

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
            var document = new XDocument();

            // Act
            Action act = () =>
                document.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_non_null_xml_document_is_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse("<configuration></configuration>");

            // Act
            Action act = () =>
                document.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected document to be <null> because we want to test the failure message, but found <configuration></configuration>.");
        }

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
            XDocument document = null;

            // Act
            Action act = () =>
                document.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_null_xml_document_is_not_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            XDocument document = null;

            // Act
            Action act = () =>
                document.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected document not to be <null> because we want to test the failure message.");
        }

        #endregion

        #region HaveRoot

        [Fact]
        public void When_asserting_document_has_root_element_and_it_does_it_should_succeed_and_return_it_for_chaining()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            XElement root = document.Should().HaveRoot("parent").Subject;

            // Assert
            root.Should().BeSameAs(document.Root);
        }

        [Fact]
        public void When_asserting_document_has_root_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () => document.Should().HaveRoot("unknown");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_document_has_root_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveRoot("unknown", "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML document to have root element \"unknown\"" +
                " because we want to test the failure message" +
                    ", but found {0}.", Formatter.ToString(document));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_a_null_document_has_root_element_it_should_fail()
        {
            // Arrange
            XDocument document = null;

            // Act
            Action act = () => document.Should().HaveRoot("unknown");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(
                "Cannot assert the document has a root element if the document itself is <null>.");
        }

        [Fact]
        public void When_asserting_a_document_has_a_null_root_element_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () => document.Should().HaveRoot(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has a root element if the element name is <null>*");
        }

        [Fact]
        public void When_asserting_document_has_root_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent xmlns='http://www.example.com/2012/test'>
                    <child/>
                  </parent>");

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
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveRoot(XName.Get("unknown", "http://www.example.com/2012/test"));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_document_has_root_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveRoot(XName.Get("unknown", "http://www.example.com/2012/test"), "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML document to have root element \"{{http://www.example.com/2012/test}}unknown\"" +
                " because we want to test the failure message" +
                    ", but found {0}.", Formatter.ToString(document));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }
        #endregion

        #region HaveElement

        [Fact]
        public void When_document_has_the_expected_child_element_it_should_not_throw_and_return_the_element_for_chaining()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            XElement element = document.Should().HaveElement("child").Subject;

            // Assert
            element.Should().BeSameAs(document.Element("parent").Element("child"));
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_but_it_does_not_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveElement("unknown");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveElement("unknown", "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML document {0} to have root element with child \"unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(document));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_with_ns_and_it_does_it_should_succeed()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent xmlns:test='http://www.example.org/2012/test'>
                    <test:child />
                  </parent>");

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
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveElement(XName.Get("unknown", "http://www.example.org/2012/test"));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () =>
                document.Should().HaveElement(XName.Get("unknown", "http://www.example.org/2012/test"),
                "because we want to test the failure message");

            // Assert
            string expectedMessage = string.Format("Expected XML document {0} to have root element with child \"{{http://www.example.org/2012/test}}unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(document));

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_document_has_root_with_child_element_with_attributes_it_should_be_possible_to_use_which_to_assert_on_the_element()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child attr='1' />
                  </parent>");

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
        public void When_asserting_a_document_has_a_null_element_it_should_fail()
        {
            // Arrange
            var document = XDocument.Parse(
                @"<parent>
                    <child />
                  </parent>");

            // Act
            Action act = () => document.Should().HaveElement(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot assert the document has an element if the element name is <null>*");
        }

        #endregion
    }
}
