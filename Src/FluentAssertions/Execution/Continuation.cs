namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public class Continuation
{
    internal Continuation(AssertionChain parent)
    {
        Then = parent;
    }

    /// <summary>
    /// Continues the assertion chain if the previous assertion was successful.
    /// </summary>
    public AssertionChain Then { get; }
}
