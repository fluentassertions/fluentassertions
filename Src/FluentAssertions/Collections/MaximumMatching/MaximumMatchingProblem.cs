using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Collections.MaximumMatching
{
    /// <summary>
    /// The <see cref="MaximumMatchingProblem{TElement}"/> class defines input for the maximum matching problem.
    /// The input is a list of predicates and a list of elements.
    /// The goal of the problem is to find such mapping between predicates and elements that would maximize number of matches.
    /// A predicate can be mapped with only one element.
    /// An element can be mapped with only one predicate.
    /// </summary>
    /// <typeparam name="TElement">The type of elements which must be matched with predicates.</typeparam>
    internal class MaximumMatchingProblem<TElement>
    {
        public MaximumMatchingProblem(
            IEnumerable<Expression<Func<TElement, bool>>> predicates,
            IEnumerable<TElement> elements)
        {
            Predicates.AddRange(predicates.Select((predicate, index) => new IndexedPredicate<TElement>(predicate, index)));
            Elements.AddRange(elements.Select((element, index) => new IndexedElement<TElement>(element, index)));
        }

        public List<IndexedPredicate<TElement>> Predicates { get; } = new();

        public List<IndexedElement<TElement>> Elements { get; } = new();

        public MaximumMatchingSolution<TElement> Solve() => new MaximumMatchingSolver<TElement>(this).Solve();
    }
}
