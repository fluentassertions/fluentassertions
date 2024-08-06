using FluentAssertions.Execution;

namespace FluentAssertions;

public class WhichResult<T>
{
    public WhichResult(T matchedElement, AssertionChain assertionChain)
    {
        MatchedElement = matchedElement;
        AssertionChain = assertionChain;
    }

    public T MatchedElement { get; }

    public AssertionChain AssertionChain { get; }
}
