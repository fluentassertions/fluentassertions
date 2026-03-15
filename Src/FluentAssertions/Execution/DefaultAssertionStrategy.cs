using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FluentAssertions.Execution;

[ExcludeFromCodeCoverage]
[System.Diagnostics.StackTraceHidden]
internal class DefaultAssertionStrategy : IAssertionStrategy2
{
    /// <summary>
    /// Returns the assertion failures that happened until now.
    /// </summary>
    public IEnumerable<AssertionFailure> Failures => [];

    /// <summary>
    /// Gets the number of assertion failures that have been collected.
    /// </summary>
    public int FailureCount => 0;

    /// <summary>
    /// Instructs the strategy to handle a deferred assertion failure.
    /// </summary>
    public void HandleFailure(AssertionFailure failure)
    {
        AssertionEngine.TestFramework.Throw(failure.ToString());
    }

    /// <summary>
    /// Discards and returns the failures that happened up to now.
    /// </summary>
    public IEnumerable<AssertionFailure> DiscardFailures() => [];

    /// <summary>
    /// Will throw a combined exception for any failures have been collected.
    /// </summary>
    public void ThrowIfAny(IDictionary<string, object> context)
    {
    }
}
