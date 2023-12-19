using System.Collections.Generic;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Represents a rule that defines which members of the expectation to include while comparing
/// two objects for structural equality.
/// </summary>
public interface IMemberSelectionRule
{
    /// <summary>
    /// Gets a value indicating whether this rule should override the default selection rules that include all members.
    /// </summary>
    bool IncludesMembers { get; }

    /// <summary>
    /// Adds or removes properties or fields to/from the collection of members that must be included while
    /// comparing two objects for structural equality.
    /// </summary>
    /// <param name="currentNode">
    ///     The node within the graph from which to select members.
    /// </param>
    /// <param name="selectedMembers">
    ///     A collection of members that was pre-populated by other selection rules. Can be empty.</param>
    /// <param name="context">Provides auxiliary information such as the configuration and such.</param>
    /// <returns>
    /// The collection of members after applying this rule. Can contain less or more than was passed in.
    /// </returns>
    IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context);
}
