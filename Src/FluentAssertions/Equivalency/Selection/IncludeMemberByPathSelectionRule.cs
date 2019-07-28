using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPathSelectionRule : SelectMemberByPathSelectionRule
    {
        private readonly MemberPath memberToInclude;

        public IncludeMemberByPathSelectionRule(MemberPath pathToInclude)
            : base(pathToInclude.ToString())
        {
            memberToInclude = pathToInclude;
        }

        public override bool IncludesMembers => true;

        protected override IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            string currentPath, IMemberInfo context)
        {
            IEnumerable<SelectedMemberInfo> matchingMembers =
                from member in context.RuntimeType.GetNonPrivateMembers()
                let memberPath = new MemberPath(member.DeclaringType, currentPath.Combine(member.Name))
                where memberToInclude.IsSameAs(memberPath) ||
                    memberToInclude.IsParentOrChildOf(memberPath)
                select member;

            return selectedMembers.Concat(matchingMembers).ToArray();
        }

        public override string ToString()
        {
            return "Include member root." + memberToInclude;
        }
    }
}
