using System.Diagnostics;
using System.IO;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Streams
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Stream"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class StreamAssertions : StreamAssertions<Stream, StreamAssertions>
    {
        public StreamAssertions(Stream stream)
            : base(stream)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <typeparamref name="TSubject"/> is in the expected state.
    /// </summary>
    public class StreamAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TSubject : Stream
        where TAssertions : StreamAssertions<TSubject, TAssertions>
    {
        public StreamAssertions(TSubject stream)
            : base(stream)
        {
        }

        protected override string Identifier => "stream";

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is writable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeWritable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} to be writable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanWrite)
                .FailWith("but it was not.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is not writable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeWritable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} not to be writable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(!Subject.CanWrite)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is seekable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeSeekable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} to be seekable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanSeek)
                .FailWith("but it was not.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is not seekable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeSeekable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} not to be seekable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(!Subject.CanSeek)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is readable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeReadable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} to be readable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanRead)
                .FailWith("but it was not.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is not readable.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeReadable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} not to be readable{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(!Subject.CanRead)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> has the <paramref name="expected"/> position.
        /// </summary>
        /// <param name="expected">The expected position of the current stream.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HavePosition(long expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the position of {context:stream} to be {0}{reason}, ", expected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanSeek)
                .FailWith("but found a non-seekable stream.")
                .Then
                .ForCondition(Subject.Position == expected)
                .FailWith("but it was {0}.", Subject.Position)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> does not have an <paramref name="unexpected"/> position.
        /// </summary>
        /// <param name="unexpected">The unexpected position of the current stream.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHavePosition(long unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the position of {context:stream} not to be {0}{reason}, ", unexpected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanSeek)
                .FailWith("but found a non-seekable stream.")
                .Then
                .ForCondition(Subject.Position != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> has the <paramref name="expected"/> length.
        /// </summary>
        /// <param name="expected">The expected length of the current stream.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveLength(long expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the length of {context:stream} to be {0}{reason}, ", expected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanSeek)
                .FailWith("but found a non-seekable stream.")
                .Then
                .ForCondition(Subject.Length == expected)
                .FailWith("but it was {0}.", Subject.Length)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> does not have an <paramref name="unexpected"/> length.
        /// </summary>
        /// <param name="unexpected">The unexpected length of the current stream.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveLength(long unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the length of {context:stream} not to be {0}{reason}, ", unexpected)
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanSeek)
                .FailWith("but found a non-seekable stream.")
                .Then
                .ForCondition(Subject.Length != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is read-only.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeReadOnly(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} to be read-only{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(!Subject.CanWrite && Subject.CanRead)
                .FailWith("but it was writable or not readable.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is not read-only.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeReadOnly(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} not to be read-only{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanWrite || !Subject.CanRead)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is write-only.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeWriteOnly(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} to be write-only{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(Subject.CanWrite && !Subject.CanRead)
                .FailWith("but it was readable or not writable.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Stream"/> is not write-only.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeWriteOnly(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:stream} not to be write-only{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but found a <null> reference.")
                .Then
                .ForCondition(!Subject.CanWrite || Subject.CanRead)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
