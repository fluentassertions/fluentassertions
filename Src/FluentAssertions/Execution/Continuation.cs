namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
/// </summary>
public class Continuation
{
    private readonly AssertionChain parent;

    internal Continuation(AssertionChain parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// Continues the assertion chain if the previous assertion was successful.
    /// </summary>
    public AssertionChain Then => parent.ClearExpectation();
}
