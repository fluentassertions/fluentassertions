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
        public ExceptionAssertions<TException> ShouldThrow<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Exception exception = InvokeSubjectWithInterception();
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
        public void ShouldNotThrow(string because = "", params object[] becauseArgs)
        {
            try
            {
#if NETSTANDARD1_3
                Task.Run(Subject).Wait();
#else
                Task.Factory.StartNew(() => Subject().Wait()).Wait();
#endif
            }
            catch (Exception exception)
            {
                while (exception is AggregateException)
                {
                    exception = exception.InnerException;
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exception{reason}, but found a {0} with message {1}.",
                        exception.GetType(), exception.Message);
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
        public void ShouldNotThrow<TException>(string because = "", params object[] becauseArgs)
        {
            try
            {
#if NETSTANDARD1_3
                Task.Run(Subject).Wait();
#else
                Task.Factory.StartNew(() => Subject().Wait()).Wait();
#endif
            }
            catch (Exception exception)
            {
                while (exception is AggregateException)
                {
                    exception = exception.InnerException;
                }

                if (exception != null)
                {
                    Execute.Assertion
                        .ForCondition(!(exception is TException))
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Did not expect {0}{reason}, but found one with message {1}.",
                            typeof(TException), exception.Message);
                }
            }
        }

        private Exception InvokeSubjectWithInterception()
        {
            Exception actualException = null;

            try
            {
#if NETSTANDARD1_3
                Task.Run(Subject).Wait();
#else
                Task.Factory.StartNew(() => Subject().Wait()).Wait();
#endif
            }
            catch (Exception exception)
            {
                var ar = exception as AggregateException;
                if (ar?.InnerException is AggregateException)
                {
                    actualException = ar.InnerException;
                }
                else
                {
                    actualException = exception;
                }
            }

            return actualException;
        }
    }
}