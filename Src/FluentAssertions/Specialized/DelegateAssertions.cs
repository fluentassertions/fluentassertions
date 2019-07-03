using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Localization;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    [DebuggerNonUserCode]
    public abstract class DelegateAssertions<TDelegate> : ReferenceTypeAssertions<Delegate, DelegateAssertions<TDelegate>> where TDelegate : Delegate
    {
        private readonly IExtractExceptions extractor;
        private readonly IClock clock;

        protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor) : this(@delegate, extractor, new Clock())
        {
        }

        private protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor, IClock clock) : base(@delegate)
        {
            this.clock = clock;
            this.extractor = extractor;
            Subject = @delegate;
        }

        /// <summary>
        /// Gets the <typeparamref name="TDelegate"/> that is being asserted.
        /// </summary>
        public new TDelegate Subject { get; }

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
            FailIfSubjectIsAsyncVoid();

            Exception exception = InvokeSubjectWithInterception();

            Type expectedType = typeof(TException);

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Exception_ExpectedXFormat + Resources.Exception_CommaButNoExceptionWasThrown,
                    expectedType);

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
            FailIfSubjectIsAsyncVoid();

            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime),
                    string.Format(Resources.TimeSpan_TheValueOfXMustBeNonNegativeFormat, nameof(waitTime)));
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval),
                    string.Format(Resources.TimeSpan_TheValueOfXMustBeNonNegativeFormat, nameof(pollInterval)));
            }

            TimeSpan? invocationEndTime = null;
            Exception exception = null;
            var timer = clock.StartTimer();

            while (invocationEndTime is null || invocationEndTime < waitTime)
            {
                exception = InvokeSubjectWithInterception();
                if (exception is null)
                {
                    return;
                }

                clock.Delay(pollInterval);
                invocationEndTime = timer.Elapsed;
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.TimeSpan_DidNotExpectAnyExceptionsAfterXFormat + Resources.Common_CommaButFoundYFormat,
                    waitTime, exception);
        }

        protected ExceptionAssertions<TException> Throw<TException>(Exception exception, string because, object[] becauseArgs)
            where TException : Exception
        {
            IEnumerable<TException> expectedExceptions = extractor.OfType<TException>(exception).ToArray();

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Exception_ExpectedAXToBeThrownFormat + Resources.Exception_CommaButNoExceptionWasThrown,
                    typeof(TException));

            Execute.Assertion
                .ForCondition(expectedExceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Exception_ExpectedAXToBeThrownFormat + Resources.Exception_CommaButFoundYZW,
                    typeof(TException), exception?.GetType(),
                    Environment.NewLine,
                    exception);

            return new ExceptionAssertions<TException>(expectedExceptions);
        }

        protected void NotThrow(Exception exception, string because, object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(exception is null)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Exception_DidNotExpectAnyException + Resources.Common_CommaButFoundXFormat,
                    exception);
        }

        protected void NotThrow<TException>(Exception exception, string because, object[] becauseArgs) where TException : Exception
        {
            var exceptions = extractor.OfType<TException>(exception);
            Execute.Assertion
                .ForCondition(!exceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Exception_DidNotExpectXFormat + Resources.Common_CommaButFoundYFormat,
                    typeof(TException), exception);

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
            if (Subject.GetMethodInfo().HasAttribute<AsyncStateMachineAttribute>())
            {
                throw new InvalidOperationException(Resources.Exception_CannotUseActionAssertionsOnAsyncVoidMethods);
            }
        }
    }
}
