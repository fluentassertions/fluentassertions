using System;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Allows chaining multiple assertion scopes together using <see cref="Continuation.Then"/>.
    /// </summary>
    /// <remarks>
    /// If the parent scope has captured a failed assertion,
    /// this class ensures that successive assertions are no longer evaluated.
    /// </remarks>
    public sealed class ContinuedAssertionScope : IAssertionScope
    {
        private readonly AssertionScope predecessor;
        private readonly bool continueAsserting;

        internal ContinuedAssertionScope(AssertionScope predecessor, bool continueAsserting)
        {
            this.predecessor = predecessor;
            this.continueAsserting = continueAsserting;
        }

        /// <inheritdoc/>
        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            if (continueAsserting)
            {
                return predecessor.Given(selector);
            }

            return new GivenSelector<T>(() => default, predecessor, continueAsserting: false);
        }

        /// <inheritdoc/>
        public IAssertionScope ForCondition(bool condition)
        {
            if (continueAsserting)
            {
                return predecessor.ForCondition(condition);
            }

            return this;
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message, params Func<object>[] argProviders)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message, argProviders);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        /// <inheritdoc/>
        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(failReasonFunc);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message, params object[] args)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message, args);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        /// <inheritdoc/>
        public IAssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            if (continueAsserting)
            {
                return predecessor.BecauseOf(because, becauseArgs);
            }

            return this;
        }

        /// <inheritdoc/>
        public Continuation ClearExpectation()
        {
            predecessor.ClearExpectation();

            return new Continuation(predecessor, continueAsserting);
        }

        /// <inheritdoc/>
        public IAssertionScope WithExpectation(string message, params object[] args)
        {
            if (continueAsserting)
            {
                return predecessor.WithExpectation(message, args);
            }

            return this;
        }

        /// <inheritdoc/>
        public IAssertionScope WithDefaultIdentifier(string identifier)
        {
            if (continueAsserting)
            {
                return predecessor.WithDefaultIdentifier(identifier);
            }

            return this;
        }

        /// <inheritdoc/>
        public IAssertionScope UsingLineBreaks => predecessor.UsingLineBreaks;

        /// <inheritdoc/>
        public string[] Discard()
        {
            return predecessor.Discard();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            predecessor.Dispose();
        }
    }
}
