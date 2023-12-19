using System.Collections.Generic;
using System.Linq;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency.Selection;

/// <summary>
/// Selection rule that adds all public properties of the expectation.
/// </summary>
internal class AllPropertiesSelectionRule : IMemberSelectionRule
{
    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        IEnumerable<IMember> selectedProperties = context.Type
            .GetProperties(context.IncludedProperties)
            .Select(info => new Property(context.Type, info, currentNode));

        return selectedMembers.Union(selectedProperties).ToList();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Include all non-private properties";
    }
}
