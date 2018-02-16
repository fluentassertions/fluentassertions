namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents an abstraction of a particular test framework such as MSTest, nUnit, etc.
    /// </summary>
    internal interface ITestFramework
    {
        /// <summary>
        /// Gets a value indicating whether the corresponding test framework is currently available.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Throws a framework-specific exception to indicate a failing unit test.
        /// </summary>
        void Throw(string message);
    }
}
