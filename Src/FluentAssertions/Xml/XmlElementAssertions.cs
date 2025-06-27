﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Xml;

/// <summary>
/// Contains a number of methods to assert that an <see cref="XmlElement"/>
/// is in the expected state./>
/// </summary>
[DebuggerNonUserCode]
public class XmlElementAssertions : XmlNodeAssertions<XmlElement, XmlElementAssertions>
{
    private readonly AssertionChain assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlElementAssertions"/> class.
    /// </summary>
    public XmlElementAssertions(XmlElement xmlElement, AssertionChain assertionChain)
        : base(xmlElement, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlElement"/> has the specified
    /// <paramref name="expected"/> inner text.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XmlElementAssertions> HaveInnerText(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject.InnerText == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have value {0}{reason}, but found {1}.",
                expected, Subject.InnerText);

        return new AndConstraint<XmlElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlElement"/> has an attribute
    /// with the specified <paramref name="expectedName"/>
    /// and <paramref name="expectedValue"/>.
    /// </summary>
    /// <param name="expectedName">The name of the expected attribute</param>
    /// <param name="expectedValue">The value of the expected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XmlElementAssertions> HaveAttribute(string expectedName, string expectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveAttributeWithNamespace(expectedName, string.Empty, expectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlElement"/> has an attribute
    /// with the specified <paramref name="expectedName"/>, <paramref name="expectedNamespace"/>
    /// and <paramref name="expectedValue"/>.
    /// </summary>
    /// <param name="expectedName">The name of the expected attribute</param>
    /// <param name="expectedNamespace">The namespace of the expected attribute</param>
    /// <param name="expectedValue">The value of the expected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XmlElementAssertions> HaveAttributeWithNamespace(
        string expectedName,
        string expectedNamespace,
        string expectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        XmlAttribute attribute = Subject.Attributes[expectedName, expectedNamespace];

        string expectedFormattedName =
            (string.IsNullOrEmpty(expectedNamespace) ? string.Empty : $"{{{expectedNamespace}}}")
            + expectedName;

        assertionChain
            .ForCondition(attribute is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have attribute {0}"
                + " with value {1}{reason}, but found no such attribute in {2}",
                expectedFormattedName, expectedValue, Subject);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(attribute!.Value == expectedValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected attribute {0} in {context:subject} to have value {1}{reason}, but found {2}.",
                    expectedFormattedName, expectedValue, attribute.Value);
        }

        return new AndConstraint<XmlElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
    /// <paramref name="expectedName"/> name, ignoring the namespace.
    /// </summary>
    /// <param name="expectedName">The name of the expected child element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElement(
        string expectedName,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveElementWithNamespace(expectedName, null, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
    /// <paramref name="expectedName"/> name and <paramref name="expectedNamespace" /> namespace.
    /// </summary>
    /// <param name="expectedName">The name of the expected child element</param>
    /// <param name="expectedNamespace">The namespace of the expected child element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElementWithNamespace(
        string expectedName,
        string expectedNamespace,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        XmlElement element = expectedNamespace == null ? Subject[expectedName] : Subject[expectedName, expectedNamespace];

        string expectedFormattedName =
            (string.IsNullOrEmpty(expectedNamespace) ? string.Empty : $"{{{expectedNamespace}}}")
            + expectedName;

        assertionChain
            .ForCondition(element is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have child element {0}{reason}, but no such child element was found.",
                expectedFormattedName.EscapePlaceholders());

        return new AndWhichConstraint<XmlElementAssertions, XmlElement>(this, element, assertionChain, "/" + expectedName);
    }

    protected override string Identifier => "XML element";
}
