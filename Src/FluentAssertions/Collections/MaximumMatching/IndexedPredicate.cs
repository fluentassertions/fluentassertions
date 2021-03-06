using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Formatting;

namespace FluentAssertions.Collections.MaximumMatching
{
    internal class IndexedPredicate<TElement>
    {
        private readonly Func<TElement, bool> compiledExpression;

        public IndexedPredicate(Expression<Func<TElement, bool>> expression, int index)
        {
            Index = index;
            Expression = expression;
            compiledExpression = expression.Compile();
        }

        public int Index { get; }

        public Expression<Func<TElement, bool>> Expression { get; }

        public bool Matches(TElement element) => compiledExpression(element);

        public override string ToString() => $"Index: {Index}, Expression: {Formatter.ToString(Expression)}";
    }
}
