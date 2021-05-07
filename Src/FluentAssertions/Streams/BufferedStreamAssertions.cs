using System.Diagnostics;
using System.IO;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1
using FluentAssertions.Execution;
#endif

namespace FluentAssertions.Streams
{
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

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1
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
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the {context:stream} buffer size to be {0}{reason}, ", expected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> BufferedStream.")
                .Then
                .ForCondition(Subject.BufferSize == expected)
                .FailWith("but it was {0}.", Subject.BufferSize)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="BufferedStream"/> has not the <paramref name="unexpected"/> buffer size.
        /// </summary>
        /// <param name="unexpected">The not expected buffer size of the current stream.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveBufferSize(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the {context:stream} buffer size not to be {0}{reason}, ", unexpected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> BufferedStream.")
                .Then
                .ForCondition(Subject.BufferSize != unexpected)
                .FailWith("but it was {0}.", Subject.BufferSize)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
#endif
    }
}
