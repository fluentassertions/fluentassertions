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
   // [DebuggerNonUserCode]
    public class FunctionAssertions<T> : DelegateAssertions<Func<T>>
    {
        public FunctionAssertions(Func<T> subject, IExtractExceptions extractor) : base(subject, extractor)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{T}"/> that is being asserted.
        /// </summary>
        public new Func<T> Subject { get; }

        protected override void InvokeSubject()
        {
            Subject();
        }

        protected override string Identifier => "function";

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
        public new AndWhichConstraint<FunctionAssertions<T>, T> NotThrow(string because = "", params object[] becauseArgs)
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
        public new AndWhichConstraint<FunctionAssertions<T>, T> NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
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

    }
}
