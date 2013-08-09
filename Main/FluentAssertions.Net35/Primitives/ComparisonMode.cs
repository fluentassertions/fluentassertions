using System;

using FluentAssertions.Specialized;

// ReSharper disable CheckNamespace
namespace FluentAssertions
// ReSharper restore CheckNamespace
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
        [Obsolete("The default is a case-insenstive wildcard substring assertion.")]
        Exact,

        /// <summary>
        /// The message must match except for the casing of the characters.
        /// </summary>
        [Obsolete("The default is a case-insenstive wildcard substring assertion.")]
        Equivalent,

        /// <summary>
        /// The message must start with the exact text, including the casing of the characters..
        /// </summary>
        /// <summary>
        /// The message must match exactly, including the casing of the characters.
        /// </summary>
        [Obsolete("The default is a case-insenstive wildcard substring assertion, so start with a *")]
        StartWith,

        /// <summary>
        /// The message must start with the text except for the casing of the characters.
        /// </summary>
        [Obsolete("Exception assertions are never case-sensitive. Use StartWith instead")]
        StartWithEquivalent,

        /// <summary>
        /// The message must contain the exact text.
        /// </summary>
        [Obsolete("The default is a case-insenstive wildcard substring assertion.")]
        Substring,

        /// <summary>
        /// The message must contain the text except for the casing of the characters.
        /// </summary>
        [Obsolete("The default is a case-insenstive wildcard substring assertion.")]
        EquivalentSubstring,

        /// <summary>
        /// The message must match a wildcard pattern consisting of ordinary characters as well as * and ?.
        /// </summary>
        Wildcard
    }
}