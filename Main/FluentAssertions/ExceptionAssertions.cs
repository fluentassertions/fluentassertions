using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> : Assertions<Exception, ExceptionAssertions<TException>>
        where TException : Exception
    {
        protected internal ExceptionAssertions(TException exception)
        {
            ActualValue = exception;
        }

        /// <summary>
        /// Gets the exception object of the exception thrown.
        /// </summary>
        public TException And 
        { 
            get
            {
                return (TException) ActualValue; 
            }
        }

        /// <summary>
        /// Asserts that the thrown exception has a message matching the <paramref name="expectedMessage"/>.
        /// </summary>
        /// <param name="expectedMessage">The expected message of the exception.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithMessage(string expectedMessage)
        {
            return WithMessage(expectedMessage, null, null);
        }

        /// <summary>
        /// Asserts that the thrown exception has a message matching the <paramref name="expectedMessage"/>.
        /// </summary>
        /// <param name="expectedMessage">The expected message of the exception.</param>
        /// <param name="reason">
        /// The reason why the message of the exception should match the <paramref name="expectedMessage"/>.
        /// </param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedMessage, string reason,
            params object[] reasonParameters)
        {
            VerifyThat((ActualValue != null),
                "Expected exception with message {0}{2}, but no exception was thrown.",
                expectedMessage, null, reason, reasonParameters);

            string message = ActualValue.Message;
            int index = message.IndexOfFirstMismatch(expectedMessage);
            
            if (index != -1)
            {
                FailWith(
                    "Expected exception with message {0}{2}, but {1} differs near '" + message[index] + "' (index " + index + ").",
                    expectedMessage, message, reason, reasonParameters);
            }

            return this;
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception of type <typeparamref name="TInnerException"/>.
        /// </summary>
        /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerException<TInnerException>()
        {
            return WithInnerException<TInnerException>(null, null);
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception of type <typeparamref name="TInnerException"/>.
        /// </summary>
        /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name="reason">The reason why the inner exception should be of the supplied type.</param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public virtual ExceptionAssertions<TException> WithInnerException<TInnerException>(string reason,
            params object[] reasonParameters)
        {
            VerifyThat(ActualValue != null, "Expected inner {0}{2}, but no exception was thrown.",
                typeof(TInnerException), null, reason, reasonParameters);

            VerifyThat(ActualValue.InnerException != null,
                "Expected inner {0}{2}, but the thrown exception has no inner exception.",
                typeof(TInnerException), null, reason, reasonParameters);

            VerifyThat((ActualValue.InnerException.GetType() == typeof(TInnerException)),
                "Expected inner {0}{2}, but found {1}.",
                typeof(TInnerException),
                ActualValue.InnerException.GetType(),
                reason, reasonParameters);

            return this;
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception with the <paramref name="expectedInnerMessage"/>.
        /// </summary>
        /// <param name="expectedInnerMessage">The expected message of the inner exception.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage)
        {
            return WithInnerMessage(expectedInnerMessage, null, null);
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception with the <paramref name="expectedInnerMessage"/>.
        /// </summary>
        /// <param name="expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name="reason">
        /// The reason why the message of the inner exception should match <paramref name="expectedInnerMessage"/>.
        /// </param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public virtual ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(ActualValue != null, "Expected exception{2}, but no exception was thrown.",
                null, null, reason, reasonParameters);

            VerifyThat(ActualValue.InnerException != null,
                "Expected exception{2}, but the thrown exception has no inner exception.",
                null, null, reason, reasonParameters);

            string innerMessage = ActualValue.InnerException.Message;

            int index = innerMessage.IndexOfFirstMismatch(expectedInnerMessage);
            if (index != -1)
            {
                FailWith(
                    "Expected inner exception with message {0}{2}, but {1} differs near '" + innerMessage[index] + "' (index " + index + ").",
                    expectedInnerMessage,
                    innerMessage,
                    reason, reasonParameters);
            }

            return this;
        }
    }
}