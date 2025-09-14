#if NET6_0_OR_GREATER
using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// A member matching rule that attempts to match a JSON property in a subject object with a corresponding
/// member in the expected object, using the provided equivalency options to determine comparison behavior.
/// </summary>
internal class TryMatchJsonPropertyByNameRule : IMemberMatchingRule
{
    /// <inheritdoc />
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options,
        AssertionChain assertionChain)
    {
        StringComparison comparison =
            options.IgnoreJsonPropertyCasing ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        return JsonProperty.Find(expectedMember, parent, subject, comparison);
    }
}

#endif
