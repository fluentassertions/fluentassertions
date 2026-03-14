using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentAssertions.Execution;

[System.Diagnostics.StackTraceHidden]
internal class CollectingAssertionStrategy : IAssertionStrategy, IAssertionStrategy2
{
    private readonly List<AssertionFailure> failures = [];

    /// <summary>
    /// Returns the messages for the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<string> FailureMessages => failures.Select(f => f.ToString());

    /// <summary>
    /// Returns the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<AssertionFailure> Failures => failures;

    /// <summary>
    /// Gets the number of assertion failures that have been collected.
    /// </summary>
    public int FailureCount => failures.Count;

    /// <summary>
    /// Discards and returns the failure messages that happened up to now.
    /// </summary>
    IEnumerable<string> IAssertionStrategy.DiscardFailures()
    {
        var discardedFailures = failures.Select(f => f.ToString()).ToArray();
        failures.Clear();
        return discardedFailures;
    }

    /// <summary>
    /// Discards and returns the failures that happened up to now.
    /// </summary>
    IEnumerable<AssertionFailure> IAssertionStrategy2.DiscardFailures()
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
    /// Instructs the strategy to handle a pre-formatted assertion failure.
    /// </summary>
    public void HandleFailure(string message)
    {
        failures.Add(new AssertionFailure(message));
    }

    /// <summary>
    /// Instructs the strategy to handle a deferred assertion failure.
    /// </summary>
    public void HandleFailure(AssertionFailure failure)
    {
        failures.Add(failure);
    }
}
