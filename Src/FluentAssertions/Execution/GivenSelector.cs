using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents a chaining object returned from <see cref="AssertionScope.Given{T}"/> to continue the assertion using
    /// an object returned by a selector.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public class GivenSelector<T>
    {
        #region Private Definitions

        private readonly T subject;
        private readonly bool predecessorSucceeded;
        private readonly AssertionScope predecessor;

        #endregion

        public GivenSelector(Func<T> selector, bool predecessorSucceeded, AssertionScope predecessor)
        {
            this.predecessorSucceeded = predecessorSucceeded;
            this.predecessor = predecessor;

            subject = predecessorSucceeded ? selector() : default;
        }

        /// <summary>
        /// Specify the condition that must be satisfied upon the subject selected through a prior selector.
        /// </summary>
        /// <param name="predicate">
        /// If <c>true</c> the assertion will be treated as successful and no exceptions will be thrown.
        /// </param>
        /// <remarks>
        /// The condition will not be evaluated if the prior assertion failed,
        /// nor will <see cref="FailWith(string, System.Func{T, object}[])"/> throw any exceptions.
        /// </remarks>
        public GivenSelector<T> ForCondition(Func<T, bool> predicate)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            predecessor.ForCondition(predicate(subject));

            return this;
        }

        /// <summary>
        /// Allows to safely refine the subject for successive assertions, even when the prior assertion has failed.
        /// </summary>
        /// <paramref name="selector">
        /// Selector which result is passed to successive calls to <see cref="ForCondition"/>.
        /// </paramref>
        /// <remarks>
        /// The selector will not be invoked if the prior assertion failed,
        /// nor will <see cref="FailWith(string,System.Func{T,object}[])"/> throw any exceptions.
        /// </remarks>
        public GivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
        {
            Guard.ThrowIfArgumentIsNull(selector, nameof(selector));

            return new GivenSelector<TOut>(() => selector(subject), predecessorSucceeded, predecessor);
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a
        /// prior call to <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// If an expectation was set through a prior call to <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>,
        /// then the failure message is appended to that expectation.
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        public ContinuationOfGiven<T> FailWith(string message)
        {
            return FailWith(message, new object[0]);
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a
        /// prior call to <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed
        /// to <see cref="FluentAssertions.Execution.AssertionScope.BecauseOf"/>. Other named placeholders will be replaced with
        /// the <see cref="FluentAssertions.Execution.AssertionScope.Current"/> scope data passed through
        /// <see cref="FluentAssertions.Execution.AssertionScope.AddNonReportable"/> and
        /// <see cref="FluentAssertions.Execution.AssertionScope.AddReportable"/>. Finally, a description of the current subject
        /// can be passed through the {context:description} placeholder. This is used in the message if no explicit context
        /// is specified through the <see cref="AssertionScope"/> constructor.
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>,
        /// then the failure message is appended to that expectation.
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public ContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
        {
            object[] mappedArguments = args.Select(a => a(subject)).ToArray();
            return FailWith(message, mappedArguments);
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a
        /// prior call to <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string, object[])"/>-style placeholders, messages may contain
        /// a few specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as
        /// passed to <see cref="FluentAssertions.Execution.AssertionScope.BecauseOf"/>. Other named placeholders will be
        /// replaced with the <see cref="FluentAssertions.Execution.AssertionScope.Current"/> scope data passed through
        /// <see cref="FluentAssertions.Execution.AssertionScope.AddNonReportable"/> and
        /// <see cref="FluentAssertions.Execution.AssertionScope.AddReportable"/>. Finally, a description of the
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor.
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to
        /// <see cref="FluentAssertions.Execution.AssertionScope.WithExpectation"/>, then the failure message is appended
        /// to that expectation.
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public ContinuationOfGiven<T> FailWith(string message, params object[] args)
        {
            bool succeeded = predecessorSucceeded;

            if (predecessorSucceeded)
            {
                Continuation continuation = predecessor.FailWith(message, args);
                succeeded = continuation.SourceSucceeded;
            }

            return new ContinuationOfGiven<T>(this, succeeded);
        }

        /// <summary>
        /// Clears the expectation set by <see cref="WithExpectation"/>.
        /// </summary>
        public ContinuationOfGiven<T> ClearExpectation()
        {
            predecessor.ClearExpectation();
            return new ContinuationOfGiven<T>(this, predecessorSucceeded);
        }
    }
}
