using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FluentAssertions.Collections.MaximumMatching
{
    internal class IndexedPredicate<TElement>
    {
        private readonly Func<TElement, bool> compiledExpression;

        public IndexedPredicate(int index, Expression<Func<TElement, bool>> predicateFunction)
        {
            Index = index;
            PredicateFunction = predicateFunction;
            compiledExpression = predicateFunction.Compile();
        }

        public int Index { get; }

        public Expression<Func<TElement, bool>> PredicateFunction { get; }

        public bool Matches(TElement element) => compiledExpression(element);

        public override int GetHashCode() => Index.GetHashCode();

        public override bool Equals(object other) => other is IndexedPredicate<TElement> otherPredicate && Index == otherPredicate.Index;
    }
}
