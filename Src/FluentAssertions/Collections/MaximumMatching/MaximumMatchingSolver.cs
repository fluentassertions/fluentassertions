using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections.MaximumMatching
{
    /// <summary>
    /// The <see cref="MaximumMatchingSolver{TElement}"/> class encapsulates the algorithm
    /// for solving the maximum matching problem (see <see cref="MaximumMatchingProblem{TElement}"/>).<br />
    /// A simplified variation of the Ford-Fulkerson algorithm is used for solving the problem. <br />
    /// </summary>
    internal class MaximumMatchingSolver<TElement>
    {
        private readonly MaximumMatchingProblem<TElement> problem;
        private readonly Dictionary<int, List<int>> matchingElementsByPredicate = new();

        public MaximumMatchingSolver(MaximumMatchingProblem<TElement> problem)
        {
            this.problem = problem;
        }

        /// <summary>
        /// Solves the maximum matching problem;
        /// </summary>
        public MaximumMatchingSolution<TElement> Solve()
        {
            var assignments = new AssignmentCollection();

            foreach (var predicate in problem.Predicates)
            {
                // At each step of the algorithm we search for a solution which contains the current predicate
                // and increases the total number of matches (i.e. Augmenting Flow through the current predicate in the Ford-Fulkerson terminology).
                var newAssignments = FindAssignmentForPredicate(predicate.Index, assignments);
                assignments.UpdateFrom(newAssignments);
            }

            var elementsByMatchedPredicate = assignments.ToDictionary(
                assignment => problem.Predicates[assignment.Predicate],
                assignment => problem.Elements[assignment.Element]);

            return new MaximumMatchingSolution<TElement>(problem, elementsByMatchedPredicate);
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
        private IEnumerable<Assignment> FindAssignmentForPredicate(int predicate, AssignmentCollection currentAssignments)
        {
            var visitedElements = new HashSet<int>();
            var breadthFirstSearchTracker = new BreadthFirstSearchTracker(predicate, currentAssignments);

            while (breadthFirstSearchTracker.TryDequeueNotAssignedPredicate(out var unassignedPredicate))
            {
                var notVisitedMatchingElements = GetMatchingElements(unassignedPredicate).Where(element => !visitedElements.Contains(element));

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

        private IEnumerable<int> GetMatchingElements(int predicateIndex)
        {
            var predicate = problem.Predicates[predicateIndex];

            if (!matchingElementsByPredicate.TryGetValue(predicateIndex, out var matchingElements))
            {
                matchingElements = problem.Elements.Where(element => predicate.Matches(element.Value)).Select(element => element.Index).ToList();
                matchingElementsByPredicate.Add(predicateIndex, matchingElements);
            }

            return matchingElements;
        }

        private struct Assignment
        {
            public int Predicate;
            public int Element;
        }

        private class AssignmentCollection : IEnumerable<Assignment>
        {
            private readonly Dictionary<int, Assignment> assignmentsByElement = new Dictionary<int, Assignment>();

            public void UpdateFrom(IEnumerable<Assignment> assignments)
            {
                foreach (var assignment in assignments)
                {
                    assignmentsByElement[assignment.Element] = assignment;
                }
            }

            public int GetAssignedPredicate(int element) => assignmentsByElement[element].Predicate;

            public bool Exists(int element) => assignmentsByElement.ContainsKey(element);

            public IEnumerator<Assignment> GetEnumerator() => assignmentsByElement.Values.GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => assignmentsByElement.Values.GetEnumerator();
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
