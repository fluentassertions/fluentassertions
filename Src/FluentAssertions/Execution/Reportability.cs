namespace FluentAssertions.Execution
{
    /// <summary>
    /// Determines whether data associated with an <see cref="AssertionScope"/> should be included in the assertion failure.
    /// </summary>
    internal enum Reportability
    {
        Hidden,
        Reportable
    }
}