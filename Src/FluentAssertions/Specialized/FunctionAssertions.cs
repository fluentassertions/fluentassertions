using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that a synchronous function yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class FunctionAssertions<T> : ReferenceTypeAssertions<Func<T>, FunctionAssertions<T>>
    {
        private readonly IExtractExceptions extractor;

        public FunctionAssertions(Func<T> subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

        protected override string Identifier => "function";

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
                return new AndWhichConstraint<FunctionAssertions<T>, T>(this, default(T));
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
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exception{reason}, but found a {0} with message {1}.",
                    exception.GetType(), exception.ToString());
        }

        private void NotThrow<TException>(Exception exception, string because, object[] becauseArgs) where TException : Exception
        {
            var exceptions = extractor.OfType<TException>(exception);

            if (exceptions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {0}{reason}, but found one with message {1}.",
                        typeof(TException), exceptions.First().ToString());
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
        public AndWhichConstraint<FunctionAssertions<T>, T> NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
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
                try
                {
                    T result = Subject();
                    return new AndWhichConstraint<FunctionAssertions<T>, T>(this, result);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                Task.Delay(pollInterval).GetAwaiter().GetResult();
                invocationEndTime = watch.Elapsed;
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

            return new AndWhichConstraint<FunctionAssertions<T>, T>(this, default(T));
        }

        private ExceptionAssertions<TException> Throw<TException>(Exception exception, string because, object[] becauseArgs)
            where TException : Exception
        {
            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", typeof(TException));

            var exceptions = extractor.OfType<TException>(exception);

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
                return exception;
            }
        }
    }
}
