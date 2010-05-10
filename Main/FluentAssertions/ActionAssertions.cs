using System;

namespace FluentAssertions
{
    public class ActionAssertions<TException> : Assertions<Action, ActionAssertions<TException>> where TException : Exception
    {
        private Exception exception;

        public ActionAssertions(Action action, string reason, object[] reasonParameters)
        {
            exception = null;

            try
            {
                action();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            VerifyThat(exception != null, "Expected {0}{2}, but no exception was thrown.",
                typeof(TException), null, reason, reasonParameters);

            VerifyThat(exception is TException,
                "Expected {0}{2}, but found {1}.",
                typeof(TException), exception.GetType(), reason, reasonParameters);
        }

        public ExceptionAssertions<TException> ExceptionAssertions
        {
            get
            {
                return new ExceptionAssertions<TException>((TException) exception);            
            }
        }
    }
}
