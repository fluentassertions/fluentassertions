using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentAssertions.Execution;

[System.Diagnostics.StackTraceHidden]
internal class CollectingAssertionStrategy : IAssertionStrategy2
{
    private readonly List<AssertionFailure> failures = [];

    /// <summary>
    /// Returns the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<AssertionFailure> Failures => failures;

    /// <summary>
    /// Gets the number of assertion failures that have been collected.
    /// </summary>
    public int FailureCount => failures.Count;

    /// <summary>
    /// Discards and returns the failures that happened up to now.
    /// </summary>
    public IEnumerable<AssertionFailure> DiscardFailures()
    {
        var discardedFailures = failures.ToArray();
        failures.Clear();
        return discardedFailures;
    }

    /// <summary>
    /// Will throw a combined exception for any failures have been collected.
    /// </summary>
    public void ThrowIfAny(IDictionary<string, object> context)
    {
        if (failures.Count > 0)
        {
            var builder = new StringBuilder();
            builder.AppendJoin(Environment.NewLine, failures.Select(f => f.ToString())).AppendLine();

            if (context.Any())
            {
                foreach (KeyValuePair<string, object> pair in context)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, "\nWith {0}:\n{1}", pair.Key, pair.Value);
                }
            }

            AssertionEngine.TestFramework.Throw(builder.ToString());
        }
    }

    /// <summary>
    /// Instructs the strategy to handle a deferred assertion failure.
    /// </summary>
    public void HandleFailure(AssertionFailure failure)
    {
        failures.Add(failure);
    }
}
