using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal class LooselyOrderedEquivalencyStrategy<TExpectation>(
    AssertionChain assertionChain,
    IValidateChildNodeEquivalency parent,
    IEquivalencyValidationContext context)
{
    private const int MaximumFailuresToReport = 10;

    private readonly Tracer tracer = context.Tracer;

    // Populated during Phase 1 + 2 using skip-formatting dry-runs (no string allocation).
    private Dictionary<(object Subject, object Expectation, int ExpectationIndex), int> countCache = new();

    // Populated lazily during Phase 3 for the ~n selected pairs with full formatting.
    private Dictionary<(object Subject, object Expectation, int ExpectationIndex), string[]> fullFailuresCache = new();

    public void FindAndRemoveMatches(List<object> subjects, List<TExpectation> expectations)
    {
        countCache = new(new ReferentialComparer());
        fullFailuresCache = new(new ReferentialComparer());

        var expectationsWithIndexes = new IndexedItemCollection<TExpectation>(expectations);

        // First find the exact matches and remove them from the collection
        FindAndRemoveExactMatches(subjects, expectationsWithIndexes);

        // Reorder the remaining expectations based on their similarity to the remaining unmatched subjects.
        expectationsWithIndexes = SortExpectationsByMinDistance(subjects, expectationsWithIndexes);

        // If there are still unmatched expectations, find the closest matches
        FindAndRemoveClosestMatches(subjects, expectationsWithIndexes);

        // Remove the items that are no longer needed from the original list of expectations
        expectationsWithIndexes.RemoveMatchedItemFrom(expectations);
    }

    private void FindAndRemoveExactMatches(List<object> subjects, IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        int expectationIndex = 0;
        while (expectationsWithIndexes.Count > expectationIndex)
        {
            IndexedItem<TExpectation> expectation = expectationsWithIndexes[expectationIndex];
            if (StrictlyMatchAgainst(subjects, expectation.Item, expectation.Index))
            {
                // It's a full match, so we don't have to look at it again
                expectationsWithIndexes.Remove(expectation);
            }
            else
            {
                // Continue with the next item, but we leave the unmatched items.
                expectationIndex++;
            }
        }
    }

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
    private bool StrictlyMatchAgainst(List<object> remainingSubjects, TExpectation expectation, int expectationIndex)
    {
        foreach ((int index, object subject) in remainingSubjects.Index())
        {
            using var _ = tracer.WriteBlock(member =>
                $"Comparing subject at {member.Subject}[{index}] with the expectation at {member.Expectation}[{expectationIndex}]");

            int failures = TryToMatchCount(expectation, subject, expectationIndex);
            if (failures == 0)
            {
                tracer.WriteLine(_ => "It's a match");
                remainingSubjects.RemoveAt(index);
                return true;
            }

            tracer.WriteLine(_ => $"Contained {failures} failures");
        }

        return false;
    }

    /// <summary>
    /// This is an optimization for loosely-ordered collection comparison. By processing expectations
    /// that are most similar to remaining subjects first, the algorithm is more likely
    /// to find the correct pairings earlier, leading to better error messages when mismatches occur.
    /// </summary>
    private IndexedItemCollection<TExpectation> SortExpectationsByMinDistance(List<object> remainingSubjects,
        IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        if (remainingSubjects.Count > 0)
        {
            return expectationsWithIndexes
                .Select(e => new
                {
                    Expectation = e,
                    MinDistance = remainingSubjects.Min(a => TryToMatchCount(e.Item, a, e.Index))
                })
                .OrderBy(x => x.MinDistance)
                .Select(x => x.Expectation)
                .ToIndexedList();
        }
        else
        {
            return expectationsWithIndexes;
        }
    }

    private void FindAndRemoveClosestMatches(List<object> remainingSubjects,
        IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        int nrFailures = 0;
        if (expectationsWithIndexes.Count > 0 && remainingSubjects.Count > 0)
        {
            IReadOnlyList<(IndexedItem<TExpectation>, object, string[])> bestMatches =
                FindClosestMismatches(remainingSubjects, expectationsWithIndexes);

            foreach (var (expectation, subject, failures) in bestMatches)
            {
                foreach (string failure in failures)
                {
                    if (nrFailures < MaximumFailuresToReport)
                    {
                        assertionChain.AddPreFormattedFailure(failure);
                        nrFailures++;
                    }
                }

                remainingSubjects.Remove(subject);
                expectationsWithIndexes.Remove(expectation);
            }
        }
    }

    private IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatches(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        // For small collections, use exact permutation search to find the globally optimal assignment.
        // factorial(8) = 40,320 which is well within reason.
        const int maxSizeForExactSearch = 8;

        return remainingSubjects.Count <= maxSizeForExactSearch ?
            FindClosestMismatchesByPermutation(remainingSubjects, expectationsWithIndexes) :
            FindClosestMismatchesByGreedyAssignment(remainingSubjects, expectationsWithIndexes);
    }

    /// <summary>
    /// Finds the best assignment by exhaustively trying all permutations. Only suitable for small collections.
    /// Uses failure counts for scoring (no string formatting) and only fetches full failure strings for the
    /// winning assignment.
    /// </summary>
    private IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatchesByPermutation(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        var bestScore = int.MaxValue;
        IReadOnlyList<object> bestAssignment = null;

        foreach (IReadOnlyList<object> assignment in remainingSubjects.Permute())
        {
            int score = 0;
            bool tooHigh = false;

            for (int index = 0; index < expectationsWithIndexes.Count && index < assignment.Count; index++)
            {
                IndexedItem<TExpectation> expectationWithIndex = expectationsWithIndexes[index];
                score += TryToMatchCount(expectationWithIndex.Item, assignment[index], expectationWithIndex.Index);

                if (score >= bestScore)
                {
                    tooHigh = true;
                    break;
                }
            }

            if (!tooHigh && score < bestScore)
            {
                bestScore = score;
                bestAssignment = assignment;
            }
        }

        if (bestAssignment is null)
        {
            return Array.Empty<(IndexedItem<TExpectation>, object, string[])>();
        }

        // Fetch full failure strings only for the winning assignment.
        int pairCount = Math.Min(expectationsWithIndexes.Count, bestAssignment.Count);
        var result = new List<(IndexedItem<TExpectation>, object, string[])>(pairCount);

        for (int index = 0; index < pairCount; index++)
        {
            IndexedItem<TExpectation> expectationWithIndex = expectationsWithIndexes[index];
            string[] failures = TryToMatch(expectationWithIndex.Item, bestAssignment[index], expectationWithIndex.Index);
            result.Add((expectationWithIndex, bestAssignment[index], failures));
        }

        return result;
    }

    /// <summary>
    /// Finds a near-optimal assignment using a greedy strategy. Suitable for large collections where the exact
    /// permutation search would be prohibitively expensive. All distances are already cached from Phase 1, so
    /// this is O(n² log n) rather than O(n! × n).
    /// </summary>
    private IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatchesByGreedyAssignment(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes)
    {
        int subjectCount = remainingSubjects.Count;
        int expectationCount = expectationsWithIndexes.Count;
        int pairCount = expectationCount * subjectCount;

        // Use failure counts (no string allocation) to build the pair list for sorting/assignment.
        var allPairs = new List<(int ExpectationIndex, int SubjectIndex, int Count)>(pairCount);

        for (int expectationIndex = 0; expectationIndex < expectationCount; expectationIndex++)
        {
            IndexedItem<TExpectation> exp = expectationsWithIndexes[expectationIndex];

            for (int subjectIndex = 0; subjectIndex < subjectCount; subjectIndex++)
            {
                int count = TryToMatchCount(exp.Item, remainingSubjects[subjectIndex], exp.Index);
                allPairs.Add((expectationIndex, subjectIndex, count));
            }
        }

        // Sort by distance ascending. When distances are equal, use expectation index then subject index as
        // tiebreakers so that assignments are deterministic and follow natural ordering.
        allPairs.Sort(static (a, b) =>
        {
            int relativeOrder = a.Count.CompareTo(b.Count);
            if (relativeOrder != 0)
            {
                return relativeOrder;
            }

            relativeOrder = a.ExpectationIndex.CompareTo(b.ExpectationIndex);
            return relativeOrder != 0 ? relativeOrder : a.SubjectIndex.CompareTo(b.SubjectIndex);
        });

        var assignedExpectationIndexes = new bool[expectationCount];
        var assignedSubjectIndexes = new bool[subjectCount];
        int totalToAssign = Math.Min(expectationCount, subjectCount);

        var result = new List<(IndexedItem<TExpectation>, object, string[])>(totalToAssign);

        // First checks candidate matches from best to worst, then picks the first unused expectation/subject pair it finds,
        // computes detailed failures only for that chosen pair, and then repeats until all possible matches are assigned.
        foreach (var (expectationIndex, subjectIndex, _) in allPairs)
        {
            if (!assignedExpectationIndexes[expectationIndex] && !assignedSubjectIndexes[subjectIndex])
            {
                // Fetch full failure strings only for the selected pairs (~n total).
                string[] failures = TryToMatch(expectationsWithIndexes[expectationIndex].Item,
                    remainingSubjects[subjectIndex], expectationsWithIndexes[expectationIndex].Index);

                result.Add((expectationsWithIndexes[expectationIndex], remainingSubjects[subjectIndex], failures));
                assignedExpectationIndexes[expectationIndex] = true;
                assignedSubjectIndexes[subjectIndex] = true;

                if (result.Count == totalToAssign)
                {
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Performs a dry-run comparison using a scope that skips formatting. Returns the number of failures
    /// without allocating any string messages. Used in Phase 1 and Phase 2 to avoid eager
    /// FailureMessageFormatter + Regex.Replace costs for pairs that will ultimately be discarded.
    /// </summary>
    private int TryToMatchCount(TExpectation expectation, object subject, int expectationIndex)
    {
        var cacheKey = (subject, (object)expectation, expectationIndex);

        if (countCache.TryGetValue(cacheKey, out int cachedCount))
        {
            return cachedCount;
        }

        using var scope = new AssertionScope { UseDryRun = true };

        var itemContext = (EquivalencyValidationContext)context.AsCollectionItem<TExpectation>(expectationIndex);
        itemContext.CyclicReferenceDetector = (CyclicReferenceDetector)((EquivalencyValidationContext)context).CyclicReferenceDetector.Clone();

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(TExpectation)), itemContext);

        int count = scope.GetFailureCount();
        scope.Discard();
        countCache[cacheKey] = count;

        return count;
    }

    private string[] TryToMatch(TExpectation expectation, object subject, int expectationIndex)
    {
        var cacheKey = (subject, (object)expectation, expectationIndex);

        if (fullFailuresCache.TryGetValue(cacheKey, out string[] cachedResult))
        {
            return cachedResult;
        }

        using var scope = new AssertionScope();

        var itemContext = (EquivalencyValidationContext)context.AsCollectionItem<TExpectation>(expectationIndex);
        itemContext.CyclicReferenceDetector = (CyclicReferenceDetector)((EquivalencyValidationContext)context).CyclicReferenceDetector.Clone();

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(TExpectation)), itemContext);

        string[] failures = scope.Discard();
        fullFailuresCache[cacheKey] = failures;

        return failures;
    }
}
