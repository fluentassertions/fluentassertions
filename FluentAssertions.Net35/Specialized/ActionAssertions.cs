using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Action"/> yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class ActionAssertions : ReferenceTypeAssertions<Action, ActionAssertions>
    {
        private readonly IExtractExceptions extractor;

        protected internal ActionAssertions(Action subject, IExtractExceptions extractor)
        {
            this.extractor = extractor;
            Subject = subject;
        }

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
        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason = "", params object[] reasonArgs)
            where TException : Exception
        {
            Exception actualException = InvokeSubjectWithInterception();
            IEnumerable<TException> expectedExceptions = extractor.OfType<TException>(actualException);

            Execute.Assertion
                .ForCondition(actualException != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", typeof(TException));

            Execute.Assertion
                .ForCondition(expectedExceptions.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", typeof(TException), actualException);

            return new ExceptionAssertions<TException>(expectedExceptions);            
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
        public void ShouldNotThrow<TException>(string reason = "", params object[] reasonArgs) where TException : Exception
        {
            Exception actualException = InvokeSubjectWithInterception();
            IEnumerable<TException> expectedExceptions = extractor.OfType<TException>(actualException);

            if (actualException != null)
            {
                Execute.Assertion
                    .ForCondition(!expectedExceptions.Any())
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {0}{reason}, but found {1}.", typeof (TException), actualException);
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
        public void ShouldNotThrow(string reason = "", params object[] reasonArgs)
        {
            try
            {
                Subject();
            }
            catch (Exception exception)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect any exception{reason}, but found {0}", exception);
            }
        }

        private Exception InvokeSubjectWithInterception()
        {
            Exception actualException = null;

            try
            {
                Subject();
            }
            catch (Exception exc)
            {
                actualException = exc;
            }
            return actualException;
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "action"; }
        }
    }

    public interface IExtractExceptions
    {
        IEnumerable<T> OfType<T>(Exception actualException) where T : Exception ;
    }
}
