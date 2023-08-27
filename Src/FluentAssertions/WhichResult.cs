using FluentAssertions.Execution;

namespace FluentAssertions;

public class WhichResult<T>
{
    public WhichResult(T matchedElement, Assertion assertion)
    {
        MatchedElement = matchedElement;
        Assertion = assertion;
    }

    public T MatchedElement { get; }

    public Assertion Assertion { get; }
}
