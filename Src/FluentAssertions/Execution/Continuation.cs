namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
/// </summary>
public class Continuation
{
    internal Continuation(AssertionChain assertionChain)
    {
        this.Then = assertionChain;
    }

    /// <summary>
    /// Continuous the assertion chain if the previous assertion was successful.
    /// </summary>
    public AssertionChain Then { get; }
}
