using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections
{
    internal class OnlyContainAssertionHelper
    {
        public static Dictionary<int, int> FindBestMatching(PredicateMatchesMatrix matches)
        {
            var assignments = new AssignmentCollection();

            foreach (var predicate in matches.AllPredicates)
            {
                var newAssignments = FindAssignmentForPredicate(predicate, matches, assignments);
                assignments.UpdateFrom(newAssignments);
            }

            return assignments.GetAssignedElementsByPredicate();
        }

        private static IEnumerable<Assignment> FindAssignmentForPredicate(
            int predicate,
            PredicateMatchesMatrix matches,
            AssignmentCollection currentAssignments)
        {
            var bfsDecisionTree = new BfsTracker(predicate, currentAssignments);
            var visitedElements = new HashSet<int>();

            while (bfsDecisionTree.TryDequeueNotAssignedPredicate(out var unassignedPredicate))
            {
                var notVisitedMatchingElements = matches.GetMatchingElements(unassignedPredicate).Where(_ => !visitedElements.Contains(_));

                foreach (var element in notVisitedMatchingElements)
                {
                    visitedElements.Add(element);

                    if (currentAssignments.Exists(element))
                    {
                        bfsDecisionTree.ReassignElement(element, unassignedPredicate);
                    }
                    else
                    {
                        var finalAssignment = new Assignment { Predicate = unassignedPredicate, Element = element };
                        return bfsDecisionTree.GetAssignmentChain(finalAssignment);
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

        private class BfsTracker
        {
            private readonly Queue<int> notAssignedPredicatesQueue = new Queue<int>();
            private readonly Dictionary<int, Assignment> previousAssignmentByPredicate = new Dictionary<int, Assignment>();

            private readonly AssignmentCollection originalAssignments;

            public BfsTracker(int notAssignedPredicate, AssignmentCollection originalAssignments)
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
