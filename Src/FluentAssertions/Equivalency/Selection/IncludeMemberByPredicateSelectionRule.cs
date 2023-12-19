using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency.Selection;

/// <summary>
/// Selection rule that includes a particular member in the structural comparison.
/// </summary>
internal class IncludeMemberByPredicateSelectionRule : IMemberSelectionRule
{
    private readonly Func<IMemberInfo, bool> predicate;
    private readonly string description;

    public IncludeMemberByPredicateSelectionRule(Expression<Func<IMemberInfo, bool>> predicate)
    {
        description = predicate.Body.ToString();
        this.predicate = predicate.Compile();
    }

    public bool IncludesMembers => true;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        var members = new List<IMember>(selectedMembers);

        foreach (MemberInfo memberInfo in currentNode.Type.GetMembers(MemberVisibility.Public |
                     MemberVisibility.Internal))
        {
            IMember member = MemberFactory.Create(memberInfo, currentNode);

            if (predicate(new MemberToMemberInfoAdapter(member)) && !members.Exists(p => p.IsEquivalentTo(member)))
            {
                members.Add(member);
            }
        }

        return members;
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Include member when " + description;
    }
}
