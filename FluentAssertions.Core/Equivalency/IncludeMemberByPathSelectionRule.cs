using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPathSelectionRule : IMemberSelectionRule
    {
        private readonly ISelectedMemberInfo selectedMemberInfo;

        public IncludeMemberByPathSelectionRule(ISelectedMemberInfo selectedMemberInfo)
        {
            this.selectedMemberInfo = selectedMemberInfo;
        }

        public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            List<ISelectedMemberInfo> members = selectedMembers.ToList();

            if (!members.Any(member => member.IsEquivalentTo(selectedMemberInfo)))
            {
                members.Add(selectedMemberInfo);
            }

            return members;
        }

        public override string ToString()
        {
            return "Select member " + selectedMemberInfo.DeclaringType + "." + selectedMemberInfo.Name;
        }
    }
}