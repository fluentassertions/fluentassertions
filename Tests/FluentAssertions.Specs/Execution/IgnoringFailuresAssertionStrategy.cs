using System.Collections.Generic;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specs.Execution;

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
