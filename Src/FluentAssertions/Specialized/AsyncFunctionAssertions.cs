using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an asynchronous function yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class AsyncFunctionAssertions<T>
    {
        private readonly IExtractExceptions extractor;

        public AsyncFunctionAssertions(Func<Task<T>> subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{Task}"/> that is being asserted.
        /// </summary>
        public Func<Task<T>> Subject { get; private set; }

        /// <summary>
        /// Gets the result of the <see cref="Func{Task}"/>.
        /// </summary>
        public T Result => this.Subject().Result;

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
        public ExceptionAssertions<TException> Throw<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            var result = InvokeSubjectWithInterception();
            return Throw<TException>(result.exception, because, becauseArgs);
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
        public async Task<ExceptionAssertions<TException>> ThrowAsync<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            var result = await InvokeSubjectWithInterceptionAsync();
            return Throw<TException>(result.exception, because, becauseArgs);
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
        public AndWhichConstraint<AsyncFunctionAssertions<T>, T> NotThrow(
            string because = "",
            params object[] becauseArgs)
        {
            try
            {
                T result = Task.Run(Subject).Result;
                return new AndWhichConstraint<AsyncFunctionAssertions<T>, T>(this, result);
            }
            catch (Exception exception)
            {
                NotThrow(exception, because, becauseArgs);
                return null;
            }
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
        public async Task<AndWhichConstraint<AsyncFunctionAssertions<T>, T>> NotThrowAsync(string because = "", params object[] becauseArgs)
        {
            try
            {
                T result = await Task.Run(Subject);
                return new AndWhichConstraint<AsyncFunctionAssertions<T>, T>(this, result);
            }
            catch (Exception exception)
            {
                NotThrow(exception, because, becauseArgs);
                return null;
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
        public void NotThrow<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            try
            {
                Task.Run(Subject).Wait();
            }
            catch (Exception exception)
            {
                NotThrow<TException>(exception, because, becauseArgs);
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
                await Task.Run(Subject);
            }
            catch (Exception exception)
            {
                NotThrow<TException>(exception, because, becauseArgs);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <param name="asyncFunctionAssertions">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public ExceptionAssertions<TException> ThrowExactly<TException>(
            string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = this.Throw<TException>(because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public async Task<ExceptionAssertions<TException>> ThrowExactlyAsync<TException>(
            string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = await this.ThrowAsync<TException>(because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        private static void NotThrow(Exception exception, string because, object[] becauseArgs)
        {
            Exception nonAggregateException = GetFirstNonAggregateException(exception);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exception{reason}, but found a {0} with message {1}.",
                    nonAggregateException.GetType(), nonAggregateException.ToString());
        }

        private static void NotThrow<TException>(Exception exception, string because, object[] becauseArgs) where TException : Exception
        {
            Exception nonAggregateException = GetFirstNonAggregateException(exception);

            if (nonAggregateException != null)
            {
                Execute.Assertion
                    .ForCondition(!(nonAggregateException is TException))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {0}{reason}, but found one with message {1}.",
                        typeof(TException), nonAggregateException.ToString());
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
        public void NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), $"The value of {nameof(waitTime)} must be non-negative.");
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval), $"The value of {nameof(pollInterval)} must be non-negative.");
            }

            TimeSpan? invocationEndTime = null;
            Exception exception = null;
            var watch = Stopwatch.StartNew();

            while (invocationEndTime is null || invocationEndTime < waitTime)
            {
                var result = InvokeSubjectWithInterception();
                exception = result.exception;
                if (exception is null)
                {
                    return;
                }

                Task.Delay(pollInterval).Wait();
                invocationEndTime = watch.Elapsed;
            }
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);
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
        public
        Task NotThrowAfterAsync(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), $"The value of {nameof(waitTime)} must be non-negative.");
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval), $"The value of {nameof(pollInterval)} must be non-negative.");
            }

            return assertionTask();

            async Task assertionTask()
            {
                TimeSpan? invocationEndTime = null;
                Exception exception = null;
                var watch = Stopwatch.StartNew();

                while (invocationEndTime is null || invocationEndTime < waitTime)
                {
                    var result = await InvokeSubjectWithInterceptionAsync();
                    exception = result.exception;
                    if (exception is null)
                    {
                        return;
                    }

                    await Task.Delay(pollInterval);
                    invocationEndTime = watch.Elapsed;
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> will complete within specified time range.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<AsyncFunctionAssertions<T>, T> CompleteWithin(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var task = Task.Run(Subject);
            var completed = task.Wait(timeSpan);
            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndWhichConstraint<AsyncFunctionAssertions<T>, T>(this, task.Result);
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> will complete within specified time range.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task<AndWhichConstraint<AsyncFunctionAssertions<T>, T>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var delayTask = Task.Delay(timeSpan);
            var subjectTask = Task.Run(Subject);
            var completedTask = await Task.WhenAny(subjectTask, delayTask);
            Execute.Assertion
                .ForCondition(completedTask.Equals(subjectTask))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndWhichConstraint<AsyncFunctionAssertions<T>, T>(this, subjectTask.Result);
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public ExceptionAssertions<TException> ThrowWithin<TException>(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            var result = InvokeSubjectWithInterception(timeSpan);
            Execute.Assertion
                .ForCondition(result.completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to throw within {0}{reason}.", timeSpan);
            return Throw<TException>(result.exception, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task<ExceptionAssertions<TException>> ThrowWithinAsync<TException>(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            var result = await InvokeSubjectWithInterceptionAsync(timeSpan);
            Execute.Assertion
                .ForCondition(result.completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to throw within {0}{reason}.", timeSpan);
            return Throw<TException>(result.exception, because, becauseArgs);
        }

        private static Exception GetFirstNonAggregateException(Exception exception)
        {
            Exception nonAggregateException = exception;
            while (nonAggregateException is AggregateException)
            {
                nonAggregateException = nonAggregateException.InnerException;
            }

            return nonAggregateException;
        }

        private ExceptionAssertions<TException> Throw<TException>(Exception exception, string because, object[] becauseArgs)
            where TException : Exception
        {
            var exceptions = extractor.OfType<TException>(exception);

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", typeof(TException));

            Execute.Assertion
                .ForCondition(exceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", typeof(TException), exception);

            return new ExceptionAssertions<TException>(exceptions);
        }

        private (bool completed, Exception exception) InvokeSubjectWithInterception(TimeSpan? timeout = null)
        {
            try
            {
                if (timeout.HasValue)
                {
                    var completed = Task.Run(Subject).Wait(timeout.Value);
                    return (completed, null);
                }
                else
                {
                    Task.Run(Subject).Wait();
                    return (true, null);
                }
            }
            catch (Exception exception)
            {
                return (true, InterceptException(exception));
            }
        }

        private async Task<(bool completed, Exception exception)> InvokeSubjectWithInterceptionAsync(TimeSpan? timeout = null)
        {
            try
            {
                if (timeout.HasValue)
                {
                    var delayTask = Task.Delay(timeout.Value);
                    var subjectTask = Task.Run(Subject);
                    var completedTask = await Task.WhenAny(subjectTask, delayTask);
                    var completed = completedTask.Equals(subjectTask);
                    if (completed)
                    {
                        return (true, completedTask.Exception);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                else
                {
                    await Task.Run(Subject);
                    return (true, null);
                }
            }
            catch (Exception exception)
            {
                return (true, InterceptException(exception));
            }
        }

        private Exception InterceptException(Exception exception)
        {
            var ar = exception as AggregateException;
            if (ar?.InnerException is AggregateException)
            {
                return ar.InnerException;
            }

            return exception;
        }
    }
}
