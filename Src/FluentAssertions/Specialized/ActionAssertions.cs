using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NETSTANDARD1_3 && !NETSTANDARD1_6
using System.Threading;
#endif
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
        /// Asserts that the current <see cref="Action"/> throws the exact exception (and not a derived exception type).
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
        public ExceptionAssertions<TException> ThrowExactly<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            FailIfSubjectIsAsyncVoid();

            Exception exception = InvokeSubjectWithInterception();

            Type expectedType = typeof(TException);

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", expectedType);

            exception.Should().BeOfType(expectedType, because, becauseArgs);

            return new ExceptionAssertions<TException>(new[] { exception as TException });
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

#if !NETSTANDARD1_3 && !NETSTANDARD1_6
        /// <summary>
        /// Asserts that the current <see cref="Action"/> stops throwing any exception
        /// after a specified amount of time.
        /// </summary>
        /// <remarks>
        /// The <see cref="Action"/> is invoked. If it raises an exception,
        /// the invocation is repeated until it either stops raising any exceptions
        /// or the specified wait time is exceeded.
        /// </remarks>
        /// <param name="waitTime">
        /// The time after which the <see cref="Action"/> should have stopped throwing any exception.
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
        public void NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            FailIfSubjectIsAsyncVoid();

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
                exception = InvokeSubjectWithInterception();
                if (exception is null)
                {
                    return;
                }

                invocationEndTime = watch.Elapsed;
                Thread.Sleep(pollInterval);
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);
        }
#endif

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
