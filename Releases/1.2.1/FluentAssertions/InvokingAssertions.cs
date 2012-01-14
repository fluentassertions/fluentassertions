using System;

namespace FluentAssertions
{
    public class InvokingAssertions<T> : Assertions<Exception, InvokingAssertions<T>>
    {
        protected Exception exception;

        protected internal InvokingAssertions(T value, Action<T> action)
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
        public ExceptionAssertions<TException> ShouldThrow<TException>()
            where TException : Exception
        {
            return ShouldThrow<TException>(String.Empty);
        }

        /// <summary>
        /// Asserts that an exception is thrown of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The expected type of the exception.</typeparam>
        /// <param name="reason">The reason why the exception should be of type <typeparamref name="TException"/>.</param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason, params object[] reasonParameters)
            where TException : Exception
        {
            VerifyThat(exception != null, "Expected {0}{2}, but no exception was thrown.",
                typeof(TException), null, reason, reasonParameters);

            VerifyThat(exception is TException,
                "Expected {0}{2}, but found {1}.",
                typeof(TException), exception.GetType(), reason, reasonParameters);

            return new ExceptionAssertions<TException>(exception as TException);
        }
    }
}
