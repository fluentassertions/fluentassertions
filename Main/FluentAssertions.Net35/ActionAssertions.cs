using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class ActionAssertions
    {
        protected internal ActionAssertions(Action subject)
        {
            Subject = subject;
        }

        public Action Subject { get; private set; }

        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason, object[] reasonParameters)
            where TException : Exception
        {
            Exception exception = null;

            try
            {
                Subject();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            Execute.Verify(exception != null, "Expected {0}{2}, but no exception was thrown.",
                typeof(TException), null, reason, reasonParameters);

            Execute.Verify(exception is TException,
                "Expected {0}{2}, but found {1}.",
                typeof(TException), exception.GetType(), reason, reasonParameters);

            return new ExceptionAssertions<TException>((TException)exception);            
        }

        public void ShouldNotThrow<TException>(string reason, object[] reasonParameters)
        {
            Exception exception = null;

            try
            {
                Subject();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            if (exception != null)
            {
                Execute.Verify(!(exception is TException),
                    "Did not except {0}{2}, but found one with message {1}.",
                    typeof (TException), exception.Message, reason, reasonParameters);
            }
        }

        public void ShouldNotThrow(string reason, object[] reasonParameters)
        {
            try
            {
                Subject();
            }
            catch (Exception exception)
            {
                Execute.Fail("Did not except any exception{2}, but found a {0} with message {1}.",
                    exception.GetType(), exception.Message, reason, reasonParameters);
            }
        }
    }
}
