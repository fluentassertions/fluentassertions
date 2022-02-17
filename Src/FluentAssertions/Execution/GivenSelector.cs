using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents a chaining object returned from <see cref="AssertionScope.Given{T}"/> to continue the assertion using
    /// an object returned by a selector.
    /// </summary>
    public class GivenSelector<T>
    {
        private readonly AssertionScope predecessor;
        private readonly T subject;

        private bool continueAsserting;

        internal GivenSelector(Func<T> selector, AssertionScope predecessor, bool continueAsserting)
        {
            this.predecessor = predecessor;
            this.continueAsserting = continueAsserting;

            subject = continueAsserting ? selector() : default;
        }

        /// <summary>
        /// Specify the condition that must be satisfied upon the subject selected through a prior selector.
        /// </summary>
        /// <param name="predicate">
        /// If <c>true</c> the assertion will be treated as successful and no exceptions will be thrown.
        /// </param>
        /// <remarks>
        /// The condition will not be evaluated if the prior assertion failed,
        /// nor will <see cref="FailWith(string, Func{T, object}[])"/> throw any exceptions.
        /// </remarks>
        public GivenSelector<T> ForCondition(Func<T, bool> predicate)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            if (continueAsserting)
            {
                predecessor.ForCondition(predicate(subject));
            }

            return this;
        }

        /// <remarks>
        /// The <paramref name="selector"/> will not be invoked if the prior assertion failed,
        /// nor will <see cref="FailWith(string, Func{T,object}[])"/> throw any exceptions.
        /// </remarks>
        /// <inheritdoc cref="IAssertionScope.Given{T}"/>
        public GivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
        {
            Guard.ThrowIfArgumentIsNull(selector, nameof(selector));

            return new GivenSelector<TOut>(() => selector(subject), predecessor, continueAsserting);
        }

        /// <inheritdoc cref="IAssertionScope.FailWith(string)"/>
        public ContinuationOfGiven<T> FailWith(string message)
        {
            return FailWith(message, new object[0]);
        }

        /// <remarks>
        /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
        /// The <paramref name="args"/> will not be invoked if the prior assertion failed,
        /// nor will <see cref="FailWith(string, Func{T,object}[])"/> throw any exceptions.
        /// </remarks>
        /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
        public ContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
        {
            if (continueAsserting)
            {
                object[] mappedArguments = args.Select(a => a(subject)).ToArray();
                return FailWith(message, mappedArguments);
            }

            return new ContinuationOfGiven<T>(this, succeeded: false);
        }

        /// <remarks>
        /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
        /// The <paramref name="args"/> will not be invoked if the prior assertion failed,
        /// nor will <see cref="FailWith(string, object[])"/> throw any exceptions.
        /// </remarks>
        /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
        public ContinuationOfGiven<T> FailWith(string message, params object[] args)
        {
            if (continueAsserting)
            {
                continueAsserting = predecessor.FailWith(message, args);
                return new ContinuationOfGiven<T>(this, continueAsserting);
            }

            return new ContinuationOfGiven<T>(this, succeeded: false);
        }

        /// <inheritdoc cref="IAssertionScope.ClearExpectation()"/>
        public ContinuationOfGiven<T> ClearExpectation()
        {
            predecessor.ClearExpectation();
            return new ContinuationOfGiven<T>(this, continueAsserting);
        }
    }
}
