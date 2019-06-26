using System;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public class Continuation
    {
        private readonly AssertionScope sourceScope;

        public Continuation(AssertionScope sourceScope, bool sourceSucceeded)
        {
            this.sourceScope = sourceScope;
            SourceSucceeded = sourceSucceeded;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was successful.
        /// </summary>
        public IAssertionScope Then => new ContinuedAssertionScope(sourceScope, SourceSucceeded);

        public bool SourceSucceeded { get; }

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(Continuation continuation)
        {
            return continuation.SourceSucceeded;
        }
    }
}
