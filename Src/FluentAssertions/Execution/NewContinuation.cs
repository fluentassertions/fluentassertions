namespace FluentAssertions.Execution;

/// <summary>
/// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
/// </summary>
public class NewContinuation<TAssertion>
    where TAssertion : Assertion<TAssertion>
{
    private readonly IAssertion assertion;

    internal NewContinuation(IAssertion assertion)
    {
        this.assertion = assertion;
    }

    /// <summary>
    /// Continuous the assertion chain if the previous assertion was successful.
    /// </summary>
    public ContinuedAssertion Then
    {
        get
        {
            return new ContinuedAssertion(assertion);
        }
    }
}
