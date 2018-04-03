namespace FluentAssertions.Execution
{
    /// <summary>
    /// Throws a generic exception in case no other test harness is detected.
    /// </summary>
    internal class FallbackTestFramework : ITestFramework
    {
        /// <summary>
        /// Gets a value indicating whether the corresponding test framework is currently available.
        /// </summary>
        public bool IsAvailable => true;

        /// <summary>
        /// Throws a framework-specific exception to indicate a failing unit test.
        /// </summary>
        public void Throw(string message)
        {
            throw new AssertionFailedException(message);
        }
    }
}
