using System;
using System.Diagnostics;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        [DebuggerNonUserCode]
        public class ExceptionAssertions<T> : Assertions<Exception, ExceptionAssertions<T>>
        {
            private readonly Exception exception;

            public ExceptionAssertions(T value, Action<T> action)
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

            public AndConstraint<ExceptionAssertions<T>> Exception<TException>()
            {
                return Exception<TException>(null, null);
            }

            public AndConstraint<ExceptionAssertions<T>> Exception<TException>(string reason,
                                                                               params object[] reasonParameters)
            {
                AssertThat(exception != null, "Expected exception <{0}>{2}, but no exception was thrown.",
                           typeof(TException), null, reason, reasonParameters);

                AssertThat(exception is TException,
                           "Expected exception <{0}>{2}, but found <{1}>.",
                           typeof(TException), exception.GetType(), reason, reasonParameters);

                return new AndConstraint<ExceptionAssertions<T>>(this);
            }

            public AndConstraint<ExceptionAssertions<T>> WithMessage(string expectedMessage)
            {
                return WithMessage(expectedMessage, null, null);
            }

            public AndConstraint<ExceptionAssertions<T>> WithMessage(string expectedMessage, string reason,
                                                                     params object[] reasonParameters)
            {
                AssertThat((exception != null),
                           "Expected exception with message <{0}>{2}, but no exception was thrown.",
                           expectedMessage, null, reason, reasonParameters);

                AssertThat(exception.Message == expectedMessage,
                           "Expected exception with message <{0}>{2}, but found <{1}>.",
                           expectedMessage, exception.Message, reason);

                return new AndConstraint<ExceptionAssertions<T>>(this);
            }

            public AndConstraint<ExceptionAssertions<T>> WithInnerException<TInnerException>()
            {
                return WithInnerException<TInnerException>(null, null);
            }

            public AndConstraint<ExceptionAssertions<T>> WithInnerException<TInnerException>(string reason,
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

                return new AndConstraint<ExceptionAssertions<T>>(this);
            }

            public AndConstraint<ExceptionAssertions<T>> WithInnerMessage(string expectedInnerMessage)
            {
                return WithInnerMessage(expectedInnerMessage, null, null);
            }

            public AndConstraint<ExceptionAssertions<T>> WithInnerMessage(string expectedInnerMessage, string reason,
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

                return new AndConstraint<ExceptionAssertions<T>>(this);
            }
        }
    }
}