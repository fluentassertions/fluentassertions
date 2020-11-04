using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that adds all public properties of the expectation.
    /// </summary>
    internal class AllPublicPropertiesSelectionRule : IMemberSelectionRule
    {
        public bool IncludesMembers => false;

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context)
        {
            IEnumerable<IMember> selectedNonPrivateProperties = context.Options.GetExpectationType(context.RuntimeType, context.CompileTimeType)
                .GetNonPrivateProperties()
                .Select(info => new Property(info, currentNode));

            return selectedMembers.Union(selectedNonPrivateProperties).ToList();
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
