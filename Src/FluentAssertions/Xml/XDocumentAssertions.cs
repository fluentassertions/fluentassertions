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
/// Contains a number of methods to assert that an <see cref="XDocument"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class XDocumentAssertions : ReferenceTypeAssertions<XDocument, XDocumentAssertions>
{
    private readonly AssertionChain assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="XDocumentAssertions" /> class.
    /// </summary>
    public XDocumentAssertions(XDocument document, AssertionChain assertionChain)
        : base(document, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> equals the <paramref name="expected"/> document,
    /// using its <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected document</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> Be(XDocument expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Equals(Subject, expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:subject} to be {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> does not equal the <paramref name="unexpected"/> document,
    /// using its <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected document</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Equals(Subject, unexpected))
            .FailWith("Did not expect {context:subject} to be {0}{reason}.", unexpected);

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> is equivalent to the <paramref name="expected"/> document,
    /// using its <see cref="XNode.DeepEquals(XNode, XNode)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected document</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> BeEquivalentTo(XDocument expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader otherReader = expected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, otherReader, because, becauseArgs);
            xmlReaderValidator.AssertThatItIsEquivalent();
        }

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> is not equivalent to the <paramref name="unexpected"/> document,
    /// using its <see cref="XNode.DeepEquals(XNode, XNode)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected document</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> NotBeEquivalentTo(XDocument unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader otherReader = unexpected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, otherReader, because, becauseArgs);
            xmlReaderValidator.AssertThatIsNotEquivalent();
        }

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
    /// <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">The name of the expected root element of the current document.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveRoot(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has a root element if the expected name is <null>.");

        return HaveRoot(XNamespace.None + expected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
    /// <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">The full name <see cref="XName"/> of the expected root element of the current document.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveRoot(XName expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (Subject is null)
        {
            throw new InvalidOperationException(
                "Cannot assert the document has a root element if the document itself is <null>.");
        }

        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has a root element if the expected name is <null>.");

        XElement root = Subject.Root;

        assertionChain
            .ForCondition(root is not null && root.Name == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have root element {0}{reason}, but found {1}.",
                expected.ToString(), Subject);

        return new AndWhichConstraint<XDocumentAssertions, XElement>(this, root, assertionChain, $"/{expected}");
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
    /// child element with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The name of the expected child element of the current document's <see cref="XDocument.Root"/> element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element if the expected name is <null>.");

        return HaveElement(XNamespace.None + expected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has the specified occurrence of
    /// child elements with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The name of the expected child element of the current document's <see cref="XDocument.Root"/> element.
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
    public AndWhichConstraint<XDocumentAssertions, IEnumerable<XElement>> HaveElement(string expected,
        OccurrenceConstraint occurrenceConstraint,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element if the expected name is <null>.");

        return HaveElement(XNamespace.None + expected, occurrenceConstraint, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
    /// child element with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The full name <see cref="XName"/> of the expected child element of the current document's <see cref="XDocument.Root"/> element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(XName expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (Subject is null)
        {
            throw new InvalidOperationException("Cannot assert the document has an element if the document itself is <null>.");
        }

        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element if the expected name is <null>.");

        assertionChain
            .ForCondition(Subject.Root is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have root element with child {0}{reason}, but it has no root element.",
                expected.ToString());

        XElement xElement = null;

        if (assertionChain.Succeeded)
        {
            xElement = Subject.Root!.Element(expected);

            assertionChain
                .ForCondition(xElement is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have root element with child {0}{reason}, but no such child element was found.",
                    expected.ToString());
        }

        return new AndWhichConstraint<XDocumentAssertions, XElement>(this, xElement, assertionChain, "/" + expected);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has the specified occurrence of
    /// child elements with the specified <paramref name="expected"/> name.
    /// </summary>
    /// <param name="expected">
    /// The full name <see cref="XName"/> of the expected child element of the current document's <see cref="XDocument.Root"/> element.
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
    public AndWhichConstraint<XDocumentAssertions, IEnumerable<XElement>> HaveElement(XName expected,
        OccurrenceConstraint occurrenceConstraint,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element count if the element name is <null>.");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Cannot assert the count if the document itself is <null>.");

        IEnumerable<XElement> xElements = [];

        if (assertionChain.Succeeded)
        {
            var root = Subject!.Root;

            assertionChain
                .ForCondition(root is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have root element containing a child {0}{reason}, but it has no root element.",
                    expected.ToString());

            if (assertionChain.Succeeded)
            {
                xElements = root!.Elements(expected);
                int actual = xElements.Count();

                assertionChain
                    .ForConstraint(occurrenceConstraint, actual)
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:subject} to have a root element containing a child {0} " +
                        $"{{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                        expected.ToString());
            }
        }

        return new AndWhichConstraint<XDocumentAssertions, IEnumerable<XElement>>(this, xElements, assertionChain,
            "/" + expected);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> doesn't have the specified child element.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The name of the expected child element of the current element's <see cref="XDocument"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> NotHaveElement(string unexpectedElement,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));

        return NotHaveElement(XNamespace.None + unexpectedElement, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> doesn't have the specified child element.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The full name <see cref="XName"/> of the expected child element of the current element's <see cref="XDocument"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<XDocumentAssertions> NotHaveElement(XName unexpectedElement,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect {context:subject} to have an element {0}{reason}, ", unexpectedElement, chain =>
                chain
                    .ForCondition(Subject is not null)
                    .FailWith("but the element itself is <null>.")
                    .Then
                    .ForCondition(!Subject!.Root!.Elements(unexpectedElement).Any())
                    .FailWith(" but the element {0} was found.", unexpectedElement));

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> has the specified child element
    /// with the specified <paramref name="expectedValue"/> name.
    /// </summary>
    /// <param name="expectedElement">
    /// The name of the expected child element of the current element's <see cref="XDocument"/>.
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElementWithValue(string expectedElement,
        string expectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedElement, nameof(expectedElement));
        Guard.ThrowIfArgumentIsNull(expectedValue, nameof(expectedValue));

        return HaveElementWithValue(XNamespace.None + expectedElement, expectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> has the specified child element
    /// with the specified <paramref name="expectedValue"/> name.
    /// </summary>
    /// <param name="expectedElement">
    /// The full name <see cref="XName"/> of the expected child element of the current element's <see cref="XDocument"/>.
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElementWithValue(XName expectedElement,
        string expectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedElement, nameof(expectedElement));
        Guard.ThrowIfArgumentIsNull(expectedValue, nameof(expectedValue));

        IEnumerable<XElement> xElements = [];

        assertionChain
            .WithExpectation("Expected {context:subject} to have an element {0} with value {1}{reason}, ",
                expectedElement.ToString(), expectedValue, chain => chain
                    .ForCondition(Subject is not null)
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "but the element itself is <null>.",
                        expectedElement.ToString(), expectedValue)
                    .Then
                    .Given(() =>
                    {
                        xElements = Subject!.Root!.Elements(expectedElement).ToList();

                        return xElements;
                    })
                    .ForCondition(collection => collection.Any())
                    .FailWith("but the element {0} isn't found.", expectedElement)
                    .Then
                    .ForCondition(collection => collection.Any(e => e.Value == expectedValue))
                    .FailWith("but the element {0} does not have such a value.", expectedElement));

        return new AndWhichConstraint<XDocumentAssertions, XElement>(this, xElements.FirstOrDefault());
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> either doesn't have the
    /// specified child element or doesn't have the specified <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedElement">
    /// The name of the unexpected child element of the current element's <see cref="XDocument"/>.
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
    public AndConstraint<XDocumentAssertions> NotHaveElementWithValue(string unexpectedElement,
        string unexpectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        return NotHaveElementWithValue(XNamespace.None + unexpectedElement, unexpectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="XDocument.Root"/> of the current <see cref="XDocument"/> either doesn't have the
    /// specified child element or doesn't have the specified <paramref name="unexpectedValue"/>.
    /// </summary>
    /// <param name="unexpectedElement">
    /// he full name <see cref="XName"/> of the unexpected child element of the current element's <see cref="XDocument"/>.
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
    public AndConstraint<XDocumentAssertions> NotHaveElementWithValue(XName unexpectedElement,
        string unexpectedValue, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedElement, nameof(unexpectedElement));
        Guard.ThrowIfArgumentIsNull(unexpectedValue, nameof(unexpectedValue));

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect {context:subject} to have an element {0} with value {1}{reason}, ",
                unexpectedElement, unexpectedValue, chain => chain
                    .ForCondition(Subject is not null)
                    .FailWith("but the element itself is <null>.")
                    .Then
                    .ForCondition(!Subject!.Root!.Elements(unexpectedElement)
                        .Any(e => e.Value == unexpectedValue))
                    .FailWith("but the element {0} does have this value.", unexpectedElement));

        return new AndConstraint<XDocumentAssertions>(this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "XML document";
}
