using System;
using System.Linq.Expressions;

namespace FluentAssertions.Equivalency
{
    public class PredicateBasedOrderingRule : IOrderingRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;

        public PredicateBasedOrderingRule(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            this.predicate = predicate.Compile();
        }

        /// <summary>
        /// Determines if ordering of the property refered to by the current <paramref name="subjectInfo"/> is relevant.
        /// </summary>
        public bool AppliesTo(ISubjectInfo subjectInfo)
        {
            return predicate(subjectInfo);
        }
    }
}