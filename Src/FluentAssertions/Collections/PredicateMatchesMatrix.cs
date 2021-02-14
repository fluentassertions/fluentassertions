using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections
{
    internal class PredicateMatchesMatrix
    {
        private readonly Dictionary<int, List<int>> matchingElementsByPredicate;

        public PredicateMatchesMatrix(int predicatesCount, int elementsCount)
        {
            matchingElementsByPredicate = new Dictionary<int, List<int>>();
            AllPredicates = Enumerable.Range(0, predicatesCount);
            AllElements = Enumerable.Range(0, elementsCount);
        }

        public IEnumerable<int> AllPredicates { get; }

        public IEnumerable<int> AllElements { get; }

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
