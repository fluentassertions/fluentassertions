using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;
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

    private Dictionary<(object Subject, object Expectation, int ExpectationIndex), string[]> failuresCache = new();

    public void FindAndRemoveMatches(List<object> subjects, List<TExpectation> expectations)
    {
        failuresCache = new(new ReferentialComparer());

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

            string[] failures = TryToMatch(expectation, subject, expectationIndex);
            if (failures.Length == 0)
            {
                tracer.WriteLine(_ => "It's a match");
                remainingSubjects.RemoveAt(index);
                return true;
            }

            tracer.WriteLine(_ => $"Contained {failures.Length} failures");
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
                    MinDistance = remainingSubjects.Min(a => TryToMatch(e.Item, a, e.Index).Length)
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
                FindClosestMismatches(remainingSubjects, expectationsWithIndexes, TryToMatch);

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

    private static IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatches(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes,
        Func<TExpectation, object, int, string[]> getFailures)
    {
        // For small collections, use exact permutation search to find the globally optimal assignment.
        // factorial(8) = 40,320 which is well within reason.
        const int maxSizeForExactSearch = 8;

        return remainingSubjects.Count <= maxSizeForExactSearch ?
            FindClosestMismatchesByPermutation(remainingSubjects, expectationsWithIndexes, getFailures) :
            FindClosestMismatchesByGreedyAssignment(remainingSubjects, expectationsWithIndexes, getFailures);
    }

    /// <summary>
    /// Finds the best assignment by exhaustively trying all permutations. Only suitable for small collections.
    /// </summary>
    private static IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatchesByPermutation(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes,
        Func<TExpectation, object, int, string[]> getFailures)
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
                score += getFailures(expectationWithIndex.Item, assignment[index], expectationWithIndex.Index).Length;

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

        int pairCount = Math.Min(expectationsWithIndexes.Count, bestAssignment.Count);
        var result = new List<(IndexedItem<TExpectation>, object, string[])>(pairCount);

        for (int index = 0; index < pairCount; index++)
        {
            IndexedItem<TExpectation> expectationWithIndex = expectationsWithIndexes[index];
            string[] failures = getFailures(expectationWithIndex.Item, bestAssignment[index], expectationWithIndex.Index);
            result.Add((expectationWithIndex, bestAssignment[index], failures));
        }

        return result;
    }

    /// <summary>
    /// Finds a near-optimal assignment using a greedy strategy. Suitable for large collections where the exact
    /// permutation search would be prohibitively expensive. All distances are already cached from Phase 1, so
    /// this is O(n² log n) rather than O(n! × n).
    /// </summary>
    private static IReadOnlyList<(IndexedItem<TExpectation> Expectation, object Actual, string[] Failures)> FindClosestMismatchesByGreedyAssignment(
        List<object> remainingSubjects, IndexedItemCollection<TExpectation> expectationsWithIndexes,
        Func<TExpectation, object, int, string[]> getFailures)
    {
        int subjectCount = remainingSubjects.Count;
        int expectationCount = expectationsWithIndexes.Count;
        int pairCount = expectationCount * subjectCount;

        var allPairs = new List<(int ExpectationIndex, int SubjectIndex, int Count)>(pairCount);

        for (int expectationIndex = 0; expectationIndex < expectationCount; expectationIndex++)
        {
            IndexedItem<TExpectation> exp = expectationsWithIndexes[expectationIndex];

            for (int subjectIndex = 0; subjectIndex < subjectCount; subjectIndex++)
            {
                int count = getFailures(exp.Item, remainingSubjects[subjectIndex], exp.Index).Length;
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

        foreach (var (expectationIndex, subjectIndex, _) in allPairs)
        {
            if (!assignedExpectationIndexes[expectationIndex] && !assignedSubjectIndexes[subjectIndex])
            {
                string[] failures = getFailures(expectationsWithIndexes[expectationIndex].Item, remainingSubjects[subjectIndex], expectationsWithIndexes[expectationIndex].Index);
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

    private string[] TryToMatch(TExpectation expectation, object subject, int expectationIndex)
    {
        // Create a cache key based on the subject and expectation instances
        var cacheKey = (subject, (object)expectation, expectationIndex);

        if (failuresCache.TryGetValue(cacheKey, out string[] cachedResult))
        {
            return cachedResult;
        }

        using var scope = new AssertionScope();

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(TExpectation)),
            context.AsCollectionItem<TExpectation>(expectationIndex));

        string[] failures = scope.Discard();
        failuresCache[cacheKey] = failures;

        return failures;
    }
}
