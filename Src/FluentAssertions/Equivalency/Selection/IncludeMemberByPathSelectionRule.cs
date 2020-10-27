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
            var members = context.RuntimeType.GetNonPrivateMembers();
            var preferredMembers = GetPreferredMembers(members, context);

            IEnumerable<SelectedMemberInfo> matchingMembers =
                from member in members
                where ShouldBeIncluded(member, currentPath, preferredMembers)
                select member;

            return selectedMembers.Concat(matchingMembers).ToArray();
        }

        private bool ShouldBeIncluded(SelectedMemberInfo memberInfo, string currentPath,
            IReadOnlyDictionary<string, SelectedMemberInfo> preferredMembers)
        {
            if (preferredMembers.TryGetValue(memberInfo.Name, out var preferredMember) && memberInfo != preferredMember)
            {
                return false;
            }

            var memberPath = new MemberPath(memberInfo.ReflectedType, currentPath.Combine(memberInfo.Name));
            return memberToInclude.IsSameAs(memberPath) || memberToInclude.IsParentOrChildOf(memberPath);
        }

        public override string ToString()
        {
            return "Include member root." + memberToInclude;
        }
    }
}
