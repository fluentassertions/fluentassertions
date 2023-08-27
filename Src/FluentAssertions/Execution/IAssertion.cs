using System;

namespace FluentAssertions.Execution;

public interface IAssertion
{
    Func<IAssertionScope> GetCurrentScope { get; }

    bool Succeeded { get; }

    Func<string> GetCallerIdentifier { get; }

    void ForCondition(bool predicate);

    void FailWith(string message, object[] args);

    Func<string> Reason { get; }
}
