namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions from a <see cref="AssertionScope.Given{T}"/> call.
/// </summary>
public class NewContinuationOfGiven<TSubject>
{
    internal NewContinuationOfGiven(NewGivenSelector<TSubject> parent)
    {
        Then = parent;
    }

    /// <summary>
    /// Continuous the assertion chain if the previous assertion was successful.
    /// </summary>
    public NewGivenSelector<TSubject> Then { get; }
}
