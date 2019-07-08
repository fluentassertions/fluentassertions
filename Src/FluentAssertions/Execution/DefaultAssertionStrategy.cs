using System;
using System.Collections.Generic;

using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
#if NET45
    [Serializable]
#endif
    internal class DefaultAssertionStrategy : IAssertionStrategy
    {
        /// <summary>
        /// Returns the messages for the assertion failures that happened until now.
        /// </summary>
        public IEnumerable<string> FailureMessages
        {
            get
            {
                return new string[0];
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
            return new string[0];
        }

        /// <summary>
        /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
        /// </summary>
        public void ThrowIfAny(IDictionary<string, object> context)
        {
        }
    }
}
