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
            var preferredMembers = GetPreferredMembers(selectedMembers, context);

            return selectedMembers
                .Where(memberInfo => ShouldBeIncluded(memberInfo, currentPath, preferredMembers))
                .ToArray();
        }

        private bool ShouldBeIncluded(SelectedMemberInfo memberInfo, string currentPath,
            IReadOnlyDictionary<string, SelectedMemberInfo> preferredMembers)
        {
            if (preferredMembers.TryGetValue(memberInfo.Name, out var preferredMember) && memberInfo != preferredMember)
            {
                return true;
            }

            return !memberToExclude.IsSameAs(new MemberPath(memberInfo.ReflectedType, currentPath.Combine(memberInfo.Name)));
        }

        public override string ToString()
        {
            return "Exclude member root." + memberToExclude;
        }
    }
}
