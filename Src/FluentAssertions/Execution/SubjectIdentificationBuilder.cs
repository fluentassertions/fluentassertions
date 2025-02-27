using System;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Responsible for constructing the code phrase that identifies the subject-under-test and which is used
/// in assertion failures managed by a single <see cref="AssertionChain"/>.
/// </summary>
/// <remarks>
/// Takes into account the caller identifiers extracted from the invoking code, any scopes created through
/// (nested) instances of <see cref="AssertionScope"/> and modifications made by the
/// <see cref="AndWhichConstraint{TParent,TSubject}"/>.
/// </remarks>
internal class SubjectIdentificationBuilder
{
    private readonly Func<string> getScopeName;
    private readonly Lazy<string[]> identifiersExtractedFromTheCode;
    private int identifierIndex;
    private Func<string> getSubject;

    public SubjectIdentificationBuilder(Func<string[]> getCallerIdentifiers, Func<string> getScopeName)
    {
        this.getScopeName = getScopeName;

        identifiersExtractedFromTheCode = new(() => getCallerIdentifiers());

        getSubject = () => GetIdentifier(0);
    }

    /// <summary>
    /// Tells the builder to assume the assertion has moved to the next identifier in a chained assertion.
    /// </summary>
    public void AdvanceToNextSubject()
    {
        identifierIndex++;
        var localIndex = identifierIndex;

        getSubject = () => GetIdentifier(localIndex);
    }

    /// <summary>
    /// Indicates whether the subject identifier has been overridden with a custom value during the assertion process.
    /// </summary>
    /// <remarks>
    /// When <see cref="OverrideSubjectIdentifier"/> or <see cref="UsePostfix(string)"/> is called,
    /// this property is set to true, signaling that a manually defined identifier is now used instead of
    /// automatically constructed identifiers.
    /// </remarks>
    public bool HasOverriddenIdentifier { get; private set; }

    /// <summary>
    /// Completely overrides the construction process with the specified lazy-evaluated identifier.
    /// </summary>
    public void OverrideSubjectIdentifier(Func<string> getSubject)
    {
        HasOverriddenIdentifier = true;
        this.getSubject = getSubject;
    }

    /// <summary>
    /// Appends the given postfix to the current subject and combines that with next subject.
    /// </summary>
    /// <remarks>
    /// If the postfix is <c>[0]</c>, the current identifier is <c>collection</c>, and the next one
    /// is <c>Parameters</c>, this method will assume the subject is <c>collection[0].Parameters</c>.
    /// </remarks>
    public void UsePostfix(string postfix)
    {
        var localIndex = identifierIndex;
        getSubject = () => (GetIdentifier(localIndex) + postfix).Combine(GetIdentifier(localIndex + 1));

        HasOverriddenIdentifier = true;
    }

    /// <summary>
    /// Returns the constructed subject identifier.
    /// </summary>
    public string Build()
    {
        var scopeName = getScopeName();
        var callerIdentifier = getSubject();

        if (scopeName is null)
        {
            return callerIdentifier ?? "";
        }
        else if (callerIdentifier is null)
        {
            return scopeName;
        }
        else
        {
            return $"{scopeName}/{callerIdentifier}";
        }
    }

    private string GetIdentifier(int index)
    {
        return identifiersExtractedFromTheCode.Value.Length > index ? identifiersExtractedFromTheCode.Value[index] : null;
    }
}
