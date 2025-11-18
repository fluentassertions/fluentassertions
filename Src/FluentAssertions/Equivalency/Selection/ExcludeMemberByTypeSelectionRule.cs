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
        
        // Always check assignability except for sealed types (which can't be derived from)
        checkAssignability = !targetType.IsSealed;

        if (targetType.IsGenericTypeDefinition)
        {
            description = $"Exclude members whose type derives from or is a closed generic type of {targetType}";
        }
        else if (checkAssignability)
        {
            description = $"Exclude members whose type is or derives from {targetType}";
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
            // Check if memberType is a closed generic of the target open generic type
            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == targetType)
            {
                return true;
            }

            // Check if memberType derives from the target open generic type
            // e.g., class Derived : OpenGeneric<int> or class Derived<T> : OpenGeneric<T>
            Type baseType = memberType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        if (checkAssignability)
        {
            // For non-sealed types (including interfaces and abstract types), 
            // check if the member type is assignable (i.e., is the same or derives from the target type)
            return targetType.IsAssignableFrom(memberType);
        }

        // For sealed types, check exact match only
        return memberType == targetType;
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return description;
    }
}
