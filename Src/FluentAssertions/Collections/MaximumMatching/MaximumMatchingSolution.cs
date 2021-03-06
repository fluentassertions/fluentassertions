using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentAssertions.Collections.MaximumMatching
{
    /// <summary>
    /// The <see cref="MaximumMatchingSolution{TElement}"/> class defines the solution (output) for the maximum matching problem.
    /// See documentation of <see cref="MaximumMatchingProblem{TElement}"/> for more details.
    /// </summary>
    /// <typeparam name="TElement">The type of elements which must be matched with predicates.</typeparam>
    internal class MaximumMatchingSolution<TElement>
    {
        private readonly Dictionary<IndexedPredicate<TElement>, IndexedElement<TElement>> matchedElementsByPredicates;
        private readonly MaximumMatchingProblem<TElement> problem;

        public MaximumMatchingSolution(
            MaximumMatchingProblem<TElement> problem,
            Dictionary<IndexedPredicate<TElement>, IndexedElement<TElement>> matchedElementsByPredicates)
        {
            this.problem = problem;
            this.matchedElementsByPredicates = matchedElementsByPredicates;
        }

        public bool UnmatchedPredicatesExist => problem.Predicates.Count != matchedElementsByPredicates.Count;

        public bool UnmatchedElementsExist => problem.Elements.Count != matchedElementsByPredicates.Count;

        public List<IndexedPredicate<TElement>> GetUnmatchedPredicates()
        {
            return problem.Predicates.Except(matchedElementsByPredicates.Keys).ToList();
        }

        public List<IndexedElement<TElement>> GetUnmatchedElements()
        {
            return problem.Elements.Except(matchedElementsByPredicates.Values).ToList();
        }
    }
}
