namespace FluentAssertions.Execution
{
    /// <summary>
    /// Helper class for verifying a condition and/or throwing a test harness specific exception representing an assertion failure.
    /// </summary>
    public static class Execute
    {
        /// <summary>
        /// Gets an object that wraps and executes a conditional or unconditional verification.
        /// </summary>
        public static Verifier Verification
        {
            get { return Verifier.Current; }
        }
    }
}