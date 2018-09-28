using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an asynchronous method yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class AsyncFunctionAssertions
    {
        private readonly IExtractExceptions extractor;

        public AsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{Task}"/> that is being asserted.
        /// </summary>
        public Func<Task> Subject { get; private set; }

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
            Exception exception = InvokeSubjectWithInterception();
            return Throw<TException>(exception, because, becauseArgs);
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
        public void NotThrow(string because = "", params object[] becauseArgs)
        {
            try
            {
                Task.Run(Subject).Wait();
            }
            catch (Exception exception)
            {
                Exception nonAggregateException = GetFirstNonAggregateException(exception);

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exception{reason}, but found a {0} with message {1}.",
                        nonAggregateException.GetType(), nonAggregateException.ToString());
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

        private Exception InvokeSubjectWithInterception()
        {
            try
            {
                Task.Run(Subject).Wait();
            }
            catch (Exception exception)
            {
                return InterceptException(exception);
            }

            return null;
        }

        private async Task<Exception> InvokeSubjectWithInterceptionAsync()
        {
            try
            {
                await Task.Run(Subject);
            }
            catch (Exception exception)
            {
                return InterceptException(exception);
            }

            return null;
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
