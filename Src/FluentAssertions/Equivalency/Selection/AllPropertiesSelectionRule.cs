using System.Collections.Generic;
using System.Linq;
using Reflectify;

namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Selection rule that adds all public properties of the expectation.
/// </summary>
internal class AllPropertiesSelectionRule : IMemberSelectionRule
{
    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        MemberVisibility visibility = context.IncludedProperties | MemberVisibility.ExplicitlyImplemented |
            MemberVisibility.DefaultInterfaceProperties;

        IEnumerable<IMember> selectedProperties = context.Type
            .GetProperties(visibility.ToMemberKind())
            .Where(property => property.GetMethod?.IsPrivate == false)
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
