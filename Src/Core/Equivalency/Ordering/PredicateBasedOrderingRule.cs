using System;
using System.Linq.Expressions;

namespace FluentAssertions.Equivalency.Ordering
{
    internal class PredicateBasedOrderingRule : IOrderingRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly string description;

        public PredicateBasedOrderingRule(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        /// <summary>
        /// Determines if ordering of the member referred to by the current <paramref name="subjectInfo"/> is relevant.
        /// </summary>
        public bool AppliesTo(ISubjectInfo subjectInfo)
        {
            return predicate(subjectInfo);
        }

        public override string ToString()
        {
            return "Be strict about the order of collections when " + description;
        }
    }
}