using System.Collections.Generic;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that removes a particular property from the structural comparison.
    /// </summary>
    internal class ExcludeMemberByPathSelectionRule : SelectMemberByPathSelectionRule
    {
        private readonly MemberPath memberToExclude;

        public ExcludeMemberByPathSelectionRule(MemberPath pathToExclude)
            : base(pathToExclude.ToString())
        {
            memberToExclude = pathToExclude;
        }

        protected override void AddOrRemoveMembersFrom(List<IMember> selectedMembers, INode parent, string parentPath,
            MemberSelectionContext context)
        {
            selectedMembers.RemoveAll(member =>
                memberToExclude.IsSameAs(new MemberPath(member, parentPath)));
        }

        public override string ToString()
        {
            return "Exclude member " + memberToExclude;
        }
    }
}
