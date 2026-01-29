using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FluentAssertions.Execution;

[ExcludeFromCodeCoverage]
[System.Diagnostics.StackTraceHidden]
internal class DefaultAssertionStrategy : IAssertionStrategy
{
    /// <summary>
    /// Returns the messages for the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<string> FailureMessages => [];

    /// <summary>
    /// Instructs the strategy to handle a assertion failure.
    /// </summary>
    public void HandleFailure(string message)
    {
        AssertionEngine.TestFramework.Throw(message);
    }

    /// <summary>
    /// Discards and returns the failure messages that happened up to now.
    /// </summary>
    public IEnumerable<string> DiscardFailures() => [];

    /// <summary>
    /// Will throw a combined exception for any failures have been collected.
    /// </summary>
    public void ThrowIfAny(IDictionary<string, object> context)
    {
    }
}
