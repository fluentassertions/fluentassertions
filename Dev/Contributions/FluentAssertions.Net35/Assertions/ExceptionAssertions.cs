using System;
using System.Diagnostics;
using System.Linq.Expressions;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> : ReferenceTypeAssertions<Exception, ExceptionAssertions<TException>>
        where TException : Exception
    {
        protected internal ExceptionAssertions(TException exception)
        {
            Subject = exception;
        }

        /// <summary>
        ///   Gets the exception object of the exception thrown.
        /// </summary>
        public TException And
        {
            get { return (TException) Subject; }
        }

        /// <summary>
        ///   Asserts that the thrown exception has a message matching the <paramref name = "expectedMessage" />.
        /// </summary>
        /// <param name = "expectedMessage">The expected message of the exception.</param>
        public ExceptionAssertions<TException> WithMessage(string expectedMessage)
        {
            return WithMessage(expectedMessage, null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception has a message matching the <paramref name = "expectedMessage" />.
        /// </summary>
        /// <param name = "expectedMessage">The expected message of the exception.</param>
        /// <param name = "reason">
        ///   The reason why the message of the exception should match the <paramref name = "expectedMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedMessage, string reason,
            params object[] reasonArgs)
        {
            Verification verification = Execute.Verification.BecauseOf(reason, reasonArgs).UsingLineBreaks;

            verification.ForCondition(Subject != null).FailWith(
                "Expected exception with message {1}{0}, but no exception was thrown.", expectedMessage);

            string message = Subject.Message;

            verification.ForCondition(!string.IsNullOrEmpty(message)).FailWith(
                "Expected exception with message {1}{0}, but message was empty.", expectedMessage);

            verification.ForCondition(message.Length >= expectedMessage.Length).FailWith(
                "Expected exception with message {1}{0}, but {2} is too short.", expectedMessage, message);

            int index = message.IndexOfFirstMismatch(expectedMessage);

            if (index != -1)
            {
                verification.FailWith(
                    "Expected exception with message {1}{0}, but {2} differs near " + message.IndexedSegmentAt(index) +
                        ".",
                    expectedMessage, message);
            }

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerException<TInnerException>()
        {
            return WithInnerException<TInnerException>(null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name = "reason">The reason why the inner exception should be of the supplied type.</param>
        /// <param name = "reasonParameters">The parameters used when formatting the <paramref name = "reason" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public virtual ExceptionAssertions<TException> WithInnerException<TInnerException>(string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(Subject != null, "Expected inner {0}{2}, but no exception was thrown.",
                typeof (TInnerException), null, reason, reasonParameters);

            Execute.Verify(Subject.InnerException != null,
                "Expected inner {0}{2}, but the thrown exception has no inner exception.",
                typeof (TInnerException), null, reason, reasonParameters);

            Execute.Verify((Subject.InnerException.GetType() == typeof (TInnerException)),
                "Expected inner {0}{2}, but found {1}.",
                typeof (TInnerException),
                Subject.InnerException.GetType(),
                reason, reasonParameters);

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage)
        {
            return WithInnerMessage(expectedInnerMessage, null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name = "reason">
        ///   The reason why the message of the inner exception should match <paramref name = "expectedInnerMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage, string reason,
            params object[] reasonArgs)
        {
            Verification verification = Execute.Verification.BecauseOf(reason, reasonArgs).UsingLineBreaks;

            verification.ForCondition(Subject != null).FailWith(
                "Expected exception {0}, but no exception was thrown.");

            verification.ForCondition(Subject.InnerException != null).FailWith(
                "Expected exception{0}, but the thrown exception has no inner exception.");

            string innerMessage = Subject.InnerException.Message;

            int index = innerMessage.IndexOfFirstMismatch(expectedInnerMessage);
            if (index != -1)
            {
                verification.FailWith(
                    "Expected inner exception with message {1}{0}, but {2} differs near " +
                        innerMessage.IndexedSegmentAt(index) + ".",
                    expectedInnerMessage, innerMessage);
            }

            return this;
        }

        /// <summary>
        ///   Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        ///   The condition that the exception must match.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression)
        {
            return Where(exceptionExpression, "");
        }

        /// <summary>
        ///   Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        ///   The condition that the exception must match.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string reason,
            params object[] reasonParameters)
        {
            Func<TException, bool> condition = exceptionExpression.Compile();
            if (!condition((TException) Subject))
            {
                Execute.Fail(
                    "Expected exception where {0}{2}, but the condition was not met.",
                    exceptionExpression.Body, null, reason, reasonParameters);
            }

            return this;
        }
    }
}