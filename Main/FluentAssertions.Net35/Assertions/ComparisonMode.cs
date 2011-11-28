namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Defines the way <see cref="ExceptionAssertions{TException}.WithMessage(string)"/> compares the expected exception 
    /// message with the actual one.
    /// </summary>
    public enum ComparisonMode
    {
        /// <summary>
        /// The message must match exactly, including the casing of the characters.
        /// </summary>
        Exact,

        /// <summary>
        /// The message must match except for the casing of the characters.
        /// </summary>
        Equivalent,

        /// <summary>
        /// The message must start with the exact text, including the casing of the characters..
        /// </summary>
        StartWith,

        /// <summary>
        /// The message must start with the text except for the casing of the characters.
        /// </summary>
        StartWithEquivalent,

        /// <summary>
        /// The message must contain the exact text.
        /// </summary>
        Substring,

        /// <summary>
        /// The message must contain the text except for the casing of the characters.
        /// </summary>
        EquivalentSubstring,

        /// <summary>
        /// The message must match a wildcard pattern consisting of ordinary characters as well as * and ?.
        /// </summary>
        Wildcard
    }
}