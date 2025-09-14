using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// First tries to find a JSON property with the same name, and if that fails, falls back to a field or property with the same name.
/// </summary>
internal class MustMatchByNameRule : IMemberMatchingRule
{
    /// <inheritdoc />
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options,
        AssertionChain assertionChain)
    {
#if NET6_0_OR_GREATER
        if (subject is System.Text.Json.Nodes.JsonNode)
        {
            return new MustMatchJsonPropertyByNameRule().Match(expectedMember, subject, parent, options, assertionChain);
        }
#endif

        return new MustMatchMemberByNameRule().Match(expectedMember, subject, parent, options, assertionChain);
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Match (JSON) member by name (or throw)";
    }
}
