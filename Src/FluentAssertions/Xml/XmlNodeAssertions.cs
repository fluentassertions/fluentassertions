﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using FluentAssertions.Xml.Equivalency;

namespace FluentAssertions.Xml;

/// <summary>
/// Contains a number of methods to assert that an <see cref="XmlNode"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class XmlNodeAssertions : XmlNodeAssertions<XmlNode, XmlNodeAssertions>
{
    public XmlNodeAssertions(XmlNode xmlNode, AssertionChain assertionChain)
        : base(xmlNode, assertionChain)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that an <see cref="XmlNode"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class XmlNodeAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
    where TSubject : XmlNode
    where TAssertions : XmlNodeAssertions<TSubject, TAssertions>
{
    private readonly AssertionChain assertionChain;

    public XmlNodeAssertions(TSubject xmlNode, AssertionChain assertionChain)
        : base(xmlNode, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlNode"/> is equivalent to the <paramref name="expected"/> node.
    /// </summary>
    /// <param name="expected">The expected node</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeEquivalentTo(XmlNode expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (var subjectReader = new XmlNodeReader(Subject))
        using (var expectedReader = new XmlNodeReader(expected))
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, expectedReader, because, becauseArgs);
            xmlReaderValidator.AssertThatItIsEquivalent();
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="XmlNode"/> is not equivalent to
    /// the <paramref name="unexpected"/> node.
    /// </summary>
    /// <param name="unexpected">The unexpected node</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEquivalentTo(XmlNode unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        using (var subjectReader = new XmlNodeReader(Subject))
        using (var unexpectedReader = new XmlNodeReader(unexpected))
        {
            var xmlReaderValidator = new XmlReaderValidator(assertionChain, subjectReader, unexpectedReader, because, becauseArgs);
            xmlReaderValidator.AssertThatIsNotEquivalent();
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "XML node";
}
