#if NET6_0_OR_GREATER
using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// Defines a rule to match a member from an expected object with a member from a subject object
/// based on the existence of a JSON property with a matching name.
/// </summary>
internal class MustMatchJsonPropertyByNameRule : IMemberMatchingRule
{
    /// <inheritdoc />
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options,
        AssertionChain assertionChain)
    {
        StringComparison comparison =
            options.IgnoreJsonPropertyCasing ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        JsonProperty property = JsonProperty.Find(expectedMember, parent, subject, comparison);

        if (property is null)
        {
            assertionChain.FailWith("Expectation has {0} that the other object does not have.",
                    expectedMember.Expectation.AsNonFormattable());
        }

        return property;
    }
}

#endif
