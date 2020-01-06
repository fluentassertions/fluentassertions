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
#if NET45
    [Serializable]
#endif
    public class ContinuedAssertionScope : IAssertionScope
    {
        private readonly AssertionScope predecessor;
        private readonly bool predecessorSucceeded;

        public ContinuedAssertionScope(AssertionScope predecessor, bool predecessorSucceeded)
        {
            this.predecessorSucceeded = predecessorSucceeded;
            this.predecessor = predecessor;
        }

        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            return predecessor.Given(selector);
        }

        public IAssertionScope ForCondition(bool condition)
        {
            if (predecessorSucceeded)
            {
                return predecessor.ForCondition(condition);
            }

            return this;
        }

        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            if (predecessorSucceeded)
            {
                return predecessor.FailWith(failReasonFunc);
            }

            return new Continuation(predecessor, false);
        }

        public Continuation FailWith(string message, params object[] args)
        {
            if (predecessorSucceeded)
            {
                return predecessor.FailWith(message, args);
            }

            return new Continuation(predecessor, false);
        }

        public IAssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            return predecessor.BecauseOf(because, becauseArgs);
        }

        public Continuation ClearExpectation()
        {
            predecessor.ClearExpectation();

            return new Continuation(predecessor, predecessorSucceeded);
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

        public bool Succeeded => predecessor.Succeeded;

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
