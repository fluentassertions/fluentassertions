using System;

namespace FluentAssertions.Execution;

public class ContinuedAssertion : Assertion<ContinuedAssertion>
{
    public ContinuedAssertion(IAssertion previousAssertion)
        : base(previousAssertion)
    {
    }

    public NewGivenSelector<T> Given<T>(Func<T> selector)
    {
        return new NewGivenSelector<T>(selector, this);
    }
}
