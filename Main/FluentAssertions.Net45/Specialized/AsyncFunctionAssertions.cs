using System;
using System.Diagnostics;
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
        protected internal AsyncFunctionAssertions(Func<Task> subject)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Func{Task}"/> that is being asserted.
        /// </summary>
        public Func<Task> Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason, object[] reasonArgs)
            where TException : Exception
        {
            Exception exception = null;

            try
            {
                Task task = Subject();
                task.Wait();
            }
            catch (AggregateException aggregateException)
            {
                exception = aggregateException.InnerException;
            }

            Execute.Verification
                .ForCondition(exception != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", typeof(TException));

            Execute.Verification
                .ForCondition(exception is TException)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", typeof(TException), exception);

            return new ExceptionAssertions<TException>((TException)exception);            
        }
    }
}