using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                var builder = new StringBuilder();
                builder.AppendLine(string.Join(Environment.NewLine, failureMessages.ToArray()));
                
                if (context.Any())
                {
                    foreach (KeyValuePair<string, string> pair in context)
                    {
                        builder.AppendFormat("\nWith {0}:\n\"{1}\"", pair.Key, pair.Value);
                    }
                }

                AssertionHelper.Throw(builder.ToString());
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