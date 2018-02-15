namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Indication of how cyclic references should be handled when validating equality of nested properties.
    /// </summary>
    public enum CyclicReferenceHandling
    {
        /// <summary>
        /// Cyclic references will be ignored.
        /// </summary>
        Ignore,

        /// <summary>
        /// Cyclic references will result in an exception.
        /// </summary>
        ThrowException
    }
}
