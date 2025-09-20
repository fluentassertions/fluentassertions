using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Represents a collection of assertion results obtained through a <see cref="AssertionScope"/>.
/// </summary>
internal class AssertionResultSet
{
    private readonly Dictionary<object, string[]> set = [];

    /// <summary>
    /// Adds the failures (if any) resulting from executing an assertion within a
    /// <see cref="AssertionScope"/> identified by a key.
    /// </summary>
    public void AddSet(object key, string[] failures)
    {
        set[key] = failures;
    }

    /// <summary>
    /// Returns the closest match compared to the set identified by the provided <paramref name="key"/> or
    /// an empty array if one of the results represents a successful assertion.
    /// </summary>
    /// <remarks>
    /// The closest match is the set that contains the least amount of failures, or no failures at all, and preferably
    /// the set that is identified by the <paramref name="key"/>.
    /// </remarks>
    public string[] GetTheFailuresForTheSetWithTheFewestFailures(object key = null)
    {
        if (ContainsSuccessfulSet())
        {
            return [];
        }

        KeyValuePair<object, string[]>[] bestResultSets = GetBestResultSets();
        if (bestResultSets.Length == 0)
        {
            return [];
        }

        KeyValuePair<object, string[]> bestMatch = Array.Find(bestResultSets, r => r.Key.Equals(key));

        if ((bestMatch.Key, bestMatch.Value) == default)
        {
            return bestResultSets[0].Value;
        }

        return bestMatch.Value;
    }

    private KeyValuePair<object, string[]>[] GetBestResultSets()
    {
        if (set.Values.Count == 0)
        {
            return [];
        }

        int fewestFailures = set.Values.Min(r => r.Length);
        return set.Where(r => r.Value.Length == fewestFailures).ToArray();
    }

    /// <summary>
    /// Gets a value indicating whether this collection contains a set without any failures at all.
    /// </summary>
    public bool ContainsSuccessfulSet() => set.Values.Any(v => v.Length == 0);
}
