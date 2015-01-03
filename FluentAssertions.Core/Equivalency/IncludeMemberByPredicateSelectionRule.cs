using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that includes a particular member in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPredicateSelectionRule : IMemberSelectionRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly string description;

        public IncludeMemberByPredicateSelectionRule(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            var members = new List<ISelectedMemberInfo>(selectedMembers);

            foreach (ISelectedMemberInfo selectedMemberInfo in context.RuntimeType.GetNonPrivateMembers())
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
            return "Exclude member when " + description;
        }
    }
}