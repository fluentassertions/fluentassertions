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
        private readonly string pathToExclude;

        public ExcludeMemberByPathSelectionRule(string pathToExclude) : base(pathToExclude)
        {
            this.pathToExclude = pathToExclude;
        }

        protected override IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            string currentPath, ISubjectInfo context)
        {
            return selectedMembers.Where(memberInfo => currentPath.Combine(memberInfo.Name) != pathToExclude).ToArray();
        }

        public override string ToString()
        {
            return "Exclude member root." + pathToExclude;
        }
    }
}