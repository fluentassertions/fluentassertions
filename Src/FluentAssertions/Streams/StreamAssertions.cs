using System;
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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} to be writable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.CanWrite)
                    .FailWith("Expected {context:stream} to be writable{reason}, but it was not.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} not to be writable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(!Subject.CanWrite)
                    .FailWith("Expected {context:stream} not to be writable{reason}, but it was.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} to be seekable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.CanSeek)
                    .FailWith("Expected {context:stream} to be seekable{reason}, but it was not.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} not to be seekable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(!Subject.CanSeek)
                    .FailWith("Expected {context:stream} not to be seekable{reason}, but it was.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} to be readable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.CanRead)
                    .FailWith("Expected {context:stream} to be readable{reason}, but it was not.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} not to be readable{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(!Subject.CanRead)
                    .FailWith("Expected {context:stream} not to be readable{reason}, but it was.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected the position of {context:stream} to be {0}{reason}, but found a <null> reference.",
                    expected);

            if (success)
            {
                long position;

                try
                {
                    position = Subject.Position;
                }
                catch (Exception exception)
                    when (exception is IOException or NotSupportedException or ObjectDisposedException)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected the position of {context:stream} to be {0}{reason}, but it failed with:{1}{2}",
                            expected, Environment.NewLine, exception.Message);

                    return new AndConstraint<TAssertions>((TAssertions)this);
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(position == expected)
                    .FailWith("Expected the position of {context:stream} to be {0}{reason}, but it was {1}.",
                        expected, position);
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected the position of {context:stream} not to be {0}{reason}, but found a <null> reference.",
                    unexpected);

            if (success)
            {
                long position;

                try
                {
                    position = Subject.Position;
                }
                catch (Exception exception)
                    when (exception is IOException or NotSupportedException or ObjectDisposedException)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected the position of {context:stream} not to be {0}{reason}, but it failed with:{1}{2}",
                            unexpected, Environment.NewLine, exception.Message);

                    return new AndConstraint<TAssertions>((TAssertions)this);
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(position != unexpected)
                    .FailWith("Expected the position of {context:stream} not to be {0}{reason}, but it was.",
                        unexpected);
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected the length of {context:stream} to be {0}{reason}, but found a <null> reference.",
                    expected);

            if (success)
            {
                long length;

                try
                {
                    length = Subject.Length;
                }
                catch (Exception exception)
                    when (exception is IOException or NotSupportedException or ObjectDisposedException)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected the length of {context:stream} to be {0}{reason}, but it failed with:{1}{2}",
                            expected, Environment.NewLine, exception.Message);

                    return new AndConstraint<TAssertions>((TAssertions)this);
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(length == expected)
                    .FailWith("Expected the length of {context:stream} to be {0}{reason}, but it was {1}.",
                        expected, length);
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected the length of {context:stream} not to be {0}{reason}, but found a <null> reference.",
                    unexpected);

            if (success)
            {
                long length;

                try
                {
                    length = Subject.Length;
                }
                catch (Exception exception)
                    when (exception is IOException or NotSupportedException or ObjectDisposedException)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected the length of {context:stream} not to be {0}{reason}, but it failed with:{1}{2}",
                            unexpected, Environment.NewLine, exception.Message);

                    return new AndConstraint<TAssertions>((TAssertions)this);
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(length != unexpected)
                    .FailWith("Expected the length of {context:stream} not to be {0}{reason}, but it was.",
                        unexpected);
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} to be read-only{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(!Subject.CanWrite && Subject.CanRead)
                    .FailWith("Expected {context:stream} to be read-only{reason}, but it was writable or not readable.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} not to be read-only{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.CanWrite || !Subject.CanRead)
                    .FailWith("Expected {context:stream} not to be read-only{reason}, but it was.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} to be write-only{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.CanWrite && !Subject.CanRead)
                    .FailWith("Expected {context:stream} to be write-only{reason}, but it was readable or not writable.");
            }

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
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:stream} not to be write-only{reason}, but found a <null> reference.");

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(!Subject.CanWrite || Subject.CanRead)
                    .FailWith("Expected {context:stream} not to be write-only{reason}, but it was.");
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
