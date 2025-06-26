using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Represents a member selection rule that excludes certain fields or properties from being considered during
/// structural equality checks based on their names.
/// </summary>
internal class ExcludeMembersByNameSelectionRule : IMemberSelectionRule
{
    private readonly string[] membersToExclude;
    private readonly string description;

    /// <summary>
    /// Initializes the rule with a list of filed or property names to exclude from the structural equality check.
    /// </summary>
    public ExcludeMembersByNameSelectionRule(string[] membersToExclude)
    {
        this.membersToExclude = membersToExclude;
        description = string.Join(", ", membersToExclude);
    }

    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectedMembers.Where(m => !membersToExclude.Contains(m.Expectation.Name, StringComparer.Ordinal)).ToArray();
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Exclude members named: " + description;
    }
}
