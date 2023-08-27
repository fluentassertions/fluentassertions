using System;

namespace FluentAssertions.Execution;

public interface IAssertion
{
    IAssertionScope CurrentScope { get; }

    bool Succeeded { get; }

    Func<string> GetCallerIdentifier { get; }

    void ForCondition(bool predicate);

    void FailWith(string message, object[] args);
}
