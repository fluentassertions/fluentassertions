using System;
using System.Diagnostics;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that a synchronous function yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class FunctionAssertions<T>
    {
        private readonly IExtractExceptions extractor;

        public FunctionAssertions(Func<T> subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{T}"/> that is being asserted.
        /// </summary>
        public Func<T> Subject { get; }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> throws an exception of type <typeparamref name="TException"/>.
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
        /// Asserts that the current <see cref="Func{T}"/> throws an exception of the exact type <typeparamref name="TException"/> (and not a derived exception type).
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public ExceptionAssertions<TException> ThrowExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = Throw<TException>(because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> does not throw any exception.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<FunctionAssertions<T>, T> NotThrow(string because = "", params object[] becauseArgs)
        {
            try
            {
                T result = Subject();
                return new AndWhichConstraint<FunctionAssertions<T>, T>(this, result);
            }
            catch (Exception exception)
            {
                NotThrow(exception, because, becauseArgs);
                return null;

            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Func{T}"/> does not throw an exception of type <typeparamref name="TException"/>.
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
                Subject();
            }
            catch (Exception exception)
            {
                NotThrow<TException>(exception, because, becauseArgs);
            }
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
                Subject();
                return null;
            }
            catch (Exception exception)
            {
                return InterceptException(exception);
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
