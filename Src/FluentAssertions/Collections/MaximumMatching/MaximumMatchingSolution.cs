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
        private readonly Dictionary<int, int> matchedElementIndexesByPredicateIndex;
        private readonly MaximumMatchingProblem<TElement> problem;

        public MaximumMatchingSolution(
            MaximumMatchingProblem<TElement> problem,
            Dictionary<int, int> matchedElementIndexesByPredicateIndex)
        {
            this.problem = problem;
            this.matchedElementIndexesByPredicateIndex = matchedElementIndexesByPredicateIndex;
        }

        public bool NotMatchedPredicatesExist => problem.Predicates.Count != matchedElementIndexesByPredicateIndex.Count;

        public bool NotMatchedElementsExist => problem.Elements.Count != matchedElementIndexesByPredicateIndex.Count;

        public List<Expression<Func<TElement, bool>>> GetNotMatchedPredicates()
        {
            return problem.AllPredicateIndices
                .Except(matchedElementIndexesByPredicateIndex.Keys)
                .Select(predicateIndex => problem.Predicates[predicateIndex])
                .ToList();
        }

        public List<int> GetNotMatchedElementIndices()
        {
            return problem.AllElementIndices.Except(matchedElementIndexesByPredicateIndex.Values).ToList();
        }
    }
}
