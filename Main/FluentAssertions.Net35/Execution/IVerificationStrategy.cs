using System.Collections.Generic;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Defines a strategy for handling failures in a <see cref="VerificationScope"/>.
    /// </summary>
    internal interface IVerificationStrategy
    {
        /// <summary>
        /// Returns the messages for the verification failures that happened until now.
        /// </summary>
        IEnumerable<string> FailureMessages { get; }

        /// <summary>
        /// Instructs the strategy to handle a verification failure.
        /// </summary>
        void HandleFailure(string message);

        /// <summary>
        /// Discards and returns the failure messages that happened up to now.
        /// </summary>
        IEnumerable<string> DiscardFailures();

        /// <summary>
        /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
        /// </summary>
        void ThrowIfAny(IDictionary<string, string> context);
    }
}