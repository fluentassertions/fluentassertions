namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
    /// </summary>
    public class Continuation
    {
        private readonly AssertionScope sourceScope;
        private readonly bool continueAsserting;

        internal Continuation(AssertionScope sourceScope, bool continueAsserting)
        {
            this.sourceScope = sourceScope;
            this.continueAsserting = continueAsserting;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was successful.
        /// </summary>
        public IAssertionScope Then
        {
            get
            {
                return new ContinuedAssertionScope(sourceScope, continueAsserting);
            }
        }

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith(string, object[])"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(Continuation continuation)
        {
            return continuation.continueAsserting;
        }
    }
}
