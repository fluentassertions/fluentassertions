using System;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Allows chaining multiple assertion scopes together using <see cref="Continuation.Then"/>.
    /// </summary>
    /// <remarks>
    /// If the parent scope has captured a failed assertion, this class ensures that successive assertions
    /// are no longer evaluated.
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

        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            if (continueAsserting)
            {
                return predecessor.Given(selector);
            }

            return new GivenSelector<T>(() => default, predecessor, continueAsserting: false);
        }

        public IAssertionScope ForCondition(bool condition)
        {
            if (continueAsserting)
            {
                return predecessor.ForCondition(condition);
            }

            return this;
        }

        public Continuation FailWith(string message)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        public Continuation FailWith(string message, params Func<object>[] argProviders)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message, argProviders);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(failReasonFunc);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        public Continuation FailWith(string message, params object[] args)
        {
            if (continueAsserting)
            {
                return predecessor.FailWith(message, args);
            }

            return new Continuation(predecessor, continueAsserting: false);
        }

        public IAssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            if (continueAsserting)
            {
                return predecessor.BecauseOf(because, becauseArgs);
            }

            return this;
        }

        public Continuation ClearExpectation()
        {
            predecessor.ClearExpectation();

            return new Continuation(predecessor, continueAsserting);
        }

        public IAssertionScope WithExpectation(string message, params object[] args)
        {
            return predecessor.WithExpectation(message, args);
        }

        public IAssertionScope WithDefaultIdentifier(string identifier)
        {
            return predecessor.WithDefaultIdentifier(identifier);
        }

        public IAssertionScope UsingLineBreaks => predecessor.UsingLineBreaks;

        public string[] Discard()
        {
            return predecessor.Discard();
        }

        public void Dispose()
        {
            predecessor.Dispose();
        }
    }
}
