using System;
using System.Diagnostics;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        [DebuggerNonUserCode]
        public class ExceptionAssertions : Assertions<Exception, ExceptionAssertions>
        {
            protected Exception exception;

            protected ExceptionAssertions()
            { }

            internal ExceptionAssertions(Action action)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            /// <summary>
            /// Asserts that an exception is thrown of type <typeparamref name="TException"/>.
            /// </summary>
            /// <typeparam name="TException">The expected type of the exception.</typeparam>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions> Exception<TException>()
            {
                return Exception<TException>(null, null);
            }

            /// <summary>
            /// Asserts that an exception is thrown of type <typeparamref name="TException"/>.
            /// </summary>
            /// <typeparam name="TException">The expected type of the exception.</typeparam>
            /// <param name="reason">The reason why the exception should be of type <typeparamref name="TException"/>.</param>
            /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions> Exception<TException>(string reason,
                                                                               params object[] reasonParameters)
            {
                AssertThat(exception != null, "Expected exception <{0}>{2}, but no exception was thrown.",
                           typeof(TException), null, reason, reasonParameters);

                AssertThat(exception is TException,
                           "Expected exception <{0}>{2}, but found <{1}>.",
                           typeof(TException), exception.GetType(), reason, reasonParameters);

                return new AndConstraint<ExceptionAssertions>(this);
            }

            /// <summary>
            /// Asserts that the thrown exception has a message matching the <paramref name="expectedMessage"/>.
            /// </summary>
            /// <param name="expectedMessage">The expected message of the exception.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions> WithMessage(string expectedMessage)
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
            public AndConstraint<ExceptionAssertions> WithMessage(string expectedMessage, string reason,
                                                                     params object[] reasonParameters)
            {
                AssertThat((exception != null),
                           "Expected exception with message <{0}>{2}, but no exception was thrown.",
                           expectedMessage, null, reason, reasonParameters);

                AssertThat(exception.Message == expectedMessage,
                           "Expected exception with message <{0}>{2}, but found <{1}>.",
                           expectedMessage, exception.Message, reason);

                return new AndConstraint<ExceptionAssertions>(this);
            }

            /// <summary>
            /// Asserts that the thrown exception contains an inner exception of type <typeparamref name="TInnerException"/>.
            /// </summary>
            /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions> WithInnerException<TInnerException>()
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
            public AndConstraint<ExceptionAssertions> WithInnerException<TInnerException>(string reason,
                                                                                             params object[] reasonParameters)
            {
                AssertThat(exception != null, "Expected inner exception <{0}>{2}, but no exception was thrown.",
                           typeof(TInnerException), null, reason, reasonParameters);

                AssertThat(exception.InnerException != null,
                           "Expected inner exception <{0}>{2}, but the thrown exception has no inner exception.",
                           typeof(TInnerException), null, reason, reasonParameters);

                AssertThat((exception.InnerException.GetType() == typeof(TInnerException)),
                           "Expected inner exception <{0}>{2}, but found <{1}>.",
                           typeof(TInnerException),
                           exception.InnerException.GetType(),
                           reason, reasonParameters);

                return new AndConstraint<ExceptionAssertions>(this);
            }

            /// <summary>
            /// Asserts that the thrown exception contains an inner exception with the <paramref name="expectedInnerMessage"/>.
            /// </summary>
            /// <param name="expectedInnerMessage">The expected message of the inner exception.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions> WithInnerMessage(string expectedInnerMessage)
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
            public AndConstraint<ExceptionAssertions> WithInnerMessage(string expectedInnerMessage, string reason,
                                                                          params object[] reasonParameters)
            {
                AssertThat(exception != null, "Expected exception{2}, but no exception was thrown.",
                           null, null, reason, reasonParameters);

                AssertThat(exception.InnerException != null,
                           "Expected exception{2}, but the thrown exception has no inner exception.",
                           null, null, reason, reasonParameters);

                AssertThat((exception.InnerException.Message == expectedInnerMessage),
                           "Expected inner exception with message <{0}>{2}, but found <{1}>.",
                           expectedInnerMessage,
                           exception.InnerException.Message,
                           reason, reasonParameters);

                return new AndConstraint<ExceptionAssertions>(this);
            }
        }

        [DebuggerNonUserCode]
        public class ExceptionAssertions<T> : ExceptionAssertions
        {
            internal ExceptionAssertions(T value, Action<T> action)
            {
                try
                {
                    action(value);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
        }
    }
}