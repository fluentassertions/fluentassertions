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
            IList<Expression<Func<TElement, bool>>> predicates,
            IList<TElement> elements)
        {
            Predicates = new();

            for (int i = 0; i < predicates.Count; i++)
            {
                Predicates.Add(new IndexedPredicate<TElement>(i, predicates[i]));
            }

            Elements = new();

            for (int i = 0; i < elements.Count; i++)
            {
                Elements.Add(new IndexedElement<TElement>(i, elements[i]));
            }
        }

        public List<IndexedPredicate<TElement>> Predicates { get; }

        public List<IndexedElement<TElement>> Elements { get; }

        public MaximumMatchingSolution<TElement> Solve()
        {

        }
    }
}
