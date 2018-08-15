using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that adds all public properties of the subject.
    /// </summary>
    internal class AllPublicPropertiesSelectionRule : IMemberSelectionRule
    {
        public bool IncludesMembers => false;

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<SelectedMemberInfo> selectedNonPrivateProperties = config.GetExpectationType(context)
                .GetNonPrivateProperties()
                .Select(SelectedMemberInfo.Create);

            return selectedMembers.Union(selectedNonPrivateProperties);
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
            return "Include all non-private properties";
        }
    }
}
