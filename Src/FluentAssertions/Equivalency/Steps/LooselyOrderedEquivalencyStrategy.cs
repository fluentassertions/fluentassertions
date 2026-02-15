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
    private readonly Dictionary<(object Subject, object Expectation, int ExpectationIndex), string[]> failuresCache = new();

    public void FindAndRemoveMatches(List<object> subjects, List<TExpectation> expectations)
    {
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
        var bestScore = int.MaxValue;
        List<(IndexedItem<TExpectation> ExpectationWithIndex, object, string[] Failures)> bestSet = null;

        const int maxPermutations = 200_000;
        int seen = 0;

        foreach (IReadOnlyList<object> assignment in remainingSubjects.Permute())
        {
            if (++seen > maxPermutations)
            {
                break;
            }

            int score = 0;
            var currentSet = new List<(IndexedItem<TExpectation> ExpectationWithIndex, object, string[] Failures)>();

            for (int index = 0; index < expectationsWithIndexes.Count && index < assignment.Count; index++)
            {
                IndexedItem<TExpectation> expectationWithIndex = expectationsWithIndexes[index];

                string[] failures = getFailures(expectationWithIndex.Item, assignment[index], expectationWithIndex.Index);

                int distance = failures.Length;
                score += distance;

                if (score >= bestScore)
                {
                    // No need to continue as we already have a better matching set
                    break;
                }

                currentSet.Add((expectationWithIndex, assignment[index], failures));
            }

            if (score < bestScore)
            {
                bestScore = score;
                bestSet = currentSet;
            }
        }

        return bestSet is not null ? bestSet : Array.Empty<(IndexedItem<TExpectation>, object, string[])>();
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
