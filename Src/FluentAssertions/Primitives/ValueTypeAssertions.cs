using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a value type is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class ValueTypeAssertions<TSubject, TAssertions>(TSubject subject, AssertionChain assertionChain)
    : ValueTypeAssertionsBase<TSubject, TSubject, TAssertions>(assertionChain)
    where TSubject : struct
    where TAssertions : ValueTypeAssertions<TSubject, TAssertions>
{
    /// <inheritdoc/>
    public override TSubject Subject { get; } = subject;
}
