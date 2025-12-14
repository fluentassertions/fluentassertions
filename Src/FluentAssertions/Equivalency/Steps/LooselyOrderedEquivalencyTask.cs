using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

#pragma warning disable CA1305

namespace FluentAssertions.Equivalency.Steps;

internal class LooselyOrderedEquivalencyTask<TExpectation>(
    AssertionChain assertionChain,
    IValidateChildNodeEquivalency parent,
    IEquivalencyValidationContext context)
{
    private readonly Tracer tracer = context.Tracer;
    private readonly Dictionary<(object Subject, object Expectation, int ExpectationIndex), string[]> matchCache = new();
    private List<object> subjects;
    private List<TExpectation> expectations;

    public void Execute(IEnumerable<object> subjects, IEnumerable<TExpectation> expectations)
    {
        this.subjects = new List<object>(subjects);
        this.expectations = new List<TExpectation>(expectations);
        int expectedCount = this.expectations.Count;

        // First find the exact matches
        FindAndRemoveExactMatches();

        this.expectations = this.expectations
            .Select(e => new
            {
                Expectation = e,
                MinDistance = this.subjects.Min(a => TryToMatch(e, a, 0).Length)
            })
            .OrderBy(x => x.MinDistance)
            .Select(x => x.Expectation)
            .ToList();

        // If there are still unmatched expectations, find the closest matches
        if (this.expectations.Count > 0 && this.subjects.Count > 0)
        {
            IReadOnlyList<(TExpectation, object, string[])> bestMatches = FindClosestMismatches(TryToMatch);
            foreach (var (expectation, subject, failures) in bestMatches)
            {
                foreach (string failure in failures)
                {
                    assertionChain.AddPreFormattedFailure(failure);
                }

                this.subjects.Remove(subject);
                this.expectations.Remove(expectation);
            }
        }

        // Now report what's missing or extraneous
        if (this.expectations.Count > 0 || this.subjects.Count > 0)
        {
            StringBuilder message = new();
            message.Append($"Expected {expectedCount} item(s), but it ");

            if (this.expectations.Count > 0)
            {
                message.Append($"misses {Formatter.ToString(this.expectations)}");
                if (this.subjects.Count > 0)
                {
                    message.AppendLine("and it ");
                }
            }

            if (this.subjects.Count > 0)
            {
                message.Append($"has extraneous item(s) {Formatter.ToString(this.subjects)}");
            }

            assertionChain.AddPreFormattedFailure(message.ToString());
        }
    }

    private void FindAndRemoveExactMatches()
    {
        int expectationIndex = 0;
        while (expectations.Count > expectationIndex)
        {
            TExpectation expectation = expectations[expectationIndex];
            if (StrictlyMatchAgainst(expectation, expectationIndex))
            {
                // It's a full match, so we don't have to look at it again
                expectations.RemoveAt(expectationIndex);
            }
            else
            {
                // Continue with the next item, but we leave the unmatched items.
                expectationIndex++;
            }
        }
    }

    private IReadOnlyList<(TExpectation Expectation, object Actual, string[] Failures)> FindClosestMismatches(
        Func<TExpectation, object, int, string[]> getFailures)
    {
        var bestScore = int.MaxValue;
        List<(TExpectation, object, string[])> best = null;

        const int maxPermutations = 200_000;
        int seen = 0;

        foreach (IReadOnlyList<object> assignment in Permute(subjects))
        {
            if (++seen > maxPermutations)
            {
                break;
            }

            int score = 0;
            var current = new List<(TExpectation, object, string[])>();

            for (int expectationIndex = 0; expectationIndex < expectations.Count; expectationIndex++)
            {
                string[] failures = getFailures(expectations[expectationIndex], assignment[expectationIndex], expectationIndex);
                int distance = failures.Length;
                score += distance;

                if (score >= bestScore)
                {
                    break; // prune
                }

                current.Add((expectations[expectationIndex], assignment[expectationIndex], failures));
            }

            if (score < bestScore)
            {
                bestScore = score;
                best = current;
            }
        }

        return (best is not null) ? best : Array.Empty<(TExpectation, object, string[])>();
    }

    /// <summary>
    /// Generates all possible permutations of the given collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="items">The collection of items for which permutations are to be generated.</param>
    /// <returns>A sequence of all possible permutations, where each permutation is represented as a read-only list of elements.</returns>
    private static IEnumerable<IReadOnlyList<T>> Permute<T>(IReadOnlyList<T> items)
    {
        int[] indices = Enumerable.Range(0, items.Count).ToArray();

        do
        {
            var result = new T[indices.Length];
            for (int index = 0; index < indices.Length; index++)
            {
                result[index] = items[indices[index]];
            }

            yield return result;
        }
        while (NextPermutation(indices));
    }

    private static bool NextPermutation(int[] indices)
    {
        int index = indices.Length - 2;
        while (index >= 0 && indices[index] >= indices[index + 1])
        {
            index--;
        }

        if (index < 0)
        {
            return false;
        }

        int j = indices.Length - 1;
        while (indices[j] <= indices[index])
        {
            j--;
        }

        (indices[index], indices[j]) = (indices[j], indices[index]);
        Array.Reverse(indices, index + 1, indices.Length - index - 1);
        return true;
    }

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
    private bool StrictlyMatchAgainst<T>(T expectation, int expectationIndex)
    {
        foreach ((int index, object subject) in subjects.Index())
        {
            using var _ = tracer.WriteBlock(member =>
                       $"Comparing subject at {member.Subject}[{index}] with the expectation at {member.Expectation}[{expectationIndex}]");

            string[] failures = TryToMatch(subject, expectation, expectationIndex);

            if (failures.Length == 0)
            {
                tracer.WriteLine(_ => "It's a match");
                subjects.RemoveAt(index);
                return true;
            }

            tracer.WriteLine(_ => $"Contained {failures.Length} failures");
        }

        return false;
    }

    private string[] TryToMatch<T>(T expectation, object subject, int expectationIndex)
    {
        // Create a cache key based on the subject and expectation instances
        var cacheKey = (subject, (object)expectation, expectationIndex);

        if (matchCache.TryGetValue(cacheKey, out string[] cachedResult))
        {
            return cachedResult;
        }

        using var scope = new AssertionScope();

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(T)),
            context.AsCollectionItem<T>(expectationIndex));

        string[] failures = scope.Discard();
        matchCache[cacheKey] = failures;

        return failures;
    }
}
