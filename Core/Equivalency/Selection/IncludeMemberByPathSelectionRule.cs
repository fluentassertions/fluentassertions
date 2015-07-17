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
        private readonly string pathToInclude;

        public IncludeMemberByPathSelectionRule(string pathToInclude) : base(pathToInclude)
        {
            this.pathToInclude = pathToInclude;
        }

        public override bool IncludesMembers
        {
            get { return true; }
        }

        protected override IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            string currentPath, ISubjectInfo context)
        {
            List<SelectedMemberInfo> members = selectedMembers.ToList();

            foreach (SelectedMemberInfo member in context.RuntimeType.GetNonPrivateMembers())
            {
                if (pathToInclude.Contains(currentPath.Combine(member.Name)))
                {
                    members.Add(member);
                }
            }

            return members;
        }

        public override string ToString()
        {
            return "Include member root." + pathToInclude;
        }
    }
}