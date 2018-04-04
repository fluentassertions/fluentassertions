using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that includes a particular member in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPredicateSelectionRule : IMemberSelectionRule
    {
        private readonly Func<IMemberInfo, bool> predicate;
        private readonly string description;

        public IncludeMemberByPredicateSelectionRule(Expression<Func<IMemberInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        public bool IncludesMembers => true;

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context, IEquivalencyAssertionOptions config)
        {
            var members = new List<SelectedMemberInfo>(selectedMembers);

            foreach (SelectedMemberInfo selectedMemberInfo in context.RuntimeType.GetNonPrivateMembers())
            {
                if (predicate(new NestedSelectionContext(context, selectedMemberInfo)))
                {
                    if (!members.Any(p => p.IsEquivalentTo(selectedMemberInfo)))
                    {
                        members.Add(selectedMemberInfo);
                    }
                }
            }

            return members;
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Include member when " + description;
        }
    }
}
