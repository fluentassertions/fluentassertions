using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Selection rule that removes members of a particular type from the structural comparison.
/// </summary>
internal class ExcludeMemberByTypeSelectionRule : IMemberSelectionRule
{
    private readonly Type targetType;

    public ExcludeMemberByTypeSelectionRule(Type targetType)
    {
        Guard.ThrowIfArgumentIsNull(targetType);
        this.targetType = targetType;
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
            // Check if memberType derives from the target open generic type
            // e.g., class Derived : OpenGeneric<int> or class Derived<T> : OpenGeneric<T>
            Type baseType = memberType;
            do
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }
            while (baseType is not null);

            return false;
        }

        return targetType.IsAssignableFrom(memberType);
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        if (targetType.IsGenericTypeDefinition)
        {
            return $"Exclude members whose type derives from/is a closed generic type of {targetType}";
        }

        if (targetType.IsSealed)
        {
            return $"Exclude members of type {targetType}";
        }

        return $"Exclude members whose type is/derives from {targetType}";
    }
}
