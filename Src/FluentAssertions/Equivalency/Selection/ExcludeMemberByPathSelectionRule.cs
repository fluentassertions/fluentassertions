using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            string currentPath, IMemberInfo context)
        {
            return selectedMembers
                .Where(memberInfo => !memberToExclude.IsSameAs(new MemberPath(memberInfo.DeclaringType, currentPath.Combine(memberInfo.Name))))
                .ToArray();
        }

        public override string ToString()
        {
            return "Exclude member root." + memberToExclude;
        }
    }
}
