using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Collections.MaximumMatching;

namespace FluentAssertions.Collections
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
        private readonly Dictionary<int, List<int>> matchingElementsByPredicate = new Dictionary<int, List<int>>();

        public MaximumMatchingProblem(
            IList<Expression<Func<TElement, bool>>> predicates,
            IList<TElement> elements)
        {
            Predicates = predicates;
            Elements = elements;
            AllPredicateIndices = Enumerable.Range(0, predicates.Count);
            AllElementIndices = Enumerable.Range(0, elements.Count);
        }

        public IList<Expression<Func<TElement, bool>>> Predicates { get; }

        public IList<TElement> Elements { get; }

        public IEnumerable<int> AllPredicateIndices { get; }

        public IEnumerable<int> AllElementIndices { get; }

        public IEnumerable<int> GetMatchingElementIndices(int predicateIndex)
        {
            if (!matchingElementsByPredicate.TryGetValue(predicateIndex, out var matchingElements))
            {
                matchingElements = new List<int>();

                var compiledPredicate = Predicates[predicateIndex].Compile();

                for (int elementIndex = 0; elementIndex < Elements.Count; elementIndex++)
                {
                    var element = Elements[elementIndex];

                    if (compiledPredicate(element))
                    {
                        matchingElements.Add(elementIndex);
                    }
                }

                matchingElementsByPredicate.Add(predicateIndex, matchingElements);
            }

            return matchingElements;
        }

        public MaximumMatchingSolution<TElement> Solve()
        {
            var matches = MaximumMatchingSolver.FindMaximumMatching<TElement>(this);
            return new MaximumMatchingSolution<TElement>(this, matches);
        }
    }
}
