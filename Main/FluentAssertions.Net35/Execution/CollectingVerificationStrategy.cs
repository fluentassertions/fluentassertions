using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Execution
{
    internal class CollectingVerificationStrategy : IVerificationStrategy
    {
        private readonly List<string> failureMessages = new List<string>();

        public CollectingVerificationStrategy(IVerificationStrategy parent)
        {
            if (parent != null)
            {
                failureMessages.AddRange(parent.FailureMessages);
            }
        }

        /// <summary>
        /// Returns the messages for the verification failures that happened until now.
        /// </summary>
        public IEnumerable<string> FailureMessages
        {
            get { return failureMessages; }
        }

        /// <summary>
        /// Discards and returns the failure messages that happened up to now.
        /// </summary>
        public IEnumerable<string> DiscardFailures()
        {
            var discardedFailures = failureMessages.ToArray();
            failureMessages.Clear();
            return discardedFailures;
        }

        /// <summary>
        /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
        /// </summary>
        public void ThrowIfAny(IDictionary<string, string> context)
        {
            if (failureMessages.Any())
            {
                string message = string.Join(Environment.NewLine, failureMessages.ToArray()) +
                    Environment.NewLine +
                    context;

                AssertionHelper.Throw(message);
            }
        }

        /// <summary>
        /// Instructs the strategy to handle a verification failure.
        /// </summary>
        public void HandleFailure(string message)
        {
            failureMessages.Add(message);
        }
    }
}