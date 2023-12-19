using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Execution;

internal class DefaultAssertionStrategy : IAssertionStrategy
{
    /// <summary>
    /// Returns the messages for the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<string> FailureMessages
    {
        get
        {
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Instructs the strategy to handle a assertion failure.
    /// </summary>
    public void HandleFailure(string message)
    {
        Services.ThrowException(message);
    }

    /// <summary>
    /// Discards and returns the failure messages that happened up to now.
    /// </summary>
    public IEnumerable<string> DiscardFailures()
    {
        return Array.Empty<string>();
    }

    /// <summary>
    /// Will throw a combined exception for any failures have been collected.
    /// </summary>
    public void ThrowIfAny(IDictionary<string, object> context)
    {
    }
}
