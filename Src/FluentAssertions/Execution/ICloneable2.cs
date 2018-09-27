namespace FluentAssertions.Execution
{
    /// <summary>
    /// Custom version of ICloneable that works on all frameworks.
    /// </summary>
    public interface ICloneable2
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object Clone();
    }
}
