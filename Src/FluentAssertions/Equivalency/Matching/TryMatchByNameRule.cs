using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// First tries to find a JSON property with the same name, and if that fails, falls back to a field or property with the same name.
/// </summary>
internal class TryMatchByNameRule : IMemberMatchingRule
{
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options, AssertionChain assertionChain)
    {
#if NET6_0_OR_GREATER
        if (subject is System.Text.Json.Nodes.JsonNode)
        {
            return new TryMatchJsonPropertyByNameRule().Match(expectedMember, subject, parent, options, assertionChain);
        }
#endif

        return new TryMatchMemberByNameRule().Match(expectedMember, subject, parent, options, assertionChain);
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Try to match (JSON) member by name";
    }
}
