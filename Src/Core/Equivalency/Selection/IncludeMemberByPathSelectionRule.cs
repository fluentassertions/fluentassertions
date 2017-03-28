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
        private readonly MemberPath pathToInclude;

        public IncludeMemberByPathSelectionRule(string pathToInclude) : base(pathToInclude)
        {
            this.pathToInclude = new MemberPath(pathToInclude);
        }

        public override bool IncludesMembers => true;

        protected override IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            string currentPath, ISubjectInfo context)
        {
            var matchingMembers =
                from member in context.RuntimeType.GetNonPrivateMembers()
                where pathToInclude.IsParentOrChildOf(currentPath.Combine(member.Name))
                select member;

            return selectedMembers.Concat(matchingMembers).ToArray();
        }

        public override string ToString()
        {
            return "Include member root." + pathToInclude;
        }
    }
}