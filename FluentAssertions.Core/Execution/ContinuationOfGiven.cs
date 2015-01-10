namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions from a <see cref="AssertionScope.Given{T}"/> call.
    /// </summary>
    public class ContinuationOfGiven<TSubject>
    {
        #region Private Definitions

        private readonly GivenSelector<TSubject> parent;
        private readonly AssertionScope scope;

        #endregion

        public ContinuationOfGiven(GivenSelector<TSubject> parent, AssertionScope scope)
        {
            this.parent = parent;
            this.scope = scope;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was succesful.
        /// </summary>
        public GivenSelector<TSubject> Then
        {
            get { return parent; }
        }

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(ContinuationOfGiven<TSubject> continuationOfGiven)
        {
            return continuationOfGiven.scope.Succeeded;
        }
    }
}