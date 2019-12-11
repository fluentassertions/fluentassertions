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
    [DebuggerNonUserCode]
    public abstract class DelegateAssertions<TDelegate> : ReferenceTypeAssertions<Delegate, DelegateAssertions<TDelegate>> where TDelegate : Delegate
    {
        private readonly IExtractExceptions extractor;

        protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor) : this(@delegate, extractor, new Clock())
        {
        }

        private protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor, IClock clock) : base(@delegate)
        {
            this.extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Subject = @delegate;
        }

        /// <summary>
        /// Gets the <typeparamref name="TDelegate"/> that is being asserted.
        /// </summary>
        public new TDelegate Subject { get; }

        private protected IClock Clock { get; }

        private protected virtual bool CanHandleAsync => false;

        /// <summary>
        /// Asserts that the current <see cref="Delegate" /> throws an exception of type <typeparamref name="TException"/>.
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
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to throw {0}{reason}, but found <null>.", typeof(TException));

            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return Throw<TException>(exception, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="Delegate" /> does not throw an exception of type <typeparamref name="TException"/>.
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
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw {0}{reason}, but found <null>.", typeof(TException));

            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            NotThrow<TException>(exception, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="Delegate" /> does not throw any exception.
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
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw{reason}, but found <null>.");

            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            NotThrow(exception, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="Delegate"/> throws an exception of the exact type <typeparamref name="TException"/> (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public ExceptionAssertions<TException> ThrowExactly<TException>(string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to throw exactly {0}{reason}, but found <null>.", typeof(TException));

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
        /// Asserts that the current <see cref="Delegate"/> stops throwing any exception
        /// after a specified amount of time.
        /// </summary>
        /// <remarks>
        /// The delegate is invoked. If it raises an exception,
        /// the invocation is repeated until it either stops raising any exceptions
        /// or the specified wait time is exceeded.
        /// </remarks>
        /// <param name="waitTime">
        /// The time after which the delegate should have stopped throwing any exception.
        /// </param>
        /// <param name="pollInterval">
        /// The time between subsequent invocations of the delegate.
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
            Execute.Assertion
                .ForCondition(Subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw after {0}{reason}, but found <null>.", waitTime);

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
            ITimer timer = Clock.StartTimer();

            while (invocationEndTime is null || invocationEndTime < waitTime)
            {
                exception = InvokeSubjectWithInterception();
                if (exception is null)
                {
                    return;
                }

                Clock.Delay(pollInterval);
                invocationEndTime = timer.Elapsed;
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);
        }

        protected ExceptionAssertions<TException> Throw<TException>(Exception exception, string because, object[] becauseArgs)
            where TException : Exception
        {
            TException[] expectedExceptions = extractor.OfType<TException>(exception).ToArray();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected a <{0}> to be thrown{reason}, ", typeof(TException))
                .ForCondition(exception != null)
                .FailWith("but no exception was thrown.")
                .Then
                .ForCondition(expectedExceptions.Any())
                .FailWith("but found <{0}>: {1}{2}.",
                    exception?.GetType(),
                    Environment.NewLine,
                    exception)
                .Then
                .ClearExpectation();

            return new ExceptionAssertions<TException>(expectedExceptions);
        }

        protected void NotThrow(Exception exception, string because, object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(exception is null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exception{reason}, but found {0}.", exception);
        }

        protected void NotThrow<TException>(Exception exception, string because, object[] becauseArgs) where TException : Exception
        {
            IEnumerable<TException> exceptions = extractor.OfType<TException>(exception);
            Execute.Assertion
                .ForCondition(!exceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {0}{reason}, but found {1}.", typeof(TException), exception);
        }

        protected abstract void InvokeSubject();

        private Exception InvokeSubjectWithInterception()
        {
            Exception actualException = null;

            try
            {
                InvokeSubject();
            }
            catch (Exception exc)
            {
                actualException = exc;
            }

            return actualException;
        }

        private void FailIfSubjectIsAsyncVoid()
        {
            if (!CanHandleAsync && Subject.GetMethodInfo().IsDecoratedWithOrInherit<AsyncStateMachineAttribute>())
            {
                throw new InvalidOperationException("Cannot use action assertions on an async void method. Assign the async method to a variable of type Func<Task> instead of Action so that it can be awaited.");
            }
        }
    }
}
