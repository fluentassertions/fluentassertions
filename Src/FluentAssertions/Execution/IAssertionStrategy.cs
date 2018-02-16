using System.Collections.Generic;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Defines a strategy for handling failures in a <see cref="AssertionScope"/>.
    /// </summary>
    public interface IAssertionStrategy
    {
        /// <summary>
        /// Returns the messages for the assertion failures that happened until now.
        /// </summary>
        IEnumerable<string> FailureMessages { get; }

        /// <summary>
        /// Instructs the strategy to handle a assertion failure.
        /// </summary>
        void HandleFailure(string message);

        /// <summary>
        /// Discards and returns the failure messages that happened up to now.
        /// </summary>
        IEnumerable<string> DiscardFailures();

        /// <summary>
        /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
        /// </summary>
        void ThrowIfAny(IDictionary<string, object> context);
    }
}
