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

        public bool Invert { get; set; }

        /// <summary>
        /// Determines if ordering of the member referred to by the current <paramref name="subjectInfo"/> is relevant.
        /// </summary>
        public OrderStrictness Evaluate(ISubjectInfo subjectInfo)
        {
            if (predicate(subjectInfo))
            {
                return Invert ? OrderStrictness.NotStrict : OrderStrictness.Strict;
            }
            else
            {
                return OrderStrictness.Irrelevant;
            }
        }

        public override string ToString()
        {
            return $"Be {(Invert ? "not strict" : "strict")} about the order of collections when {description}";
        }
    }
}