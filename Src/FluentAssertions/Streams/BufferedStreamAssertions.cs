using System.Diagnostics;
using System.IO;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Streams;

/// <summary>
/// Contains a number of methods to assert that an <see cref="Stream"/> is in the expected state.
/// </summary>
///
[DebuggerNonUserCode]
public class BufferedStreamAssertions : BufferedStreamAssertions<BufferedStreamAssertions>
{
    public BufferedStreamAssertions(BufferedStream stream)
        : base(stream)
    {
    }
}

public class BufferedStreamAssertions<TAssertions> : StreamAssertions<BufferedStream, TAssertions>
    where TAssertions : BufferedStreamAssertions<TAssertions>
{
    public BufferedStreamAssertions(BufferedStream stream)
        : base(stream)
    {
    }

    protected override string Identifier => "buffered stream";

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Asserts that the current <see cref="BufferedStream"/> has the <paramref name="expected"/> buffer size.
    /// </summary>
    /// <param name="expected">The expected buffer size of the current stream.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveBufferSize(int expected, string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the buffer size of {context:stream} to be {0}{reason}, but found a <null> reference.",
                expected);

        if (success)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject!.BufferSize == expected)
                .FailWith("Expected the buffer size of {context:stream} to be {0}{reason}, but it was {1}.",
                    expected, Subject.BufferSize);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="BufferedStream"/> does not have a buffer size of <paramref name="unexpected"/>.
    /// </summary>
    /// <param name="unexpected">The unexpected buffer size of the current stream.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveBufferSize(int unexpected, string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the buffer size of {context:stream} not to be {0}{reason}, but found a <null> reference.",
                unexpected);

        if (success)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject!.BufferSize != unexpected)
                .FailWith("Expected the buffer size of {context:stream} not to be {0}{reason}, but it was.",
                    unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
#endif
}
