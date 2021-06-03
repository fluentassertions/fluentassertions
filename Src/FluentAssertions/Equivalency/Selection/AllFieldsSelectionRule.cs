using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that adds all public fields of the expectation
    /// </summary>
    internal class AllFieldsSelectionRule : IMemberSelectionRule
    {
        public bool IncludesMembers => false;

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context)
        {
            IEnumerable<IMember> selectedNonPrivateFields = context.Type
                .GetNonPrivateFields(context.IncludedFields)
                .Select(info => new Field(info, currentNode));

            return selectedMembers.Union(selectedNonPrivateFields).ToList();
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
