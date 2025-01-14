namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions from a <see cref="AssertionChain.Given{T}"/> call.
/// </summary>
public class ContinuationOfGiven<TSubject>
{
    internal ContinuationOfGiven(GivenSelector<TSubject> parent)
    {
        Then = parent;
    }

    /// <summary>
    /// Continues the assertion chain if the previous assertion was successful.
    /// </summary>
    public GivenSelector<TSubject> Then { get; }

    public bool Succeeded => Then.Succeeded;
}
