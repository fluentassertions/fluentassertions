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

        public ConversionSelector ConversionSelector => inner.ConversionSelector;

        public IEnumerable<IEquivalencyStep> GetUserEquivalencySteps(ConversionSelector conversionSelector)
        {
            return inner.GetUserEquivalencySteps(conversionSelector).Select(step => new CollectionMemberAssertionRuleDecorator(step)).ToArray();
        }

        public bool IsRecursive => inner.IsRecursive;

        public bool AllowInfiniteRecursion => inner.AllowInfiniteRecursion;

        public CyclicReferenceHandling CyclicReferenceHandling => inner.CyclicReferenceHandling;

        public EnumEquivalencyHandling EnumEquivalencyHandling => inner.EnumEquivalencyHandling;

        public bool UseRuntimeTyping => inner.UseRuntimeTyping;

        public bool IncludeProperties => inner.IncludeProperties;

        public bool IncludeFields => inner.IncludeFields;

        public EqualityStrategy GetEqualityStrategy(Type type)
        {
            return inner.GetEqualityStrategy(type);
        }

        public ITraceWriter TraceWriter => inner.TraceWriter;
    }
}
