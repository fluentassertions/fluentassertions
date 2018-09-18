using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Action"/> yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class ActionAssertions : ReferenceTypeAssertions<Action, ActionAssertions>
    {
        private readonly IExtractExceptions extractor;

        public ActionAssertions(Action subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> throws an exception of type <typeparamref name="TException"/>.
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
            FailIfSubjectIsAsyncVoid();

            Exception actualException = InvokeSubjectWithInterception();
            IEnumerable<TException> expectedExceptions = extractor.OfType<TException>(actualException);

            Execute.Assertion
                .ForCondition(actualException != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a <{0}> to be thrown{reason}, but no exception was thrown.", typeof(TException));

            Execute.Assertion
                .ForCondition(expectedExceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected a <{0}> to be thrown{reason}, but found a <{1}>: {3}.",
                    typeof(TException), actualException.GetType(),
                    Environment.NewLine,
                    actualException);

            return new ExceptionAssertions<TException>(expectedExceptions);
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> does not throw an exception of type <typeparamref name="TException"/>.
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
            FailIfSubjectIsAsyncVoid();

            Exception actualException = InvokeSubjectWithInterception();

            if (actualException != null)
            {
                IEnumerable<TException> expectedExceptions = extractor.OfType<TException>(actualException);

                Execute.Assertion
                    .ForCondition(!expectedExceptions.Any())
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {0}{reason}, but found {1}.", typeof(TException), actualException);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> does not throw any exception.
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
            FailIfSubjectIsAsyncVoid();

            try
            {
                Subject();
            }
            catch (Exception exception)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exception{reason}, but found {0}.", exception);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> stops throwing any exception
        /// after a specified amount of time.
        /// </summary>
        /// <remarks>
        /// The <see cref="Action"/> is invoced. If it raises an exception,
        /// the invocation is repeated until it either stops raising any exceptions
        /// or the specified wait time is exceeded.
        /// </remarks>
        /// <param name="waitTime">
        /// The time in milliseconds after which the <see cref="Action"/> should habe stopped throwing any exception.
        /// </param>
        /// <param name="pollInterval">
        /// The time between subsequent invocations of the <see cref="Action"/>.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if waitTime or pollInterval are negative.</exception>
        public void NotThrow(int waitTime, int pollInterval, string because = "", params object[] becauseArgs)
        {
            FailIfSubjectIsAsyncVoid();

            if(waitTime < 0 || pollInterval < 0)
                throw new ArgumentOutOfRangeException("The values of waitTime and pollInterval must be positive.");

            var watch = Stopwatch.StartNew();
            long invocationEndTime = -1;
            Exception exception = null;

            while (invocationEndTime < waitTime)
            {
                exception = InvokeSubjectWithInterception();
                if (exception is null)
                    return;

                Task.Delay(pollInterval).Wait();
                invocationEndTime = watch.ElapsedMilliseconds;
            }
            Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Did not expect any exception after {0} milliseconds{reason}, but found {1}.", waitTime, exception);
        }

        private Exception InvokeSubjectWithInterception()
        {
            Exception actualException = null;

            try
            {
                Subject();
            }
            catch (Exception exc)
            {
                actualException = exc;
            }

            return actualException;
        }

        private void FailIfSubjectIsAsyncVoid()
        {
            if (Subject.GetMethodInfo().HasAttribute<AsyncStateMachineAttribute>())
            {
                throw new InvalidOperationException("Cannot use action assertions on an async void method. Assign the async method to a variable of type Func<Task> instead of Action so that it can be awaited.");
            }
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "action";
    }
}
