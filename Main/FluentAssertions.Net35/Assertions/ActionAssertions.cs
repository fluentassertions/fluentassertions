using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Action"/> yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class ActionAssertions
    {
        protected internal ActionAssertions(Action subject)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Action"/> that is being asserted.
        /// </summary>
        public Action Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> throws an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason, object[] reasonArgs)
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

            Execute.Verification
                .ForCondition(exception != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", typeof(TException));


            Execute.Verification
                .ForCondition(exception is TException)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", typeof(TException), exception);

            return new ExceptionAssertions<TException>((TException)exception);            
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> does not throw an exception of type <typeparamref name="TException"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public void ShouldNotThrow<TException>(string reason, object[] reasonArgs)
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
                Execute.Verification
                    .ForCondition(!(exception is TException))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {0}{reason}, but found one with message {1}.",
                        typeof (TException), exception.Message);
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Action"/> does not throw any exception.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public void ShouldNotThrow(string reason, object[] reasonArgs)
        {
            try
            {
                Subject();
            }
            catch (Exception exception)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect any exception{reason}, but found a {0} with message {1}.",
                        exception.GetType(), exception.Message);
            }
        }
    }
}
