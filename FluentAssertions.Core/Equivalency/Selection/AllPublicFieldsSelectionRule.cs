using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that adds all public fields of the subject. 
    /// </summary>
    internal class AllPublicFieldsSelectionRule : IMemberSelectionRule
    {
        public bool IncludesMembers
        {
            get { return false; }
        }

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            return
                selectedMembers.Union(
                    config.GetSubjectType(context).GetNonPrivateFields().Select(SelectedMemberInfo.Create));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Include all non-private fields";
        }
    }
}