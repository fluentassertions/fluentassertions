using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections
{
    internal class PredicateMatchesMatrix
    {
        private readonly Dictionary<int, List<int>> matchingElementsByPredicate;
        private readonly IEnumerable<int> allPredicates;

        public PredicateMatchesMatrix(int predicatesCount)
        {
            matchingElementsByPredicate = new Dictionary<int, List<int>>();
            allPredicates = Enumerable.Range(0, predicatesCount);
        }

        public IEnumerable<int> AllPredicates => allPredicates;

        public IEnumerable<int> GetMatchingElements(int predicate)
        {
            matchingElementsByPredicate.TryGetValue(predicate, out var matchingElements);
            return matchingElements ?? Enumerable.Empty<int>();
        }

        public void AddMatch(int predicateIndex, int elementIndex)
        {
            if (!matchingElementsByPredicate.TryGetValue(predicateIndex, out var matchingElements))
            {
                matchingElements = new List<int>();
                matchingElementsByPredicate.Add(predicateIndex, matchingElements);
            }

            matchingElements.Add(elementIndex);
        }
    }
}
