namespace FluentAssertions.Execution
{
    /// <summary>
    /// Enables chaining multiple assertions from a <see cref="AssertionScope.Given{T}"/> call.
    /// </summary>
    public class ContinuationOfGiven<TSubject>
    {
        private readonly GivenSelector<TSubject> parent;
        private readonly bool succeeded;

        internal ContinuationOfGiven(GivenSelector<TSubject> parent, bool succeeded)
        {
            this.succeeded = succeeded;
            this.parent = parent;
        }

        /// <summary>
        /// Continuous the assertion chain if the previous assertion was successful.
        /// </summary>
        public GivenSelector<TSubject> Then => parent;

        /// <summary>
        /// Provides back-wards compatibility for code that expects <see cref="AssertionScope.FailWith(string, object[])"/> to return a boolean.
        /// </summary>
        public static implicit operator bool(ContinuationOfGiven<TSubject> continuationOfGiven)
        {
            return continuationOfGiven.succeeded;
        }
    }
}
