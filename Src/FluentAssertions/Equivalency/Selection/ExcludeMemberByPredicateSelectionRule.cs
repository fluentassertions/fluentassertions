using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that removes a particular member from the structural comparison based on a predicate.
    /// </summary>
    internal class ExcludeMemberByPredicateSelectionRule : IMemberSelectionRule
    {
        private readonly Func<IMemberInfo, bool> predicate;
        private readonly string description;

        public ExcludeMemberByPredicateSelectionRule(Expression<Func<IMemberInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        public bool IncludesMembers => false;

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context, IEquivalencyAssertionOptions config)
        {
            return selectedMembers.Where(p => !predicate(new NestedSelectionContext(context, p))).ToArray();
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Exclude member when " + description;
        }
    }
}
