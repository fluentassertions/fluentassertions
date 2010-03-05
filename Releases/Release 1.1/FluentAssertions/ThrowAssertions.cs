using System;

namespace FluentAssertions
{
    public static partial class FluentAssertionExtensions
    {
        public class ThrowAssertions<T> : Assertions<Exception, ThrowAssertions<T>>
        {
            protected Exception exception;

            internal ThrowAssertions(Action action)
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

            internal ThrowAssertions(T value, Action<T> action)
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

            /// <summary>
            /// Asserts that an exception is thrown of type <typeparamref name="TException"/>.
            /// </summary>
            /// <typeparam name="TException">The expected type of the exception.</typeparam>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions<TException>> Exception<TException>()
                where TException : Exception
            {
                return Exception<TException>(string.Empty);
            }

            /// <summary>
            /// Asserts that an exception is thrown of type <typeparamref name="TException"/>.
            /// </summary>
            /// <typeparam name="TException">The expected type of the exception.</typeparam>
            /// <param name="reason">The reason why the exception should be of type <typeparamref name="TException"/>.</param>
            /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
            public AndConstraint<ExceptionAssertions<TException>> Exception<TException>(string reason, params object[] reasonParameters)
                where TException : Exception
            {
                VerifyThat(exception != null, "Expected exception <{0}>{2}, but no exception was thrown.",
                           typeof(TException), null, reason, reasonParameters);

                VerifyThat(exception is TException,
                           "Expected exception <{0}>{2}, but found <{1}>.",
                           typeof(TException), exception.GetType(), reason, reasonParameters);

                return
                    new AndConstraint<ExceptionAssertions<TException>>(new ExceptionAssertions<TException>(exception as TException));
            }
        }
    }
}
