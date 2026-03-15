using System.Collections.Generic;

namespace FluentAssertions.Execution;

/// <summary>
/// Defines a strategy for handling failures in an <see cref="AssertionScope"/> using deferred <see cref="AssertionFailure"/> objects
/// instead of pre-rendered strings.
/// </summary>
public interface IAssertionStrategy2
{
    /// <summary>
    /// Returns the assertion failures that happened until now.
    /// </summary>
    IEnumerable<AssertionFailure> Failures { get; }

    /// <summary>
    /// Gets the number of assertion failures that have been collected.
    /// </summary>
    int FailureCount { get; }

    /// <summary>
    /// Instructs the strategy to handle an assertion failure.
    /// </summary>
    void HandleFailure(AssertionFailure failure);

    /// <summary>
    /// Discards and returns the failures that happened up to now.
    /// </summary>
    IEnumerable<AssertionFailure> DiscardFailures();

    /// <summary>
    /// Will throw a combined exception for any failures have been collected.
    /// </summary>
    void ThrowIfAny(IDictionary<string, object> context);
}
