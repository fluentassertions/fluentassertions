using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Selection rule that removes members of a particular type from the structural comparison.
/// </summary>
internal class ExcludeMemberByTypeSelectionRule : IMemberSelectionRule
{
    private readonly Type targetType;
    private readonly bool checkAssignability;
    private readonly string description;

    public ExcludeMemberByTypeSelectionRule(Type targetType)
    {
        this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        checkAssignability = targetType.IsInterface || targetType.IsAbstract;

        if (targetType.IsGenericTypeDefinition)
        {
            description = $"Exclude members of closed generic type {targetType}";
        }
        else if (checkAssignability)
        {
            description = $"Exclude members assignable to type {targetType}";
        }
        else
        {
            description = $"Exclude members of type {targetType}";
        }
    }

    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectedMembers.Where(p => !ShouldExclude(p.Type)).ToArray();
    }

    private bool ShouldExclude(Type memberType)
    {
        if (targetType.IsGenericTypeDefinition)
        {
            // Handle open generic types (e.g., Nullable<>)
            return memberType.IsGenericType && memberType.GetGenericTypeDefinition() == targetType;
        }

        if (checkAssignability)
        {
            // For interfaces and abstract types, check if the member type is assignable
            return targetType.IsAssignableFrom(memberType);
        }

        // For concrete types, check exact match
        return memberType == targetType;
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return description;
    }
}
