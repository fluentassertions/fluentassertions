using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Execution
{
    internal class CollectingVerificationStrategy : IVerificationStrategy
    {
        private readonly List<string> failureMessages = new List<string>();
        private readonly Verifier verification = new Verifier();

        /// <summary>
        /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
        /// </summary>
        public void ThrowIfAny(string context)
        {
            if (failureMessages.Any())
            {
                string message = string.Join(Environment.NewLine, failureMessages.ToArray()) +
                    Environment.NewLine +
                    context;

                AssertionHelper.Throw(message);
            }
        }

        public bool HasFailures
        {
            get { return failureMessages.Any(); }
        }

        public int FailureCount
        {
            get { return failureMessages.Count; }
        }

        public IEnumerable<string> Failures
        {
            get { return failureMessages; }
        }

        public void HandleFailure(string message)
        {
            failureMessages.Add(message);
        }

        public Verifier GetCurrentVerifier()
        {
            return verification;
        }
    }
}