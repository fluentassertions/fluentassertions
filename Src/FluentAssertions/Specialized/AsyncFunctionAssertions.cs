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
    public class AsyncFunctionAssertions<TTask, TAssertions> : DelegateAssertions<Func<TTask>, TAssertions>
        where TTask : Task
        where TAssertions : AsyncFunctionAssertions<TTask, TAssertions>
    {
        public AsyncFunctionAssertions(Func<TTask> subject, IExtractExceptions extractor) : this(subject, extractor, new Clock())
        {
        }

        public AsyncFunctionAssertions(Func<TTask> subject, IExtractExceptions extractor, IClock clock) : base(subject, extractor, clock)
        {
        }

        protected override string Identifier => "async function";

        private protected override bool CanHandleAsync => true;

        protected override void InvokeSubject()
        {
            Subject.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TTask"/> will complete within specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> CompleteWithin(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}, but found <null>.", timeSpan);

            TTask task = Subject.ExecuteInDefaultSynchronizationContext();
            bool completed = Clock.Wait(task, timeSpan);

            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TTask"/> will complete within the specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public async Task<AndConstraint<TAssertions>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}, but found <null>.", timeSpan);

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            TTask task = Subject.ExecuteInDefaultSynchronizationContext();

            Task completedTask =
                await Task.WhenAny(task, Clock.DelayAsync(timeSpan, timeoutCancellationTokenSource.Token));

            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                await completedTask;
            }

            Execute.Assertion
                .ForCondition(completedTask == task)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public async Task<ExceptionAssertions<TException>> ThrowExactlyAsync<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            Type expectedType = typeof(TException);

            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to throw exactly {0}{reason}, but found <null>.", expectedType);

            Exception exception = await InvokeWithInterceptionAsync(Subject.ExecuteInDefaultSynchronizationContext);

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", expectedType);

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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public async Task<ExceptionAssertions<TException>> ThrowAsync<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to throw {0}{reason}, but found <null>.", typeof(TException));

            Exception exception = await InvokeWithInterceptionAsync(Subject.ExecuteInDefaultSynchronizationContext);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public async Task<AndConstraint<TAssertions>> NotThrowAsync(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw{reason}, but found <null>.");

            try
            {
                await Subject.ExecuteInDefaultSynchronizationContext();
            }
            catch (Exception exception)
            {
                NotThrow(exception, because, becauseArgs);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> does not throw an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public async Task<AndConstraint<TAssertions>> NotThrowAsync<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw{reason}, but found <null>.");

            try
            {
                await Subject.ExecuteInDefaultSynchronizationContext();
            }
            catch (Exception exception)
            {
                NotThrow<TException>(exception, because, becauseArgs);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if waitTime or pollInterval are negative.</exception>
        public Task<AndConstraint<TAssertions>> NotThrowAfterAsync(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), $"The value of {nameof(waitTime)} must be non-negative.");
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval),
                    $"The value of {nameof(pollInterval)} must be non-negative.");
            }

            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

            Func<Task> wrappedSubject = Subject.ExecuteInDefaultSynchronizationContext;

            return assertionTask();

            async Task<AndConstraint<TAssertions>> assertionTask()
            {
                TimeSpan? invocationEndTime = null;
                Exception exception = null;
                ITimer timer = Clock.StartTimer();

                while (invocationEndTime is null || invocationEndTime < waitTime)
                {
                    exception = await InvokeWithInterceptionAsync(wrappedSubject);
                    if (exception is null)
                    {
                        return new AndConstraint<TAssertions>((TAssertions)this);
                    }

                    await Clock.DelayAsync(pollInterval, CancellationToken.None);
                    invocationEndTime = timer.Elapsed;
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

                return new AndConstraint<TAssertions>((TAssertions)this);
            }
        }

        private static async Task<Exception> InvokeWithInterceptionAsync(Func<Task> action)
        {
            try
            {
                await action();
                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}
