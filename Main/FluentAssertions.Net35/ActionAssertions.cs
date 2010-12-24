using System;

namespace FluentAssertions
{
    public class ActionAssertions : Assertions<Action, ActionAssertions> 
    {
        public ExceptionAssertions<TException> AssertItThrows<TException>(Action action, string reason, object[] reasonParameters)
            where TException : Exception
        {
            Exception exception = null;

            try
            {
                action();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            Verification.Verify(exception != null, "Expected {0}{2}, but no exception was thrown.",
                typeof(TException), null, reason, reasonParameters);

            Verification.Verify(exception is TException,
                "Expected {0}{2}, but found {1}.",
                typeof(TException), exception.GetType(), reason, reasonParameters);

            return new ExceptionAssertions<TException>((TException)exception);            
        }

        public void AssertItDoesNotThrow<TException>(Action action, string reason, object[] reasonParameters)
        {
            Exception exception = null;

            try
            {
                action();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            if (exception != null)
            {
                Verification.Verify(!(exception is TException),
                    "Did not except {0}{2}, but found one with message {1}.",
                    typeof (TException), exception.Message, reason, reasonParameters);
            }
        }

        public void AssertItDoesNotThrowAny(Action action, string reason, object[] reasonParameters)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                Verification.Fail("Did not except any exception{2}, but found a {0} with message {1}.",
                    exception.GetType(), exception.Message, reason, reasonParameters);
            }
        }
    }
}
