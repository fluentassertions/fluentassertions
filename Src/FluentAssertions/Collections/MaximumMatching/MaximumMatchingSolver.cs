using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertionsAsync.Collections.MaximumMatching;

/// <summary>
/// The <see cref="MaximumMatchingSolver{TElement}"/> class encapsulates the algorithm
/// for solving the maximum matching problem (see <see cref="MaximumMatchingProblem{TElement}"/>).
/// See https://en.wikipedia.org/wiki/Maximum_cardinality_matching for more details.<br />
/// A simplified variation of the Ford-Fulkerson algorithm is used for solving the problem.
/// See https://en.wikipedia.org/wiki/Ford%E2%80%93Fulkerson_algorithm for more details.
/// </summary>
internal class MaximumMatchingSolver<TValue>
{
    private readonly MaximumMatchingProblem<TValue> problem;
    private readonly Dictionary<Predicate<TValue>, List<Element<TValue>>> matchingElementsByPredicate = new();

    public MaximumMatchingSolver(MaximumMatchingProblem<TValue> problem)
    {
        this.problem = problem;
    }

    /// <summary>
    /// Solves the maximum matching problem;
    /// </summary>
    public MaximumMatchingSolution<TValue> Solve()
    {
        var matches = new MatchCollection();

        foreach (var predicate in problem.Predicates)
        {
            // At each step of the algorithm we search for a solution which contains the current predicate
            // and increases the total number of matches (i.e. Augmenting Flow through the current predicate in the Ford-Fulkerson terminology).
            var newMatches = FindMatchForPredicate(predicate, matches);
            matches.UpdateFrom(newMatches);
        }

        var elementsByMatchedPredicate = matches.ToDictionary(match => match.Predicate, match => match.Element);

        return new MaximumMatchingSolution<TValue>(problem, elementsByMatchedPredicate);
    }

    /// <summary>
    /// To find a solution which contains the specified predicate and increases the total number of matches
    /// we: <br />
    /// - Search for a free element which matches the specified predicate.<br />
    /// - Or take over an element which was previously matched with another predicate and repeat the procedure for the previously matched predicate.<br />
    /// - We are basically searching for a path in the graph of matches between predicates and elements which would start at the specified predicate
    /// and end at an unmatched element.<br />
    /// - Breadth first search used to traverse the graph.<br />
    /// </summary>
    private IEnumerable<Match> FindMatchForPredicate(Predicate<TValue> predicate, MatchCollection currentMatches)
    {
        var visitedElements = new HashSet<Element<TValue>>();
        var breadthFirstSearchTracker = new BreadthFirstSearchTracker(predicate, currentMatches);

        while (breadthFirstSearchTracker.TryDequeueUnMatchedPredicate(out var unmatchedPredicate))
        {
            var notVisitedMatchingElements =
                GetMatchingElements(unmatchedPredicate).Where(element => !visitedElements.Contains(element));

            foreach (var element in notVisitedMatchingElements)
            {
                visitedElements.Add(element);

                if (currentMatches.Contains(element))
                {
                    breadthFirstSearchTracker.ReassignElement(element, unmatchedPredicate);
                }
                else
                {
                    var finalMatch = new Match { Predicate = unmatchedPredicate, Element = element };
                    return breadthFirstSearchTracker.GetMatchChain(finalMatch);
                }
            }
        }

        return Enumerable.Empty<Match>();
    }

    private List<Element<TValue>> GetMatchingElements(Predicate<TValue> predicate)
    {
        if (!matchingElementsByPredicate.TryGetValue(predicate, out var matchingElements))
        {
            matchingElements = problem.Elements.Where(element => predicate.Matches(element.Value)).ToList();
            matchingElementsByPredicate.Add(predicate, matchingElements);
        }

        return matchingElements;
    }

    private struct Match
    {
        public Predicate<TValue> Predicate;
        public Element<TValue> Element;
    }

    private sealed class MatchCollection : IEnumerable<Match>
    {
        private readonly Dictionary<Element<TValue>, Match> matchesByElement = new();

        public void UpdateFrom(IEnumerable<Match> matches)
        {
            foreach (var match in matches)
            {
                matchesByElement[match.Element] = match;
            }
        }

        public Predicate<TValue> GetMatchedPredicate(Element<TValue> element)
        {
            return matchesByElement[element].Predicate;
        }

        public bool Contains(Element<TValue> element) => matchesByElement.ContainsKey(element);

        public IEnumerator<Match> GetEnumerator() => matchesByElement.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => matchesByElement.Values.GetEnumerator();
    }

    private sealed class BreadthFirstSearchTracker
    {
        private readonly Queue<Predicate<TValue>> unmatchedPredicatesQueue = new();
        private readonly Dictionary<Predicate<TValue>, Match> previousMatchByPredicate = new();

        private readonly MatchCollection originalMatches;

        public BreadthFirstSearchTracker(Predicate<TValue> unmatchedPredicate, MatchCollection originalMatches)
        {
            unmatchedPredicatesQueue.Enqueue(unmatchedPredicate);

            this.originalMatches = originalMatches;
        }

        public bool TryDequeueUnMatchedPredicate(out Predicate<TValue> unmatchedPredicate)
        {
            if (unmatchedPredicatesQueue.Count == 0)
            {
                unmatchedPredicate = null;
                return false;
            }

            unmatchedPredicate = unmatchedPredicatesQueue.Dequeue();
            return true;
        }

        public void ReassignElement(Element<TValue> element, Predicate<TValue> newMatchedPredicate)
        {
            var previouslyMatchedPredicate = originalMatches.GetMatchedPredicate(element);

            previousMatchByPredicate.Add(previouslyMatchedPredicate,
                new Match { Predicate = newMatchedPredicate, Element = element });

            unmatchedPredicatesQueue.Enqueue(previouslyMatchedPredicate);
        }

        public IEnumerable<Match> GetMatchChain(Match lastMatch)
        {
            var match = lastMatch;

            do
            {
                yield return match;
            }
            while (previousMatchByPredicate.TryGetValue(match.Predicate, out match));
        }
    }
}
