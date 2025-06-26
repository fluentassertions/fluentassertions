using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using FluentAssertions.Xml.Equivalency;

namespace FluentAssertions.Xml;

/// <summary>
/// Contains a number of methods to assert that an <see cref="XElement"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class XElementAssertions : ReferenceTypeAssertions<XElement, XElementAssertions>
{
    private readonly AssertionChain assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="XElementAssertions" /> class.
    /// </summary>
    public XElementAssertions(XElement xElement, AssertionChain assertionChain)
        : base(xElement, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> equals the
    /// <paramref name="expected"/> element, by using
    /// <see cref="XNode.DeepEquals(XNode, XNode)"/>
    /// </summary>
    /// <param name="expected">The expected element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> Be(XElement expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(XNode.DeepEquals(Subject, expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:subject} to be {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> does not equal the
    /// <paramref name="unexpected"/> element, using
    /// <see cref="XNode.DeepEquals(XNode, XNode)" />.
    /// </summary>
    /// <param name="unexpected">The unexpected element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotBe(XElement unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition((Subject is null && unexpected is not null) || !XNode.DeepEquals(Subject, unexpected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:subject} to be {0}{reason}.", unexpected);

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> is equivalent to the
    /// <paramref name="expected"/> element, using a semantic equivalency
    /// comparison.
    /// </summary>
    /// <param name="expected">The expected element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> BeEquivalentTo(XElement expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader expectedReader = expected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, expectedReader, because, becauseArgs);
            xmlReaderValidator.AssertThatItIsEquivalent();
        }

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> is not equivalent to
    /// the <paramref name="unexpected"/> element, using a semantic
    /// equivalency comparison.
    /// </summary>
    /// <param name="unexpected">The unexpected element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotBeEquivalentTo(XElement unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader otherReader = unexpected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, otherReader, because, becauseArgs);
            xmlReaderValidator.AssertThatIsNotEquivalent();
        }

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has the specified <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> HaveValue(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the element to have value {0}{reason}, but {context:member} is <null>.", expected);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject!.Value == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} '{0}' to have value {1}{reason}, but found {2}.",
                    Subject.Name, expected, Subject.Value);
        }

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>.
    /// </summary>
    /// <param name="expectedName">The name of the expected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expectedName"/> is empty.</exception>
    public AndConstraint<XElementAssertions> HaveAttribute(string expectedName,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(expectedName);

        return HaveAttribute(XNamespace.None + expectedName, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>.
    /// </summary>
    /// <param name="expectedName">The name <see cref="XName"/> of the expected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedName"/> is <see langword="null"/>.</exception>
    public AndConstraint<XElementAssertions> HaveAttribute(XName expectedName,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedName);

        string expectedText = expectedName.ToString();

        assertionChain
            .WithExpectation("Expected attribute {0} in element to exist {reason}, ", expectedText,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject is not null)
                    .FailWith(
                        "but {context:member} is <null>."))
            .Then
            .WithExpectation("Expected {context:subject} to have attribute {0}{reason}, ", expectedText,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() => Subject!.Attribute(expectedName))
                    .ForCondition(attribute => attribute is not null)
                    .FailWith("but found no such attribute in {0}.", Subject));

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> doesn't have an attribute with the specified <paramref name="unexpectedName"/>.
    /// </summary>
    /// <param name="unexpectedName">The name of the unexpected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpectedName"/> is empty.</exception>
    public AndConstraint<XElementAssertions> NotHaveAttribute(string unexpectedName,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(unexpectedName);

        return NotHaveAttribute(XNamespace.None + unexpectedName, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> doesn't have an attribute with the specified <paramref name="unexpectedName"/>.
    /// </summary>
    /// <param name="unexpectedName">The name <see cref="XName"/> of the unexpected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedName"/> is <see langword="null"/>.</exception>
    public AndConstraint<XElementAssertions> NotHaveAttribute(XName unexpectedName,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedName);

        string unexpectedText = unexpectedName.ToString();

        assertionChain
            .WithExpectation("Did not expect attribute {0} in element to exist{reason}, ", unexpectedText,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject is not null)
                    .FailWith(
                        "but {context:member} is <null>.",
                        unexpectedText))
            .Then
            .WithExpectation("Did not expect {context:subject} to have attribute {0}{reason}, ", unexpectedText,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() => Subject!.Attribute(unexpectedName))
                    .ForCondition(attribute => attribute is null)
                    .FailWith("but found such attribute in {0}.", Subject));

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
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
    /// <exception cref="ArgumentNullException"><paramref name="expectedName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expectedName"/> is empty.</exception>
    public AndConstraint<XElementAssertions> HaveAttributeWithValue(string expectedName, string expectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(expectedName);

        return HaveAttributeWithValue(XNamespace.None + expectedName, expectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
    /// and <paramref name="expectedValue"/>.
    /// </summary>
    /// <param name="expectedName">The name <see cref="XName"/> of the expected attribute</param>
    /// <param name="expectedValue">The value of the expected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedName"/> is <see langword="null"/>.</exception>
    public AndConstraint<XElementAssertions> HaveAttributeWithValue(XName expectedName, string expectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedName);

        string expectedText = expectedName.ToString();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected attribute {0} in element to have value {1}{reason}, ",
                expectedText, expectedValue,
                chain => chain
                    .ForCondition(Subject is not null)
                    .FailWith("but {context:member} is <null>."))
            .Then
            .WithExpectation("Expected {context:subject} to have attribute {0} with value {1}{reason}, ",
                expectedText, expectedValue,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() => Subject!.Attribute(expectedName))
                    .ForCondition(attr => attr is not null)
                    .FailWith("but found no such attribute in {0}", Subject))
            .Then
            .WithExpectation("Expected attribute {0} in {context:subject} to have value {1}{reason}, ",
                expectedText, expectedValue,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() => Subject!.Attribute(expectedName))
                    .ForCondition(attr => attr!.Value == expectedValue)
                    .FailWith("but found {0}.", attr => attr.Value));

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> doesn't have an attribute with the specified <paramref name="unexpectedName"/>
    /// and/or <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedName">The name of the unexpected attribute</param>
    /// <param name="unexpectedValue">The value of the unexpected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpectedName"/> is empty.</exception>
    public AndConstraint<XElementAssertions> NotHaveAttributeWithValue(string unexpectedName, string unexpectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(unexpectedName, nameof(unexpectedName));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        return NotHaveAttributeWithValue(XNamespace.None + unexpectedName, unexpectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> doesn't have an attribute with the specified <paramref name="unexpectedName"/>
    /// and/or <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedName">The name <see cref="XName"/> of the unexpected attribute</param>
    /// <param name="unexpectedValue">The value of the unexpected attribute</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedName"/> is <see langword="null"/>.</exception>
    public AndConstraint<XElementAssertions> NotHaveAttributeWithValue(XName unexpectedName, string unexpectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedName, nameof(unexpectedName));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        string unexpectedText = unexpectedName.ToString();

        assertionChain
            .WithExpectation("Did not expect attribute {0} in element to have value {1}{reason}, ",
                unexpectedText, unexpectedValue,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject is not null)
                    .FailWith("but {context:member} is <null>."))
            .Then
            .WithExpectation("Did not expect {context:subject} to have attribute {0} with value {1}{reason}, ",
                unexpectedText, unexpectedValue,
                chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() => Subject!.Attributes()
                        .FirstOrDefault(a => a.Name == unexpectedName && a.Value == unexpectedValue))
                    .ForCondition(attribute => attribute is null)
                    .FailWith("but found such attribute in {0}.", Subject));

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
    /// <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">The name <see cref="XName"/> of the expected child element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndWhichConstraint<XElementAssertions, XElement> HaveElement(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(expected);

        return HaveElement(XNamespace.None + expected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
    /// <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">The name <see cref="XName"/> of the expected child element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XElementAssertions, XElement> HaveElement(XName expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                "Expected the element to have child element {0}{reason}, but {context:member} is <null>.",
                expected.ToString().EscapePlaceholders());

        XElement xElement = null;

        if (assertionChain.Succeeded)
        {
            xElement = Subject!.Element(expected);

            assertionChain
                .ForCondition(xElement is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have child element {0}{reason}, but no such child element was found.",
                    expected.ToString().EscapePlaceholders());
        }

        return new AndWhichConstraint<XElementAssertions, XElement>(this, xElement, assertionChain, "/" + expected);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> has the specified occurrence of
    /// child elements with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The full name <see cref="XName"/> of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="occurrenceConstraint">
    /// A constraint specifying the number of times the specified elements should appear.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XElementAssertions, IEnumerable<XElement>> HaveElement(XName expected,
        OccurrenceConstraint occurrenceConstraint,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the element has an element count if the element name is <null>.");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have an element with count of {0}{reason}, but the element itself is <null>.",
                expected.ToString());

        IEnumerable<XElement> xElements = [];

        if (assertionChain.Succeeded)
        {
            xElements = Subject!.Elements(expected);
            int actual = xElements.Count();

            assertionChain
                .ForConstraint(occurrenceConstraint, actual)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have an element {0} {expectedOccurrence}" +
                    $"{{reason}}, but found it {actual.Times()}.",
                    expected.ToString());
        }

        return new AndWhichConstraint<XElementAssertions, IEnumerable<XElement>>(this, xElements, assertionChain, "/" + expected);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> has the specified occurrence of
    /// child elements with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The name of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="occurrenceConstraint">
    /// A constraint specifying the number of times the specified elements should appear.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XElementAssertions, IEnumerable<XElement>> HaveElement(string expected,
        OccurrenceConstraint occurrenceConstraint,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the element has an element if the expected name is <null>.");

        return HaveElement(XNamespace.None + expected, occurrenceConstraint, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> doesn't have the specified child element.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The name of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotHaveElement(string unexpectedElement,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));

        return NotHaveElement(XNamespace.None + unexpectedElement, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> doesn't have the specified child element.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The full name <see cref="XName"/> of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotHaveElement(XName unexpectedElement,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Did not expect {context:subject} to have an element {0}{reason}, but the element itself is <null>.",
                unexpectedElement.ToString());

        if (assertionChain.Succeeded)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!Subject!.Elements(unexpectedElement).Any())
                .FailWith("Did not expect {context:subject} to have an element {0}{reason}, " +
                    "but the element {0} was found.", unexpectedElement);
        }

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> has the specified child element
    /// with the specified <paramref name="expectedValue"/> name.
    /// </summary>
    /// <param name="expectedElement">
    /// The name of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="expectedValue">
    /// The expected value of this particular element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<XElementAssertions, XElement> HaveElementWithValue(string expectedElement,
        string expectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedElement, nameof(expectedElement));
        Guard.ThrowIfArgumentIsNull(expectedValue, nameof(expectedValue));

        return HaveElementWithValue(XNamespace.None + expectedElement, expectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> has the specified child element
    /// with the specified <paramref name="expectedValue"/> name.
    /// </summary>
    /// <param name="expectedElement">
    /// The full name <see cref="XName"/> of the expected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="expectedValue">
    /// The expected value of this particular element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<XElementAssertions, XElement> HaveElementWithValue(XName expectedElement,
        string expectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedElement, nameof(expectedElement));
        Guard.ThrowIfArgumentIsNull(expectedValue, nameof(expectedValue));

        IEnumerable<XElement> xElements = [];

        assertionChain
            .WithExpectation("Expected {context:subject} to have an element {0} with value {1}{reason}, ",
                expectedElement.ToString(), expectedValue,
                chain => chain
                    .ForCondition(Subject is not null)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("but the element itself is <null>."))
            .Then
            .WithExpectation("Expected {context:subject} to have an element {0} with value {1}{reason}, ",
                expectedElement, expectedValue, chain => chain
                    .BecauseOf(because, becauseArgs)
                    .Given(() =>
                    {
                        xElements = Subject!.Elements(expectedElement);

                        return xElements;
                    })
                    .ForCondition(elements => elements.Any())
                    .FailWith("but the element {0} isn't found.", expectedElement)
                    .Then
                    .ForCondition(elements => elements.Any(e => e.Value == expectedValue))
                    .FailWith("but the element {0} does not have such a value.", expectedElement));

        return new AndWhichConstraint<XElementAssertions, XElement>(this, xElements.FirstOrDefault());
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> either doesn't have the
    /// specified child element or doesn't have the specified <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The name of the unexpected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="unexpectedValue">
    /// The unexpected value of this particular element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotHaveElementWithValue(string unexpectedElement,
        string unexpectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        return NotHaveElementWithValue(XNamespace.None + unexpectedElement, unexpectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XElement"/> of the current <see cref="XElement"/> either doesn't have the
    /// specified child element or doesn't have the specified <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedElement">
    /// he full name <see cref="XName"/> of the unexpected child element of the current element's <see cref="XElement"/>.
    /// </param>
    /// <param name="unexpectedValue">
    /// The unexpected value of this particular element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XElementAssertions> NotHaveElementWithValue(XName unexpectedElement,
        string unexpectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        assertionChain
            .WithExpectation("Did not expect {context:subject} to have an element {0} with value {1}{reason}, ",
                unexpectedElement.ToString(), unexpectedValue,
                chain => chain
                    .ForCondition(Subject is not null)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("but the element itself is <null>.")
                    .Then
                    .ForCondition(!Subject!.Elements(unexpectedElement)
                        .Any(e => e.Value == unexpectedValue))
                    .FailWith("but the element {0} does have this value.", unexpectedElement));

        return new AndConstraint<XElementAssertions>(this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "XML element";
}
