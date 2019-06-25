using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an asynchronous method yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class AsyncFunctionAssertions : DelegateAssertions<Func<Task>>
    {
        private readonly IClock clock;

        public AsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor) : this(subject, extractor, new Clock())
        {
        }

        public AsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor, IClock clock) : base(subject, extractor, clock)
        {
            this.clock = clock;
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{Task}"/> that is being asserted.
        /// </summary>
        public new Func<Task> Subject { get; }

        protected override string Identifier => "async function";

        protected override void InvokeSubject()
        {
            Subject().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws an exception of the exact type <typeparamref name="TException"/> (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public async Task<ExceptionAssertions<TException>> ThrowExactlyAsync<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            Exception exception = await InvokeSubjectWithInterceptionAsync();

            Type expectedType = typeof(TException);

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Exception_ExpectedXFormat + Resources.Exception_CommaButNoExceptionWasThrown,
                    expectedType);

            exception.Should().BeOfType(expectedType, because, becauseArgs);

            return new ExceptionAssertions<TException>(new[] { exception as TException });
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task<ExceptionAssertions<TException>> ThrowAsync<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            Exception exception = await InvokeSubjectWithInterceptionAsync();
            return Throw<TException>(exception, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> does not throw any exception.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task NotThrowAsync(string because = "", params object[] becauseArgs)
        {
            try
            {
                await Subject();
            }
            catch (Exception exception)
            {
                NotThrow(exception, because, becauseArgs);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> does not throw an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task NotThrowAsync<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            try
            {
                await Subject();
            }
            catch (Exception exception)
            {
                NotThrow<TException>(exception, because, becauseArgs);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> stops throwing any exception
        /// after a specified amount of time.
        /// </summary>
        /// <remarks>
        /// The <see cref="Func{T}"/> is invoked. If it raises an exception,
        /// the invocation is repeated until it either stops raising any exceptions
        /// or the specified wait time is exceeded.
        /// </remarks>
        /// <param name="waitTime">
        /// The time after which the <see cref="Func{T}"/> should have stopped throwing any exception.
        /// </param>
        /// <param name="pollInterval">
        /// The time between subsequent invocations of the <see cref="Func{T}"/>.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if waitTime or pollInterval are negative.</exception>
        public Task NotThrowAfterAsync(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime),
                    string.Format(Resources.TimeSpan_TheValueOfXMustBeNonNegativeFormat, nameof(waitTime)));
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval),
                    string.Format(Resources.TimeSpan_TheValueOfXMustBeNonNegativeFormat, nameof(pollInterval)));
            }

            return assertionTask();

            async Task assertionTask()
            {
                TimeSpan? invocationEndTime = null;
                Exception exception = null;
                var timer = clock.StartTimer();

                while (invocationEndTime is null || invocationEndTime < waitTime)
                {
                    exception = await InvokeSubjectWithInterceptionAsync();
                    if (exception is null)
                    {
                        return;
                    }

                    await clock.DelayAsync(pollInterval, CancellationToken.None);
                    invocationEndTime = timer.Elapsed;
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.TimeSpan_DidNotExpectAnyExceptionsAfterXFormat + Resources.Common_CommaButFoundYFormat,
                        waitTime, exception);
            }
        }

        private async Task<Exception> InvokeSubjectWithInterceptionAsync()
        {
            try
            {
                await Subject();
                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}
