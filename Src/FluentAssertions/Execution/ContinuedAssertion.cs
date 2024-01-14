using System;

namespace FluentAssertions.Execution;

public class ContinuedAssertion : Assertion<ContinuedAssertion>
{
    public ContinuedAssertion(IAssertion previousAssertion)
        : base(previousAssertion)
    {
    }
}
