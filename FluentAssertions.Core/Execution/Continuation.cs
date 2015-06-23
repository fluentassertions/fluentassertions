namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions on an <see cref="AssertionScope"/>.
    /// </summary>
    public class Continuation
    {
        #region Private Definition

        private readonly AssertionScope sourceScope;
        private readonly bool sourceSucceeded;

        #endregion

        public Continuation(AssertionScope sourceScope, bool sourceSucceeded)
        {
            this.sourceScope = sourceScope;
            this.sourceSucceeded = sourceSucceeded;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was successful.
        /// </summary>
        public AssertionScope Then
        {
            get { return new AssertionScope(sourceScope, sourceSucceeded); }
        }

        public bool SourceSucceeded
        {
            get { return sourceSucceeded; }
        }

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(Continuation continuation)
        {
            return continuation.sourceSucceeded;
        }
    }
}