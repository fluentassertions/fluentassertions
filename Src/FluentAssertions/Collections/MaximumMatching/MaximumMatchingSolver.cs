using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// The <see cref="MaximumMatchingSolver"/> class encapsulates the algorithm
    /// for solving the maximum matching problem (see <see cref="MaximumMatchingProblem{TElement}"/>).<br />
    /// A simplified variation of the Ford-Fulkerson algorithm is used for solving the problem. <br />
    /// </summary>
    internal class MaximumMatchingSolver
    {
        public static Dictionary<int, int> FindMaximumMatching<T>(MaximumMatchingProblem<T> maximumMatchingProblem)
        {
            var assignments = new AssignmentCollection();

            foreach (var predicate in maximumMatchingProblem.AllPredicateIndices)
            {
                // At each step of the algorithm we search for a solution which contains the current predicate
                // and increases the total number of matches (i.e. Augmenting Flow through the current predicate in the Ford-Fulkerson terminology).
                var newAssignments = FindAssignmentForPredicate(predicate, maximumMatchingProblem, assignments);
                assignments.UpdateFrom(newAssignments);
            }

            return assignments.GetAssignedElementsByPredicate();
        }

        /// <summary>
        /// To find a solution which contains the specified predicate and increases the total number of matches
        /// we: <br />
        /// - Search for a free element which matches the specified predicate.<br />
        /// - Or take over an element which was previously assigned to another predicate and repeat the procedure for the previously assigned predicate.<br />
        /// - We are basically searching for a path in the graph of matches between predicates and elements which would start at the specified predicate
        /// and end at an unassigned element.<br />
        /// - Breadth first search used to traverse the graph.<br />
        /// </summary>
        private static IEnumerable<Assignment> FindAssignmentForPredicate<T>(
            int predicate,
            MaximumMatchingProblem<T> maximumMatchingProblem,
            AssignmentCollection currentAssignments)
        {
            var visitedElements = new HashSet<int>();
            var breadthFirstSearchTracker = new BreadthFirstSearchTracker(predicate, currentAssignments);

            while (breadthFirstSearchTracker.TryDequeueNotAssignedPredicate(out var unassignedPredicate))
            {
                var notVisitedMatchingElements = maximumMatchingProblem.GetMatchingElementIndices(unassignedPredicate).Where(_ => !visitedElements.Contains(_));

                foreach (var element in notVisitedMatchingElements)
                {
                    visitedElements.Add(element);

                    if (currentAssignments.Exists(element))
                    {
                        breadthFirstSearchTracker.ReassignElement(element, unassignedPredicate);
                    }
                    else
                    {
                        var finalAssignment = new Assignment { Predicate = unassignedPredicate, Element = element };
                        return breadthFirstSearchTracker.GetAssignmentChain(finalAssignment);
                    }
                }
            }

            return Enumerable.Empty<Assignment>();
        }

        private struct Assignment
        {
            public int Predicate;
            public int Element;
        }

        private class AssignmentCollection
        {
            private readonly Dictionary<int, int> predicateByAssignedElement = new Dictionary<int, int>();

            public void UpdateFrom(IEnumerable<Assignment> assignments)
            {
                foreach (var assignment in assignments)
                {
                    predicateByAssignedElement[assignment.Element] = assignment.Predicate;
                }
            }

            public int GetAssignedPredicate(int element) => predicateByAssignedElement[element];

            public bool Exists(int element) => predicateByAssignedElement.ContainsKey(element);

            public Dictionary<int, int> GetAssignedElementsByPredicate()
            {
                var result = new Dictionary<int, int>();

                foreach (var pair in predicateByAssignedElement)
                {
                    result.Add(pair.Value, pair.Key);
                }

                return result;
            }
        }

        private class BreadthFirstSearchTracker
        {
            private readonly Queue<int> notAssignedPredicatesQueue = new Queue<int>();
            private readonly Dictionary<int, Assignment> previousAssignmentByPredicate = new Dictionary<int, Assignment>();

            private readonly AssignmentCollection originalAssignments;

            public BreadthFirstSearchTracker(int notAssignedPredicate, AssignmentCollection originalAssignments)
            {
                notAssignedPredicatesQueue.Enqueue(notAssignedPredicate);

                this.originalAssignments = originalAssignments;
            }

            public bool TryDequeueNotAssignedPredicate(out int notAssignedPredicate)
            {
                if (notAssignedPredicatesQueue.Count == 0)
                {
                    notAssignedPredicate = -1;
                    return false;
                }

                notAssignedPredicate = notAssignedPredicatesQueue.Dequeue();
                return true;
            }

            public void ReassignElement(int element, int newAssignedPredicate)
            {
                var previouslyAssignedPredicate = originalAssignments.GetAssignedPredicate(element);
                previousAssignmentByPredicate.Add(previouslyAssignedPredicate, new Assignment { Predicate = newAssignedPredicate, Element = element });
                notAssignedPredicatesQueue.Enqueue(previouslyAssignedPredicate);
            }

            public IEnumerable<Assignment> GetAssignmentChain(Assignment lastAssignment)
            {
                var assignment = lastAssignment;

                do
                {
                    yield return assignment;
                }
                while (previousAssignmentByPredicate.TryGetValue(assignment.Predicate, out assignment));
            }
        }
    }
}
