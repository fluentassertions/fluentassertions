using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Adapts an <see cref="IAssertionStrategy"/> to the <see cref="IAssertionStrategy2"/> interface
/// for backwards compatibility.
/// </summary>
[System.Diagnostics.StackTraceHidden]
internal class AssertionStrategyAdapter : IAssertionStrategy2
{
    private readonly IAssertionStrategy inner;

    public AssertionStrategyAdapter(IAssertionStrategy inner)
    {
        this.inner = inner;
    }

    public IEnumerable<AssertionFailure> Failures =>
        inner.FailureMessages.Select(m => new AssertionFailure(m));

    public int FailureCount => inner.FailureMessages.Count();

    public void HandleFailure(AssertionFailure failure)
    {
        inner.HandleFailure(failure.ToString());
    }

    public IEnumerable<AssertionFailure> DiscardFailures()
    {
        return inner.DiscardFailures().Select(m => new AssertionFailure(m)).ToArray();
    }

    public void ThrowIfAny(IDictionary<string, object> context)
    {
        inner.ThrowIfAny(context);
    }
}
