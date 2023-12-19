using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class XmlSpecs
{
    [Fact]
    public async Task
        When_asserting_a_xml_selfclosing_document_is_equivalent_to_a_different_xml_document_with_same_structure_it_should_succeed()
    {
        // Arrange
        var subject = new
        {
            Document = XDocument.Parse("<parent><child /></parent>")
        };

        var expectation = new
        {
            Document = XDocument.Parse("<parent><child /></parent>")
        };

        // Act / Assert
        await subject.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task When_asserting_a_xml_document_is_equivalent_to_a_xml_document_with_elements_missing_it_should_fail()
    {
        var subject = new
        {
            Document = XDocument.Parse("<parent><child /><child2 /></parent>")
        };

        var expectation = new
        {
            Document = XDocument.Parse("<parent><child /></parent>")
        };

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected EndElement \"parent\" in property subject.Document at \"/parent\", but found Element \"child2\".*");
    }

    [Fact]
    public async Task When_xml_elements_are_equivalent_it_should_not_throw()
    {
        // Arrange
        var subject = new
        {
            Element = XElement.Parse("<parent><child /></parent>")
        };

        var expectation = new
        {
            Element = XElement.Parse("<parent><child /></parent>")
        };

        // Act / Assert
        await subject.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task When_an_xml_element_property_is_equivalent_to_an_xml_element_with_elements_missing_it_should_fail()
    {
        // Arrange
        var subject = new
        {
            Element = XElement.Parse("<parent><child /><child2 /></parent>")
        };

        var expectation = new
        {
            Element = XElement.Parse("<parent><child /></parent>")
        };

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected EndElement \"parent\" in property subject.Element at \"/parent\", but found Element \"child2\"*");
    }

    [Fact]
    public async Task When_asserting_an_xml_attribute_is_equal_to_the_same_xml_attribute_it_should_succeed()
    {
        var subject = new
        {
            Attribute = new XAttribute("name", "value")
        };

        var expectation = new
        {
            Attribute = new XAttribute("name", "value")
        };

        // Act / Assert
        await subject.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task When_asserting_an_xml_attribute_is_equal_to_a_different_xml_attribute_it_should_fail_with_descriptive_message()
    {
        // Arrange
        var subject = new
        {
            Attribute = new XAttribute("name", "value")
        };

        var expectation = new
        {
            Attribute = new XAttribute("name2", "value")
        };

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expectation, "because we want to test the failure {0}", "message");

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected property subject.Attribute to be name2=\"value\" because we want to test the failure message, but found name=\"value\"*");
    }
}
