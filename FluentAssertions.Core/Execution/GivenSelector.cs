using System;
using System.Linq;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents a chaining object returned from <see cref="AssertionScope.Given{T}"/> to continue the assertion using
    /// an object returned by a selector.
    /// </summary>
    public class GivenSelector<T>
    {
        #region Private Definitions

        private readonly T subject;
        private readonly bool evaluateCondition;
        private readonly AssertionScope parentScope;

        #endregion

        public GivenSelector(Func<T> selector, bool evaluateCondition, AssertionScope parentScope)
        {
            this.evaluateCondition = evaluateCondition;
            this.parentScope = parentScope;

            subject = evaluateCondition ? selector() : default(T);
        }

        /// <summary>
        /// Specify the condition that must be satisfied upon the subject selected through a prior selector.
        /// </summary>
        /// <param name="condition">
        /// If <c>true</c> the assertion will be treated as successful and no exceptions will be thrown.
        /// </param>
        /// <remarks>
        /// The condition will not be evaluated if the prior assertion failed, nor will <see cref="FailWith(string,System.Func{T,object}[])"/>
        /// throw any exceptions.
        /// </remarks>
        public GivenSelector<T> ForCondition(Func<T, bool> predicate)
        {
            if (evaluateCondition)
            {
                parentScope.ForCondition(predicate(subject));
            }

            return this;
        }

        /// <summary>
        /// Allows to safely refine the subject for successive assertions, even when the prior assertion has failed.
        /// </summary>
        /// <paramref name="selector">
        /// Selector which result is passed to successive calls to <see cref="ForCondition"/>.
        /// </paramref>
        /// <remarks>
        /// The selector will not be invoked if the prior assertion failed, nor will <see cref="FailWith(string,System.Func{T,object}[])"/>
        /// throw any exceptions.
        /// </remarks>
        public GivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
        {
            return new GivenSelector<TOut>(() => selector(subject), evaluateCondition, parentScope);
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a 
        /// prior call to to <see cref="WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation. 
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        public ContinuationOfGiven<T> FailWith(string message)
        {
            return FailWith(message, new object[0]);
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a 
        /// prior call to to <see cref="WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few 
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed 
        /// to <see cref="BecauseOf"/>. Other named placeholders will be replaced with the <see cref="Current"/> scope data 
        /// passed through <see cref="AddNonReportable"/> and <see cref="AddReportable"/>. Finally, a description of the 
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no 
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor. 
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation. 
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public ContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
        {
            return FailWith(message, args.Select(a => a(subject)).ToArray());
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a 
        /// prior call to to <see cref="WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few 
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed 
        /// to <see cref="BecauseOf"/>. Other named placeholders will be replaced with the <see cref="Current"/> scope data 
        /// passed through <see cref="AddNonReportable"/> and <see cref="AddReportable"/>. Finally, a description of the 
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no 
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor. 
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation. 
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public ContinuationOfGiven<T> FailWith(string message, params object[] args)
        {
            bool succeeded = parentScope.Succeeded;

            if (evaluateCondition)
            {
                Continuation continuation = parentScope.FailWith(message, args);
                succeeded = continuation.SourceSucceeded;
            }

            return new ContinuationOfGiven<T>(this, succeeded);
        }
    }
}