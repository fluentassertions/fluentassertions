using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency.Ordering;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Collection of <see cref="PathBasedOrderingRule"/>s.
    /// </summary>
    public class OrderingRuleCollection : IEnumerable<IOrderingRule>
    {
        private readonly List<IOrderingRule> rules = new List<IOrderingRule>();

        /// <summary>
        /// Initializes a new collection of ordering rules.
        /// </summary>
        public OrderingRuleCollection()
        {
        }

        /// <summary>
        /// Initializes a new collection of ordering rules based on an existing collection of ordering rules.
        /// </summary>
        public OrderingRuleCollection(IEnumerable<IOrderingRule> orderingRules)
        {
            rules.AddRange(orderingRules);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IOrderingRule> GetEnumerator()
        {
            return rules.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IOrderingRule rule)
        {
            rules.Add(rule);
        }

        /// <summary>
        /// Determines whether the rules in this collection dictate strict ordering during the equivalency assertion on
        /// the collection pointed to by <paramref name="subjectInfo"/>.
        /// </summary>
        public bool IsOrderingStrictFor(ISubjectInfo subjectInfo)
        {
            List<OrderStrictness> results = rules.Select(r => r.Evaluate(subjectInfo)).ToList();
            return results.Contains(OrderStrictness.Strict) && !results.Contains(OrderStrictness.NotStrict);
        }
    }
}