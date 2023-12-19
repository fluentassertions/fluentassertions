using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;
using FluentAssertionsAsync.Xml.Equivalency;

namespace FluentAssertionsAsync.Xml;

/// <summary>
/// Contains a number of methods to assert that an <see cref="XDocument"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class XDocumentAssertions : ReferenceTypeAssertions<XDocument, XDocumentAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XDocumentAssertions" /> class.
    /// </summary>
    public XDocumentAssertions(XDocument document)
        : base(document)
    {
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
    public AndConstraint<XDocumentAssertions> Be(XDocument expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
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
    public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
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
    public AndConstraint<XDocumentAssertions> BeEquivalentTo(XDocument expected, string because = "", params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader otherReader = expected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(subjectReader, otherReader, because, becauseArgs);
            xmlReaderValidator.Validate(shouldBeEquivalent: true);
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
    public AndConstraint<XDocumentAssertions> NotBeEquivalentTo(XDocument unexpected, string because = "",
        params object[] becauseArgs)
    {
        using (XmlReader subjectReader = Subject?.CreateReader())
        using (XmlReader otherReader = unexpected?.CreateReader())
        {
            var xmlReaderValidator = new XmlReaderValidator(subjectReader, otherReader, because, becauseArgs);
            xmlReaderValidator.Validate(shouldBeEquivalent: false);
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveRoot(string expected, string because = "",
        params object[] becauseArgs)
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveRoot(XName expected, string because = "",
        params object[] becauseArgs)
    {
        if (Subject is null)
        {
            throw new InvalidOperationException(
                "Cannot assert the document has a root element if the document itself is <null>.");
        }

        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has a root element if the expected name is <null>.");

        XElement root = Subject.Root;

        Execute.Assertion
            .ForCondition(root is not null && root.Name == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have root element {0}{reason}, but found {1}.",
                expected.ToString(), Subject);

        return new AndWhichConstraint<XDocumentAssertions, XElement>(this, root);
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(string expected, string because = "",
        params object[] becauseArgs)
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
        OccurrenceConstraint occurrenceConstraint, string because = "", params object[] becauseArgs)
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
    public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(XName expected, string because = "",
        params object[] becauseArgs)
    {
        if (Subject is null)
        {
            throw new InvalidOperationException("Cannot assert the document has an element if the document itself is <null>.");
        }

        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element if the expected name is <null>.");

        bool success = Execute.Assertion
            .ForCondition(Subject.Root is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:subject} to have root element with child {0}{reason}, but it has no root element.",
                expected.ToString());

        XElement xElement = null;

        if (success)
        {
            xElement = Subject.Root!.Element(expected);

            Execute.Assertion
                .ForCondition(xElement is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have root element with child {0}{reason}, but no such child element was found.",
                    expected.ToString());
        }

        return new AndWhichConstraint<XDocumentAssertions, XElement>(this, xElement);
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
        OccurrenceConstraint occurrenceConstraint, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot assert the document has an element count if the element name is <null>.");

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Cannot assert the count if the document itself is <null>.");

        IEnumerable<XElement> xElements = Enumerable.Empty<XElement>();

        if (success)
        {
            var root = Subject!.Root;

            success = Execute.Assertion
                .ForCondition(root is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:subject} to have root element containing a child {0}{reason}, but it has no root element.",
                    expected.ToString());

            if (success)
            {
                xElements = root!.Elements(expected);
                int actual = xElements.Count();

                Execute.Assertion
                    .ForConstraint(occurrenceConstraint, actual)
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:subject} to have a root element containing a child {0} " +
                        $"{{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                        expected.ToString());
            }
        }

        return new AndWhichConstraint<XDocumentAssertions, IEnumerable<XElement>>(this, xElements);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "XML document";
}
