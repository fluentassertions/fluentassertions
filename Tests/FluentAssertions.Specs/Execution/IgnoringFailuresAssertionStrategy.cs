namespace FluentAssertions.Specs.Execution
{
    using System.Collections.Generic;
    using FluentAssertions.Execution;

    internal class IgnoringFailuresAssertionStrategy : IAssertionStrategy
    {
        public IEnumerable<string> FailureMessages => new string[0];

        public void HandleFailure(string message)
        {
        }

        public IEnumerable<string> DiscardFailures() => new string[0];

        public void ThrowIfAny(IDictionary<string, object> context)
        {
        }
    }
}
