using System.Collections.Generic;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Selection rule that removes a particular property from the structural comparison.
/// </summary>
internal class ExcludeMemberByPathSelectionRule(MemberPath pathToExclude) : SelectMemberByPathSelectionRule
{
    private MemberPath memberToExclude = pathToExclude;

    protected override void AddOrRemoveMembersFrom(List<IMember> selectedMembers, INode parent, string parentPath,
        MemberSelectionContext context)
    {
        selectedMembers.RemoveAll(member =>
            memberToExclude.IsSameAs(new MemberPath(member, parentPath)));
    }

    public void AppendPath(MemberPath nextPath)
    {
        memberToExclude = memberToExclude.AsParentCollectionOf(nextPath);
    }

    protected override MemberPath MemberPath => memberToExclude;

    public MemberPath CurrentPath => memberToExclude;

    public override string ToString()
    {
        return "Exclude member " + memberToExclude;
    }
}
