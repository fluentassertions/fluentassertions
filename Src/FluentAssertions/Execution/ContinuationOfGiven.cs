namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions from a <see cref="AssertionScope.Given{T}"/> call.
    /// </summary>
    public class ContinuationOfGiven<TSubject>
    {
        #region Private Definitions

        private readonly bool succeeded;

        #endregion

        public ContinuationOfGiven(GivenSelector<TSubject> parent, bool succeeded)
        {
            Then = parent;
            this.succeeded = succeeded;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was successful.
        /// </summary>
        public GivenSelector<TSubject> Then { get; }

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(ContinuationOfGiven<TSubject> continuationOfGiven)
        {
            return continuationOfGiven.succeeded;
        }
    }
}
