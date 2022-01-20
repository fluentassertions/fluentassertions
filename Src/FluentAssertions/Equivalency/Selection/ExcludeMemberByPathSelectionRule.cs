using System.Collections.Generic;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that removes a particular property from the structural comparison.
    /// </summary>
    internal class ExcludeMemberByPathSelectionRule : SelectMemberByPathSelectionRule
    {
        private MemberPath memberToExclude;

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

        public void ExtendPath(MemberPath nextPath)
        {
            memberToExclude = memberToExclude.Extend(nextPath, "[*]");
            SelectedPath = memberToExclude.ToString();
        }

        public override string ToString()
        {
            return "Exclude member " + memberToExclude;
        }
    }
}
