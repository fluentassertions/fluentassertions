using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;

namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberAssertionOptionsDecorator : IEquivalencyAssertionOptions
    {
        private readonly IEquivalencyAssertionOptions inner;

        public CollectionMemberAssertionOptionsDecorator(IEquivalencyAssertionOptions inner)
        {
            this.inner = inner;
        }

        public IEnumerable<IMemberSelectionRule> SelectionRules
        {
            get
            {
                return inner.SelectionRules.Select(rule => new CollectionMemberSelectionRuleDecorator(rule)).ToArray();
            }
        }

        public IEnumerable<IMemberMatchingRule> MatchingRules
        {
            get { return inner.MatchingRules.Select(rule => new CollectionMemberMatchingRuleDecorator(rule)).ToArray(); }
        }

        public OrderingRuleCollection OrderingRules
        {
            get
            {
                return new OrderingRuleCollection(inner.OrderingRules.Select(rule => new CollectionMemberOrderingRuleDecorator(rule)));
            }
        }

        public IEnumerable<IEquivalencyStep> UserEquivalencySteps
        {
            get { return inner.UserEquivalencySteps.Select(step => new CollectionMemberAssertionRuleDecorator(step)).ToArray(); }
        }

        public bool IsRecursive
        {
            get { return inner.IsRecursive; }
        }

        public bool AllowInfiniteRecursion
        {
            get { return inner.AllowInfiniteRecursion; }
        }

        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return inner.CyclicReferenceHandling; }
        }

        public EnumEquivalencyHandling EnumEquivalencyHandling
        {
            get { return inner.EnumEquivalencyHandling; }
        }

        public bool UseRuntimeTyping
        {
            get { return inner.UseRuntimeTyping; }
        }

        public bool IncludeProperties
        {
            get { return inner.IncludeProperties; }
        }

        public bool IncludeFields
        {
            get { return inner.IncludeFields; }
        }

        public bool IsValueType(Type type)
        {
            return inner.IsValueType(type);
        }

        public ITraceWriter TraceWriter => inner.TraceWriter;
    }
}