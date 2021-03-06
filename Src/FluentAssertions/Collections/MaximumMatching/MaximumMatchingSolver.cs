using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections.MaximumMatching
{
    /// <summary>
    /// The <see cref="MaximumMatchingSolver{TElement}"/> class encapsulates the algorithm
    /// for solving the maximum matching problem (see <see cref="MaximumMatchingProblem{TElement}"/>).<br />
    /// A simplified variation of the Ford-Fulkerson algorithm is used for solving the problem. <br />
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
            var assignments = new AssignmentCollection();

            foreach (var predicate in problem.Predicates)
            {
                // At each step of the algorithm we search for a solution which contains the current predicate
                // and increases the total number of matches (i.e. Augmenting Flow through the current predicate in the Ford-Fulkerson terminology).
                var newAssignments = FindAssignmentForPredicate(predicate, assignments);
                assignments.UpdateFrom(newAssignments);
            }

            var elementsByMatchedPredicate = assignments.ToDictionary(assignment => assignment.Predicate, assignment => assignment.Element);

            return new MaximumMatchingSolution<TValue>(problem, elementsByMatchedPredicate);
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
        private IEnumerable<Assignment> FindAssignmentForPredicate(Predicate<TValue> predicate, AssignmentCollection currentAssignments)
        {
            var visitedElements = new HashSet<Element<TValue>>();
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

        private IEnumerable<Element<TValue>> GetMatchingElements(Predicate<TValue> predicate)
        {
            if (!matchingElementsByPredicate.TryGetValue(predicate, out var matchingElements))
            {
                matchingElements = problem.Elements.Where(element => predicate.Matches(element.Value)).ToList();
                matchingElementsByPredicate.Add(predicate, matchingElements);
            }

            return matchingElements;
        }

        private struct Assignment
        {
            public Predicate<TValue> Predicate;
            public Element<TValue> Element;
        }

        private class AssignmentCollection : IEnumerable<Assignment>
        {
            private readonly Dictionary<Element<TValue>, Assignment> assignmentsByElement = new();

            public void UpdateFrom(IEnumerable<Assignment> assignments)
            {
                foreach (var assignment in assignments)
                {
                    assignmentsByElement[assignment.Element] = assignment;
                }
            }

            public Predicate<TValue> GetAssignedPredicate(Element<TValue> element)
            {
                return assignmentsByElement[element].Predicate;
            }                

            public bool Exists(Element<TValue> element) => assignmentsByElement.ContainsKey(element);

            public IEnumerator<Assignment> GetEnumerator() => assignmentsByElement.Values.GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => assignmentsByElement.Values.GetEnumerator();
        }

        private class BreadthFirstSearchTracker
        {
            private readonly Queue<Predicate<TValue>> notAssignedPredicatesQueue = new();
            private readonly Dictionary<Predicate<TValue>, Assignment> previousAssignmentByPredicate = new();

            private readonly AssignmentCollection originalAssignments;

            public BreadthFirstSearchTracker(Predicate<TValue> notAssignedPredicate, AssignmentCollection originalAssignments)
            {
                notAssignedPredicatesQueue.Enqueue(notAssignedPredicate);

                this.originalAssignments = originalAssignments;
            }

            public bool TryDequeueNotAssignedPredicate(out Predicate<TValue> notAssignedPredicate)
            {
                if (notAssignedPredicatesQueue.Count == 0)
                {
                    notAssignedPredicate = null;
                    return false;
                }

                notAssignedPredicate = notAssignedPredicatesQueue.Dequeue();
                return true;
            }

            public void ReassignElement(Element<TValue> element, Predicate<TValue> newAssignedPredicate)
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
